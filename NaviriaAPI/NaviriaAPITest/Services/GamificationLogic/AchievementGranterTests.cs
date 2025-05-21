

using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using NaviriaAPI.Services.GamificationLogic;
using NaviriaAPI.Entities;
using NaviriaAPI.Entities.EmbeddedEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IRepositories;

namespace NaviriaAPI.Tests.Services.GamificationLogic
{
    [TestFixture]
    public class AchievementGranterTests
    {
        private Mock<IUserRepository> _userRepoMock;
        private Mock<IAchievementRepository> _achievementRepoMock;
        private Mock<ILogger<AchievementGranter>> _loggerMock;
        private AchievementGranter _sut;

        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _achievementRepoMock = new Mock<IAchievementRepository>();
            _loggerMock = new Mock<ILogger<AchievementGranter>>();

            _sut = new AchievementGranter(_userRepoMock.Object, _achievementRepoMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task TC001_GiveAsync_UserNotFound_ThrowsNotFoundException()
        {
            _userRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((UserEntity?)null);

            var ex = Assert.ThrowsAsync<NotFoundException>(async () =>
                await _sut.GiveAsync("userId", "achievementId"));

            Assert.That(ex.Message, Is.EqualTo("User not found"));
        }

        [Test]
        public async Task TC002_GiveAsync_AchievementAlreadyExists_DoesNotAddAgain()
        {
            var user = new UserEntity
            {
                Achievements = new System.Collections.Generic.List<UserAchievementInfo>
            {
                new UserAchievementInfo { AchievementId = "achievementId" }
            }
            };

            _userRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(user);

            await _sut.GiveAsync("userId", "achievementId");

            // Переконаємось, що UpdateAsync не викликався
            _userRepoMock.Verify(x => x.UpdateAsync(It.IsAny<UserEntity>()), Times.Never);
        }

        [Test]
        public async Task TC003_GiveAsync_AchievementNotFound_ThrowsNotFoundException()
        {
            var user = new UserEntity();

            _userRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            _achievementRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((AchievementEntity?)null);

            var ex = Assert.ThrowsAsync<NotFoundException>(async () =>
                await _sut.GiveAsync("userId", "achievementId"));

            Assert.That(ex.Message, Is.EqualTo("Achievement not found"));
        }

    }
}
