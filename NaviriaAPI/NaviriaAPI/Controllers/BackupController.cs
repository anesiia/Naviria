using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver.Core.Configuration;
using System.Diagnostics;

namespace NaviriaAPI.Controllers
{
    public class BackupController : ControllerBase
    {
        private readonly string _backupFolderPath;
        private readonly string _connectionString;

        public BackupController(IConfiguration configuration)
        {
            _backupFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Backups");
            _connectionString = configuration.GetConnectionString("MongoDB");
        }

        private async Task RunProcessAsync(string fileName, string arguments)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = fileName;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                await process.WaitForExitAsync();
            }
        }

        private string GetBackupFileName()
        {
            return $"backup_NaviriaDB_{DateTime.Now:yyyyMMddHHmmss}";
        }

        private async Task<string> CreateBackupFolderAsync(string backupFileName)
        {
            var backupFolder = Path.Combine(_backupFolderPath, backupFileName);
            Directory.CreateDirectory(backupFolder);
            await Task.Delay(2000);
            return backupFolder;
        }
        private string GetLatestBsonFile(string backupFolder)
        {
            var bsonFiles = Directory.GetFiles(backupFolder, "*.bson");
            return bsonFiles.FirstOrDefault();
        }

        [HttpPost("export-data")]
        public async Task<IActionResult> CreateBackup()
        {
            try
            {
                var backupFileName = GetBackupFileName();
                var backupFolder = await CreateBackupFolderAsync(backupFileName);

                var processArgs = $"--uri=\"{_connectionString}\" --db NaviriaDB --archive=\"" +
                    $"{Path.Combine(backupFolder, "backup.gz")}\" --gzip";
                
                await RunProcessAsync("mongodump", processArgs);

                var bsonFile = Directory.GetFiles(backupFolder, "*.gz").FirstOrDefault();


                if (string.IsNullOrEmpty(bsonFile))
                {
                    return StatusCode(500, $"Failed to create backup: Backup file not found.");
                }

                var fileContents = System.IO.File.ReadAllBytes(bsonFile);
                var contentType = "application/octet-stream";

                return File(fileContents, contentType, Path.GetFileName(bsonFile));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to create backup: {ex.Message}");
            }
        }

        [HttpGet("list")]
        public IActionResult GetBackupList()
        {
            try
            {
                if (!Directory.Exists(_backupFolderPath))
                {
                    return Ok(new string[] { });
                }

                var backupFolders = Directory.GetDirectories(_backupFolderPath)
                    .Select(Path.GetFileName)
                    .OrderByDescending(folder => folder);

                return Ok(backupFolders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to get backup list: {ex.Message}");
            }
        }

        [HttpPost("import-data")]
        public async Task<IActionResult> RestoreBackup([FromForm] string backupFileName)
        {
            try
            {
                var backupFilePath = Path.Combine(_backupFolderPath, backupFileName);
                var processArgs = $"--drop --db NaviriaDB \"{backupFilePath}\"";
                await RunProcessAsync("mongorestore", processArgs);

                return Ok("Database restored successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to restore database: {ex.Message}");
            }
        }

    }
}
