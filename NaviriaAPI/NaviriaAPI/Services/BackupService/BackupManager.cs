using Microsoft.Extensions.Options;
using NaviriaAPI.Options;
using System.Diagnostics;

namespace NaviriaAPI.Services;

public class BackupManager
{
    private readonly string _backupFolderPath;
    private readonly string _connectionString;
    private readonly ILogger<BackupManager> _logger;

    private const int BackupRetentionDays = 30;
    private const int MinDaysBetweenBackups = 7;

    public BackupManager(IOptions<MongoDbOptions> options, ILogger<BackupManager> logger)
    {
        _connectionString = options.Value.ConnectionString;
        _backupFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Backups");
        _logger = logger;
    }

    public async Task<bool> CreateBackupAsync()
    {
        if (RecentBackupExists())
        {
            _logger.LogInformation("Skipping backup: recent backup exists.");
            return false;
        }

        var backupDir = CreateNewBackupDirectory();
        var backupFile = Path.Combine(backupDir, "backup.gz");

        var args = $"--uri=\"{_connectionString}\" --db NaviriaDB --archive=\"{backupFile}\" --gzip";

        if (await RunProcessAsync("mongodump", args))
        {
            _logger.LogInformation("Backup created: {Path}", backupFile);
            CleanOldBackups();
            return true;
        }

        _logger.LogError("Backup failed.");
        return false;
    }

    private bool RecentBackupExists()
    {
        if (!Directory.Exists(_backupFolderPath)) return false;

        var recent = Directory.GetDirectories(_backupFolderPath)
            .Select(d => new DirectoryInfo(d))
            .OrderByDescending(d => d.CreationTime)
            .FirstOrDefault();

        return recent != null && (DateTime.UtcNow - recent.CreationTime).TotalDays < MinDaysBetweenBackups;
    }

    private string CreateNewBackupDirectory()
    {
        var name = $"backup_NaviriaDB_{DateTime.UtcNow:yyyyMMddHHmmss}";
        var path = Path.Combine(_backupFolderPath, name);
        Directory.CreateDirectory(path);
        return path;
    }

    private void CleanOldBackups()
    {
        if (!Directory.Exists(_backupFolderPath)) return;

        var backups = Directory.GetDirectories(_backupFolderPath)
            .Select(d => new DirectoryInfo(d))
            .OrderByDescending(d => d.CreationTime)
            .ToList();

        if (backups.Count <= 1) return;

        var toDelete = backups
            .Skip(1) // to left newest one 
            .Where(b => (DateTime.UtcNow - b.CreationTime).TotalDays > BackupRetentionDays);

        foreach (var b in toDelete)
        {
            try
            {
                Directory.Delete(b.FullName, true);
                _logger.LogInformation("Deleted old backup: {Path}", b.FullName);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to delete: {Path}", b.FullName);
            }
        }
    }

    private async Task<bool> RunProcessAsync(string fileName, string args)
    {
        try
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = args,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            await process.WaitForExitAsync();
            return process.ExitCode == 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Process execution failed.");
            return false;
        }
    }
}
