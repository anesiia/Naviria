using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using NaviriaAPI.Services.CleanupServices;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Exceptions;
using NaviriaAPI.Entities;

namespace NaviriaAPI.Tests.Services.CleanupServices
{
    public class AchievementCleanupServiceTests
    {
        private Mock<IAchievementRepository> _achievementRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        private AchievementCleanupService _service;

        [SetUp]
        public void SetUp()
        {
            _achievementRepoMock = new Mock<IAchievementRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _service = new AchievementCleanupService(_achievementRepoMock.Object, _userRepoMock.Object);
        }

        [Test]
        public void TC001_DeleteAchievementAndRemoveFromUsersAsync_ThrowsNotFoundException_WhenAchievementDoesNotExist()
        {
            // Arrange
            var achievementId = "123";
            _achievementRepoMock.Setup(r => r.GetByIdAsync(achievementId))
                .ReturnsAsync((AchievementEntity?)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() =>
                _service.DeleteAchievementAndRemoveFromUsersAsync(achievementId));

            Assert.That(ex!.Message, Is.EqualTo("Achievement with ID 123 not found."));
        }

        [Test]
        public async Task TC002_DeleteAchievementAndRemoveFromUsersAsync_RemovesAchievementFromUsers_AndDeletesAchievement()
        {
            // Arrange
            var achievementId = "456";
            var achievement = new AchievementEntity
            {
                Id = achievementId,
                Name = "Top Performer",
                Description = "Earned for excellent performance.",
                Points = 50,
                IsRare = true
            };

            _achievementRepoMock.Setup(r => r.GetByIdAsync(achievementId))
                .ReturnsAsync(achievement);

            _userRepoMock.Setup(r => r.RemoveAchievementFromAllUsersAsync(achievementId))
                .Returns(Task.CompletedTask);

            _achievementRepoMock.Setup(r => r.DeleteAsync(achievementId))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAchievementAndRemoveFromUsersAsync(achievementId);

            // Assert
            Assert.That(result, Is.True);

            _userRepoMock.Verify(r => r.RemoveAchievementFromAllUsersAsync(achievementId), Times.Once);
            _achievementRepoMock.Verify(r => r.DeleteAsync(achievementId), Times.Once);
        }

        [Test]
        public async Task TC003_DeleteAchievementAndRemoveFromUsersAsync_ReturnsFalse_WhenDeleteFails()
        {
            // Arrange
            var achievementId = "789";
            var achievement = new AchievementEntity
            {
                Id = achievementId,
                Name = "Failed One",
                Description = "Should fail to delete",
                Points = 10,
                IsRare = false
            };

            _achievementRepoMock.Setup(r => r.GetByIdAsync(achievementId))
                .ReturnsAsync(achievement);

            _userRepoMock.Setup(r => r.RemoveAchievementFromAllUsersAsync(achievementId))
                .Returns(Task.CompletedTask);

            _achievementRepoMock.Setup(r => r.DeleteAsync(achievementId))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DeleteAchievementAndRemoveFromUsersAsync(achievementId);

            // Assert
            Assert.That(result, Is.False);

            _userRepoMock.Verify(r => r.RemoveAchievementFromAllUsersAsync(achievementId), Times.Once);
            _achievementRepoMock.Verify(r => r.DeleteAsync(achievementId), Times.Once);
        }
    }
}