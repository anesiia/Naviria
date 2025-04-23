using NaviriaAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviriaAPI.Tests.Services.GamificationLogic
{
    [TestFixture]
    public class LevelServiceTests
    {
        private LevelService _levelService;

        [SetUp]
        public void Setup()
        {
            _levelService = new LevelService();
        }

        private int GetXpForLevel(int level)
        {
            double rawXp = 50 * Math.Pow(level, 2.2);
            return (int)Math.Ceiling(rawXp / 10.0) * 10;
        }

        [Test]
        public void TC001_CalculateLevelProgress_ShouldReturnCorrectLevel_WhenXpIsZero()
        {
            // Arrange
            int xp = 0;

            // Act
            var result = _levelService.CalculateLevelProgress(xp);

            // Assert
            Assert.That(result.Level, Is.EqualTo(0));
            Assert.That(result.TotalXp, Is.EqualTo(xp));
            Assert.That(result.XpForNextLevel, Is.GreaterThan(0));
            Assert.That(result.Progress, Is.EqualTo(0.0));
        }

        [Test]
        public void TC002_CalculateLevelProgress_ShouldReturnCorrectLevel_WhenXpIsEqualToNextLevelXp()
        {
            // Arrange
            int xp = 50;

            // Act
            var result = _levelService.CalculateLevelProgress(xp);

            // Assert
            Assert.That(result.Level, Is.EqualTo(1));  // Level 1 should be reached after 50 XP
            Assert.That(result.TotalXp, Is.EqualTo(xp));
            Assert.That(result.XpForNextLevel, Is.GreaterThan(xp));
            Assert.That(result.Progress, Is.EqualTo(0.0));  // Full progress at the current level
        }

        [Test]
        public void TC003_CalculateLevelProgress_ShouldReturnCorrectLevel_WhenXpIsInBetweenLevels()
        {
            // Arrange
            int xp = 100;  // This should be between levels 1 and 2

            // Act
            var result = _levelService.CalculateLevelProgress(xp);

            // Assert
            Assert.That(result.Level, Is.EqualTo(1));  // Still level 1
            Assert.That(result.TotalXp, Is.EqualTo(xp));
            Assert.That(result.XpForNextLevel, Is.GreaterThan(xp));
            Assert.That(result.Progress, Is.GreaterThan(0.0).And.LessThan(1.0));  // Some progress into level 2
        }

        [Test]
        public void CalculateLevelProgress_ShouldReturnCorrectProgress_WhenXpIsMaxForLevel()
        {
            // Arrange
            int xpForLevel2 = GetXpForLevel(2); // припустимо, що рівень 2 починається з цього XP
            int xpForLevel3 = GetXpForLevel(3); // а рівень 3 — з цього XP

            int xp = xpForLevel3 - 1; // максимум для рівня 2

            // Act
            var result = _levelService.CalculateLevelProgress(xp);

            // Assert
            Assert.That(result.Level, Is.EqualTo(2));
            Assert.That(result.TotalXp, Is.EqualTo(xp));
            Assert.That(result.XpForNextLevel, Is.EqualTo(xpForLevel3));
            Assert.That(result.Progress, Is.EqualTo(1.00).Within(0.01));
        }

        [Test]
        public void TC005_CalculateLevelProgress_ShouldHandleLargeXpValues()
        {
            // Arrange
            int xp = 10000;  // Test a large XP value

            // Act
            var result = _levelService.CalculateLevelProgress(xp);

            // Assert
            Assert.That(result.Level, Is.GreaterThan(0));  // Should be at least level 3 or higher
            Assert.That(result.TotalXp, Is.EqualTo(xp));
            Assert.That(result.XpForNextLevel, Is.GreaterThan(xp));
            Assert.That(result.Progress, Is.GreaterThan(0.0).And.LessThan(1.0));  // In-progress into the next level
        }

        [Test]
        public void TC006_CalculateLevelProgress_ShouldReturnCorrectProgress_WhenXpExceedsCurrentLevel()
        {
            // Arrange
            int xp = 250;  

            // Act
            var result = _levelService.CalculateLevelProgress(xp);

            // Assert
            Assert.That(result.Level, Is.EqualTo(2));
            Assert.That(result.TotalXp, Is.EqualTo(xp));
            Assert.That(result.XpForNextLevel, Is.GreaterThan(xp));
            Assert.That(result.Progress, Is.GreaterThan(0.0).And.LessThan(1.0));  // Progress into level 3
        }

        [Test]
        public void TC007_CalculateLevelProgress_ShouldReturnCorrectXpForNextLevel_WhenUserIsOnFirstLevel()
        {
            // Arrange
            int xp = 1;  // Small XP value, user should still be on level 0 or starting

            // Act
            var result = _levelService.CalculateLevelProgress(xp);

            // Assert
            Assert.That(result.Level, Is.EqualTo(0));  // Should still be at level 0
            Assert.That(result.XpForNextLevel, Is.GreaterThan(0));  // Next level XP should be above 1
        }
    }
}