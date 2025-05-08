using Moq;
using NUnit.Framework;
using NaviriaAPI.Services.AchievementStrategies;
using NaviriaAPI.IServices.IGamificationLogic;
using NaviriaAPI.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace NaviriaAPI.Tests.Services.AchievementStrategies
{
    [TestFixture]
    public class RegistrationAchievementStrategyTests
    {
        private RegistrationAchievementStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            // Ініціалізуємо стратегію перед кожним тестом
            _strategy = new RegistrationAchievementStrategy();
        }

        [Test]
        public void TC001_Trigger_ReturnsCorrectAchievementTrigger()
        {
            // Arrange
            // Вже ініціалізовано в SetUp методі

            // Act
            var trigger = _strategy.Trigger;

            // Assert
            Assert.That(trigger, Is.EqualTo(AchievementTrigger.OnRegistration));
        }

        [Test]
        public async Task TC002_GetAchievementIdsAsync_ReturnsCorrectAchievementId()
        {
            // Arrange
            var userId = ObjectId.GenerateNewId().ToString(); // Генеруємо новий унікальний ID користувача

            // Act
            var achievementIds = await _strategy.GetAchievementIdsAsync(userId);

            // Assert
            Assert.That(achievementIds, Contains.Item(AchievementIds.Registration));
            Assert.That(achievementIds.Count(), Is.EqualTo(1)); // Має бути лише один елемент в колекції
        }
    }
}