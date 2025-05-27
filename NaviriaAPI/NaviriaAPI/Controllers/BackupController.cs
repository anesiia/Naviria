using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Diagnostics;

namespace NaviriaAPI.Controllers
{
    /// <summary>
    /// API controller for database backup and restore operations.
    /// Provides endpoints to export, import, and list backups of the database or collections.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BackupController : ControllerBase
    {
        private readonly string _backupFolderPath;
        private readonly string _connectionString;
        private readonly ILogger<BackupController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackupController"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration for connection strings.</param>
        /// <param name="logger">Logger instance.</param>
        public BackupController(IConfiguration configuration, ILogger<BackupController> logger)
        {
            _backupFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Backups");
            _connectionString = configuration.GetConnectionString("MongoDB");
            _logger = logger;
        }

        /// <summary>
        /// Creates a backup of the entire database and returns the backup file.
        /// </summary>
        /// <returns>
        /// 200: The backup file in gzip format.<br/>
        /// 500: If the backup could not be created or an internal error occurred.
        /// </returns>
        [HttpPost("export-db-data")]
        public async Task<IActionResult> CreateBackup()
        {
            try
            {
                var backupFileName = GetBackupFileName();
                var backupFolder = await CreateBackupFolderAsync(backupFileName);

                _logger.LogInformation("CONNECTION STRING: {0}", _connectionString);

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

        /// <summary>
        /// Gets the list of available database backups.
        /// </summary>
        /// <returns>
        /// 200: A list of backup folders.<br/>
        /// 500: If an error occurs while retrieving the list.
        /// </returns>
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

        /// <summary>
        /// Restores the entire database from a specified backup file.
        /// </summary>
        /// <param name="backupFileName">The name of the backup file to restore.</param>
        /// <returns>
        /// 200: If the database was restored successfully.<br/>
        /// 400: If the backup file name is missing.<br/>
        /// 500: If an error occurs during restoration.
        /// </returns>
        [HttpPost("import-db-data")]
        public async Task<IActionResult> RestoreDBBackup([FromForm] string backupFileName)
        {
            if (string.IsNullOrWhiteSpace(backupFileName))
                return BadRequest("Backup file name is required.");

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

        /// <summary>
        /// Restores a collection from its latest backup.
        /// </summary>
        /// <param name="collectionName">The name of the collection to restore.</param>
        /// <returns>
        /// 200: If the collection was restored successfully.<br/>
        /// 400: If the collection name is missing.<br/>
        /// 404: If no backup for the collection is found.<br/>
        /// 500: If an error occurs during restoration.
        /// </returns>
        [HttpPost("import-collection")]
        public async Task<IActionResult> RestoreBackup([FromQuery] string collectionName)
        {
            if (string.IsNullOrEmpty(collectionName))
                return BadRequest("Collection name is required");

            try
            {
                var collectionBackupFolder = Path.Combine(_backupFolderPath, collectionName);
                if (!Directory.Exists(collectionBackupFolder))
                {
                    return NotFound($"No backups found for collection: {collectionName}");
                }

                var latestBackupFolder = Directory.GetDirectories(collectionBackupFolder)
                    .OrderByDescending(d => d)
                    .FirstOrDefault();

                if (string.IsNullOrEmpty(latestBackupFolder))
                {
                    return NotFound($"No valid backups found for collection: {collectionName}");
                }

                var processArgs = $"--uri=\"{_connectionString}\" --db=NaviriaDB --collection={collectionName} --drop \"{latestBackupFolder}\"";
                await RunProcessAsync("mongorestore", processArgs);

                return Ok("Collection restored successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to restore collection: {ex.Message}");
            }
        }

        private async Task RunProcessAsync(string fileName, string arguments)
        {
            using var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };
            process.Start();
            await process.WaitForExitAsync();
        }

        private static string GetBackupFileName()
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
    }
}
