using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NaviriaAPI.Services.BackupService;

public class WeeklyBackupService : BackgroundService
{
    private readonly ILogger<WeeklyBackupService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public WeeklyBackupService(ILogger<WeeklyBackupService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var backupManager = scope.ServiceProvider.GetRequiredService<BackupManager>();

                _logger.LogInformation("Checking if backup is needed...");
                await backupManager.CreateBackupAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in scheduled backup.");
            }

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken); // start check daily
        }
    }
}