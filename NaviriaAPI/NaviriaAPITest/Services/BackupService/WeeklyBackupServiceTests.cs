using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NaviriaAPI.Services;
using NaviriaAPI.Services.BackupService;
using NUnit.Framework;

namespace NaviriaAPI.Tests.Services.BackupService
{

    //[TestFixture]
    //public class WeeklyBackupServiceTests
    //{
    //    private Mock<ILogger<WeeklyBackupService>> _mockLogger;
    //    private Mock<BackupManager> _mockBackupManager;
    //    private Mock<IServiceScope> _mockScope;
    //    private Mock<IServiceScopeFactory> _mockScopeFactory;
    //    private Mock<IServiceProvider> _mockServiceProvider;

    //    [SetUp]
    //    public void Setup()
    //    {
    //        _mockLogger = new Mock<ILogger<WeeklyBackupService>>();
    //        _mockBackupManager = new Mock<BackupManager>(null!, null!) { CallBase = false };

    //        _mockScope = new Mock<IServiceScope>();
    //        _mockScopeFactory = new Mock<IServiceScopeFactory>();
    //        _mockServiceProvider = new Mock<IServiceProvider>();

    //        _mockScope.Setup(s => s.ServiceProvider).Returns(_mockServiceProvider.Object);
    //        _mockScopeFactory.Setup(f => f.CreateScope()).Returns(_mockScope.Object);

    //        // Provide the ScopeFactory when requested
    //        _mockServiceProvider
    //            .Setup(sp => sp.GetService(typeof(IServiceScopeFactory)))
    //            .Returns(_mockScopeFactory.Object);

    //        // Provide the BackupManager when requested
    //        _mockServiceProvider
    //.Setup(sp => sp.GetService(typeof(BackupManager)))
    //.Returns(_mockBackupManager.Object);
    //    }

    //    [Test]
    //    public async Task TC001_ExecuteAsync_CallsBackupManagerOnce_WhenServiceStarts()
    //    {
    //        // Arrange
    //        _mockBackupManager
    //            .Setup(b => b.CreateBackupAsync())
    //            .ReturnsAsync(true);

    //        var stoppingTokenSource = new CancellationTokenSource();
    //        stoppingTokenSource.CancelAfter(200); // Cancel quickly to simulate one iteration

    //        var service = new WeeklyBackupService(_mockLogger.Object, _mockServiceProvider.Object);

    //        // Act
    //        await service.StartAsync(stoppingTokenSource.Token);
    //        await Task.Delay(300); // Give time for one iteration
    //        await service.StopAsync(CancellationToken.None);

    //        // Assert
    //        _mockBackupManager.Verify(b => b.CreateBackupAsync(), Times.Once);
    //    }

    //    [Test]
    //    public async Task TC002_ExecuteAsync_LogsError_WhenExceptionIsThrown()
    //    {
    //        // Arrange
    //        _mockBackupManager
    //            .Setup(b => b.CreateBackupAsync())
    //            .ThrowsAsync(new Exception("Simulated failure"));

    //        var stoppingTokenSource = new CancellationTokenSource();
    //        stoppingTokenSource.CancelAfter(200);

    //        var service = new WeeklyBackupService(_mockLogger.Object, _mockServiceProvider.Object);

    //        // Act
    //        await service.StartAsync(stoppingTokenSource.Token);
    //        await Task.Delay(300);
    //        await service.StopAsync(CancellationToken.None);

    //        // Assert
    //        _mockLogger.Verify(
    //            x => x.Log(
    //                It.Is<LogLevel>(l => l == LogLevel.Error),
    //                It.IsAny<EventId>(),
    //                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Simulated failure")),
    //                It.IsAny<Exception>(),
    //                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
    //            ),
    //            Times.AtLeastOnce
    //        );
    //    }
    //}
}
