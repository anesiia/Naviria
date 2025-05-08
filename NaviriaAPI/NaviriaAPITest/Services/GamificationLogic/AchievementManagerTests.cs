using Moq;
using NUnit.Framework;
using NaviriaAPI.Services.GamificationLogic;
using NaviriaAPI.IServices;
using NaviriaAPI.IServices.IGamificationLogic;
using NaviriaAPI.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NaviriaAPI.Tests.Services.GamificationLogic
{
    [TestFixture]
    public class AchievementManagerTests
    {
        private Mock<IAchievementStrategy> _strategyMock;
        private Mock<IAchievementGranter> _granterMock;
        private Mock<ILogger<AchievementManager>> _loggerMock;
        private AchievementManager _achievementManager;

        [SetUp]
        public void SetUp()
        {
            _strategyMock = new Mock<IAchievementStrategy>();
            _granterMock = new Mock<IAchievementGranter>();
            _loggerMock = new Mock<ILogger<AchievementManager>>();

            // Створення менеджера досягнень з моками
            _achievementManager = new AchievementManager(
                new List<IAchievementStrategy> { _strategyMock.Object },
                _granterMock.Object,
                _loggerMock.Object
            );
        }


        [Test]
        public async Task TC001_EvaluateAsync_ValidTrigger_CallsGiveAsync()
        {
            // Arrange
            var userId = "testUserId";
            var achievementIds = new List<string> { "Achievement1", "Achievement2" };
            _strategyMock.Setup(s => s.Trigger).Returns(AchievementTrigger.OnRegistration);
            _strategyMock.Setup(s => s.GetAchievementIdsAsync(userId, null)).ReturnsAsync(achievementIds);

            // Act
            await _achievementManager.EvaluateAsync(userId, AchievementTrigger.OnRegistration);

            // Assert
            _granterMock.Verify(g => g.GiveAsync(userId, "Achievement1"), Times.Once());
            _granterMock.Verify(g => g.GiveAsync(userId, "Achievement2"), Times.Once());
        }


        [Test]
        public async Task TC002_EvaluateAsync_GranterThrowsException_LogsError()
        {
            // Arrange
            var userId = "testUserId";
            var achievementIds = new List<string> { "Achievement1" };
            _strategyMock.Setup(s => s.Trigger).Returns(AchievementTrigger.OnRegistration);
            _strategyMock.Setup(s => s.GetAchievementIdsAsync(userId, null)).ReturnsAsync(achievementIds);
            _granterMock.Setup(g => g.GiveAsync(userId, "Achievement1")).ThrowsAsync(new Exception("Granter error"));

            // Act
            await _achievementManager.EvaluateAsync(userId, AchievementTrigger.OnRegistration);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to give achievement with ID Achievement1")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }



        [Test]
        public async Task TC003_EvaluateAsync_ValidTriggerButNoAchievements_DoesNotCallGiveAsync()
        {
            // Arrange
            var userId = "testUserId";
            var achievementIds = new List<string>(); // Немає досягнень
            _strategyMock.Setup(s => s.Trigger).Returns(AchievementTrigger.OnRegistration);
            _strategyMock.Setup(s => s.GetAchievementIdsAsync(userId, null)).ReturnsAsync(achievementIds);

            // Act
            await _achievementManager.EvaluateAsync(userId, AchievementTrigger.OnRegistration);

            // Assert
            _granterMock.Verify(g => g.GiveAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }
    }
}