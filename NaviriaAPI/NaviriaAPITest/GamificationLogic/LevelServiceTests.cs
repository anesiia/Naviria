using NaviriaAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviriaAPITest.GamificationLogic
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

        //[Test]
        //public void TC004_CalculateLevelProgress_ShouldReturnCorrectProgress_WhenXpIsMaxForLevel()
        //{
        //    // Arrange
        //    int xp = 240;  // This XP is between the XP required for level 2 (210) and level 3 (570).

        //    // Act
        //    var result = _levelService.CalculateLevelProgress(xp);

        //    // Assert
        //    Assert.That(result.Level, Is.EqualTo(2));  // Expected level is 2, since 240 is between 210 and 570
        //    Assert.That(result.TotalXp, Is.EqualTo(xp));
        //    Assert.That(result.XpForNextLevel, Is.EqualTo(570));  // The XP required for level 3
        //    Assert.That(result.Progress, Is.EqualTo(Math.Round(0.3)));  // Round to 2 decimal places
        //}



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