using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using NaviriaAPI.Data;
using System.Diagnostics;

namespace NaviriaAPI.Controllers
{
    //restore last collection
    //restore certain collection
    //same for full db
    //get collections names
    public class BackupController : ControllerBase
    {
        private readonly string _backupFolderPath;
        private readonly string _connectionString;
        private readonly ILogger<BackupController> _logger;

        public BackupController(IConfiguration configuration, ILogger<BackupController> logger)
        {
            _backupFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Backups");
            _connectionString = configuration.GetConnectionString("MongoDB");
            _logger = logger;
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

        /*
        [HttpGet("collections")]
        public IActionResult GetCollections()
        {
            try
            {
                var collectionNames = _database.ListCollectionNames().ToList();
                return Ok(collectionNames);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to get collections: {ex.Message}");
            }
        }*/

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

        [HttpPost("export-db-data")]
        public async Task<IActionResult> CreateBackup()
        {
            try
            {
                var backupFileName = GetBackupFileName();
                var backupFolder = await CreateBackupFolderAsync(backupFileName);

                Console.WriteLine(_connectionString);
                _logger.LogInformation("CONNECTION STRING: " + _connectionString);


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

        [HttpPost("import-db-data")]
        public async Task<IActionResult> RestoreDBBackup([FromForm] string backupFileName)
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

        /*
        [HttpGet("collections")]
        public IActionResult GetCollections()
        {
            try
            {
                var collections = new MongoClient(_connectionString).GetDatabase("NaviriaDB").ListCollectionNames().ToList();
                //var collections = _database.ListCollectionNames().ToList();
                return Ok(collections);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve collections: {ex.Message}");
            }
        }*/


        /*
        [HttpPost("export-collection")]
        public async Task<IActionResult> CreateBackup([FromQuery] string collectionName)
        {
            try
            {
                if (string.IsNullOrEmpty(collectionName))
                {
                    return BadRequest("Collection name is required");
                }
                var backupFolder = Path.Combine(_backupFolderPath, collectionName, DateTime.Now.ToString("yyyyMMddHHmmss"));
                Directory.CreateDirectory(backupFolder);

                var processArgs = $"--uri=\"{_connectionString}\" --db=NaviriaDB --collection={collectionName} --out=\"{backupFolder}\"";
                await RunProcessAsync("mongodump", processArgs);

                return Ok($"Backup created successfully: {backupFolder}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to create backup: {ex.Message}");
            }
        }*/

        /*
        [HttpPost("export-collection")]
        public async Task<IActionResult> CreateBackup([FromQuery] string collectionName)
        {
            try
            {
                if (string.IsNullOrEmpty(collectionName))
                {
                    return BadRequest("Collection name is required");
                }

                var collectionBackupFolder = Path.Combine(_backupFolderPath, collectionName);
                Directory.CreateDirectory(collectionBackupFolder);
                var timestampedBackupFolder = Path.Combine(collectionBackupFolder, DateTime.Now.ToString("yyyyMMddHHmmss"));
                Directory.CreateDirectory(timestampedBackupFolder);

                var processArgs = $"--uri=\"{_connectionString}\" --db NaviriaDB --archive=\"" +
                    $"{Path.Combine(backupFolder, "backup.gz")}\" --gzip";


                var processArgs = $"--uri=\"{_connectionString}\" --db=NaviriaDB --collection={collectionName}" +
                    $" --out=\"{timestampedBackupFolder}\"";
                await RunProcessAsync("mongodump", processArgs);

                return Ok($"Backup created successfully: {timestampedBackupFolder}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to create backup: {ex.Message}");
            }
        }
        */


        /*

        [HttpPost("import-collection")]
        public async Task<IActionResult> RestoreBackup([FromQuery] string collectionName, [FromQuery] string backupFolderPath)
        {
            try
            {
                if (string.IsNullOrEmpty(collectionName) || string.IsNullOrEmpty(backupFolderPath))
                {
                    return BadRequest("Collection name and backup folder path are required");
                }
                var processArgs = $"--uri=\"{_connectionString}\" --db=NaviriaDB --collection={collectionName} --drop \"{backupFolderPath}\"";
                await RunProcessAsync("mongorestore", processArgs);

                return Ok("Collection restored successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to restore collection: {ex.Message}");
            }
        }*/

        [HttpPost("import-collection")]
        public async Task<IActionResult> RestoreBackup([FromQuery] string collectionName)
        {
            try
            {
                if (string.IsNullOrEmpty(collectionName))
                {
                    return BadRequest("Collection name is required");
                }

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
    }

}

