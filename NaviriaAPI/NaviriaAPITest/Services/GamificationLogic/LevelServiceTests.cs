using Moq;
using NaviriaAPI.Services;
using NaviriaAPI.IServices;
using NaviriaAPI.DTOs;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.Entities.EmbeddedEntities;
using NUnit.Framework;

namespace NaviriaAPI.Tests.Services.GamificationLogic
{
    [TestFixture]
    public class LevelServiceTests
    {
        private LevelService _levelService;
        private Mock<INotificationService> _notificationServiceMock;

        [SetUp]
        public void Setup()
        {
            _notificationServiceMock = new Mock<INotificationService>();
            _levelService = new LevelService(_notificationServiceMock.Object);
        }

        private int GetXpForLevel(int level)
        {
            double rawXp = 50 * Math.Pow(level, 2.2);
            return (int)Math.Ceiling(rawXp / 10.0) * 10;
        }

        [Test]
        public void TC001_CalculateLevelProgressAsync_ShouldThrowArgumentNullException_WhenUserIsNull()
        {
            // Arrange
            UserDto user = null;

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _levelService.CalculateLevelProgressAsync(user, 100));

            Assert.That(ex!.Message, Does.Contain("User is not found"));
        }

        [TestCase(0)]
        [TestCase(49)]
        [TestCase(100)]
        [TestCase(1000)]
        public void TC002_BuildLevelProgress_ProgressShouldBeBetweenZeroAndOne(int xp)
        {
            // Act
            var result = InvokeBuildLevelProgress(xp);

            // Assert
            Assert.That(result.Progress, Is.InRange(0.0, 1.0));
        }


        [TestCase(0, 0)]
        [TestCase(10, 0)]
        [TestCase(50, 1)]
        [TestCase(100, 1)]
        [TestCase(231, 2)]
        [TestCase(400, 2)]
        [TestCase(1000, 3)] //
        public void TC003_CalculateLevelProgress_ShouldReturnCorrectLevel(int xp, int expectedLevel)
        {
            // Act
            var result = InvokeBuildLevelProgress(xp);

            // Assert
            Assert.That(result.Level, Is.EqualTo(expectedLevel));
            Assert.That(result.TotalXp, Is.EqualTo(xp));
            Assert.That(result.XpForNextLevel, Is.GreaterThan(xp));
            Assert.That(result.Progress, Is.GreaterThanOrEqualTo(0.0).And.LessThanOrEqualTo(1.0));
        }

        [Test]
        public async Task TC004_LevelUpNotification_ShouldBeSent_WhenLevelIncreases()
        {
            // Arrange
            var user = new UserDto { Id = "user1", Points = 0 };
            int additionalXp = GetXpForLevel(2); // Enough XP to reach level 2

            // Act
            var result = await _levelService.CalculateLevelProgressAsync(user, additionalXp);

            // Assert
            _notificationServiceMock.Verify(s =>
                s.CreateAsync(It.Is<NotificationCreateDto>(n =>
                    n.UserId == "user1" && n.Text.Contains("рівня"))), Times.Once);

            Assert.That(result.Level, Is.EqualTo(2));
        }

        [Test]
        public async Task TC005_NoNotification_WhenLevelDoesNotChange()
        {
            // Arrange
            var user = new UserDto { Id = "user1", Points = GetXpForLevel(2) };
            int additionalXp = 5; // Not enough to reach level 3

            // Act
            var result = await _levelService.CalculateLevelProgressAsync(user, additionalXp);

            // Assert
            _notificationServiceMock.Verify(s => s.CreateAsync(It.IsAny<NotificationCreateDto>()), Times.Never);
            Assert.That(result.Level, Is.EqualTo(2));
        }

        [Test]
        public void TC006_GetXpForLevel_ShouldReturnIncreasingXp()
        {
            // Arrange & Act
            int xp1 = GetXpForLevel(1);
            int xp2 = GetXpForLevel(2);
            int xp3 = GetXpForLevel(3);

            // Assert
            Assert.That(xp1, Is.LessThan(xp2));
            Assert.That(xp2, Is.LessThan(xp3));
        }

        [Test]
        public void TC008_GetXpForLevel_ShouldBeMultipleOfTen()
        {
            // Act & Assert
            for (int level = 1; level <= 10; level++)
            {
                int xp = GetXpForLevel(level);
                Assert.That(xp % 10, Is.EqualTo(0), $"XP for level {level} should be divisible by 10");
            }
        }

        [TestCase(0)]
        [TestCase(10)]
        [TestCase(50)]
        [TestCase(100)]
        [TestCase(200)]
        [TestCase(400)]
        [TestCase(1000)]
        public void TC009_PrintLevelProgressCalculations(int xp)
        {
            // Act
            var result = InvokeBuildLevelProgress(xp);

            // Output level thresholds for current and next level
            int currentLevelXp = GetXpForLevel(result.Level);
            int nextLevelXp = GetXpForLevel(result.Level + 1);

            // Print output to console
            TestContext.WriteLine($"XP: {xp}");
            TestContext.WriteLine($"Level: {result.Level}");
            TestContext.WriteLine($"Progress: {result.Progress:P2}"); // in percentage
            TestContext.WriteLine($"TotalXp: {result.TotalXp}");
            TestContext.WriteLine($"XpForNextLevel: {result.XpForNextLevel}");
            TestContext.WriteLine($"XP required for current level ({result.Level}): {currentLevelXp}");
            TestContext.WriteLine($"XP required for next level ({result.Level + 1}): {nextLevelXp}");
            TestContext.WriteLine(new string('-', 50));

            // Assert just to ensure values are valid
            Assert.That(result.Level, Is.GreaterThanOrEqualTo(0));
        }


        // Helper to call private method via reflection for unit testing internal logic
        private LevelProgressInfo InvokeBuildLevelProgress(int xp)
        {
            var method = typeof(LevelService).GetMethod("BuildLevelProgress", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (LevelProgressInfo)method.Invoke(_levelService, new object[] { xp });
        }
    }
}