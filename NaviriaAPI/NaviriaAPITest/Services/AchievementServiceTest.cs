using Moq;
using NUnit.Framework;
using NaviriaAPI.DTOs;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.IServices.IGamificationLogic;
using NaviriaAPI.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using NaviriaAPI.Entities;
using System.Linq;
using NaviriaAPI.Exceptions;
using NaviriaAPI.Entities.EmbeddedEntities;

namespace NaviriaAPI.Tests.Services
{
    [TestFixture]
    public class AchievementServiceTest
    {
        private Mock<IAchievementRepository> _mockRepository;
        private Mock<IUserService> _mockUserService;
        private Mock<IUserRepository> _mockUserRepo;
        private Mock<ILevelService> _mockLevelService;
        private Mock<ILogger<AchievementService>> _mockLogger;
        private AchievementService _achievementService;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IAchievementRepository>();
            _mockUserService = new Mock<IUserService>();
            _mockUserRepo = new Mock<IUserRepository>();
            _mockLevelService = new Mock<ILevelService>();
            _mockLogger = new Mock<ILogger<AchievementService>>();

            _achievementService = new AchievementService(
                _mockRepository.Object,
                _mockUserService.Object,
                _mockUserRepo.Object,
                _mockLogger.Object,
                _mockLevelService.Object);
        }

        // TC-01
        [Test]
        public async Task TC01_CreateAchievement_ShouldReturnAchievementDto_WhenValidDataIsPassed()
        {
            var createDto = new AchievementCreateDto
            {
                Name = "New Achievement",
                Description = "Description of new achievement"
            };

            _mockRepository
                .Setup(repo => repo.CreateAsync(It.IsAny<AchievementEntity>()))
                .Callback<AchievementEntity>(e => e.Id = "1")
                .Returns(Task.CompletedTask);

            var result = await _achievementService.CreateAsync(createDto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo("1"));
            Assert.That(result.Name, Is.EqualTo(createDto.Name));
            Assert.That(result.Description, Is.EqualTo(createDto.Description));
            _mockRepository.Verify(repo => repo.CreateAsync(It.IsAny<AchievementEntity>()), Times.Once);
        }

        // TC-02
        [Test]
        public async Task TC02_UpdateAchievement_ShouldReturnTrue_WhenValidDataIsPassed()
        {
            var updateDto = new AchievementUpdateDto
            {
                Name = "Updated Achievement",
                Description = "Updated description"
            };

            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<AchievementEntity>()))
                           .ReturnsAsync(true);

            var result = await _achievementService.UpdateAsync("1", updateDto);

            Assert.That(result, Is.True);
            _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<AchievementEntity>()), Times.Once);
        }

        // TC-03
        [Test]
        public async Task TC03_GetAchievement_ShouldReturnAchievementDto_WhenValidIdIsPassed()
        {
            var achievement = new AchievementEntity
            {
                Id = "1",
                Name = "First Achievement",
                Description = "This is the first achievement"
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync("1"))
                           .ReturnsAsync(achievement);

            var result = await _achievementService.GetByIdAsync("1");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo("1"));
            Assert.That(result.Name, Is.EqualTo("First Achievement"));
            Assert.That(result.Description, Is.EqualTo("This is the first achievement"));
            _mockRepository.Verify(repo => repo.GetByIdAsync("1"), Times.Once);
        }

        // TC-04
        [Test]
        public async Task TC04_GetAchievement_ShouldReturnNull_WhenInvalidIdIsPassed()
        {
            _mockRepository.Setup(repo => repo.GetByIdAsync("99999"))
                           .ReturnsAsync((AchievementEntity)null);

            var result = await _achievementService.GetByIdAsync("99999");

            Assert.That(result, Is.Null);
            _mockRepository.Verify(repo => repo.GetByIdAsync("99999"), Times.Once);
        }

        // TC-05
        [Test]
        public async Task TC05_DeleteAchievement_ShouldReturnTrue_WhenValidIdIsPassed()
        {
            _mockRepository.Setup(repo => repo.DeleteAsync("1"))
                           .ReturnsAsync(true);

            var result = await _achievementService.DeleteAsync("1");

            Assert.That(result, Is.True);
            _mockRepository.Verify(repo => repo.DeleteAsync("1"), Times.Once);
        }

        // TC-06
        [Test]
        public async Task TC06_DeleteAchievement_ShouldReturnFalse_WhenInvalidIdIsPassed()
        {
            _mockRepository.Setup(repo => repo.DeleteAsync("99999"))
                           .ReturnsAsync(false);

            var result = await _achievementService.DeleteAsync("99999");

            Assert.That(result, Is.False);
            _mockRepository.Verify(repo => repo.DeleteAsync("99999"), Times.Once);
        }

        // TC-07
        [Test]
        public async Task TC07_GetAllAchievements_ShouldReturnListOfAchievements()
        {
            var achievements = new List<AchievementEntity>
            {
                new AchievementEntity { Id = "1", Name = "First", Description = "Desc 1" },
                new AchievementEntity { Id = "2", Name = "Second", Description = "Desc 2" }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync())
                           .ReturnsAsync(achievements);

            var result = await _achievementService.GetAllAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.ElementAt(0).Id, Is.EqualTo("1"));
            Assert.That(result.ElementAt(1).Id, Is.EqualTo("2"));
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Test]
        public void TC08_GetAllUserAchievements_ShouldThrowException_WhenUserIdIsNull()
        {
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _achievementService.GetAllUserAchievementsAsync(null));
            Assert.That(ex.ParamName, Is.EqualTo("userId"));
        }

        [Test]
        public void TC09_GetAllUserAchievements_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            _mockUserService.Setup(us => us.UserExistsAsync("nonexistent"))
                            .ReturnsAsync(false);

            Assert.ThrowsAsync<NotFoundException>(() =>
                _achievementService.GetAllUserAchievementsAsync("nonexistent"));
        }

        [Test]
        public void TC10_AwardAchievementPoints_ShouldThrow_WhenPointsAlreadyReceived()
        {
            var userDto = new UserDto
            {
                Id = "user1",
                Achievements = new List<UserAchievementInfo>
        {
            new UserAchievementInfo
            {
                AchievementId = "ach1",
                IsPointsReceived = true,
                ReceivedAt = DateTime.UtcNow

            }
        }
            };

            var achievement = new AchievementEntity
            {
                Id = "ach1", // Переконайся, що тип — string
                Points = 10
            };

            _mockUserService.Setup(us => us.GetByIdAsync("user1")).ReturnsAsync(userDto);
            _mockRepository.Setup(ar => ar.GetByIdAsync("ach1")).ReturnsAsync(achievement);

            var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
                _achievementService.AwardAchievementPointsAsync("user1", "ach1"));

            Assert.That(ex.Message, Is.EqualTo("Points for this achievement already received"));
        }

        [Test]
        public async Task TC11_AwardAchievementPoints_ShouldReturnTrue_WhenValid()
        {
            var userDto = new UserDto
            {
                Id = "user1",
                Points = 0,
                Achievements = new List<UserAchievementInfo>
        {
            new UserAchievementInfo { AchievementId = "ach1", IsPointsReceived = false }
        }
            };

            var achievement = new AchievementEntity { Id = "ach1", Points = 10 };

            _mockUserService.Setup(us => us.GetByIdAsync("user1")).ReturnsAsync(userDto);
            _mockRepository.Setup(ar => ar.GetByIdAsync("ach1")).ReturnsAsync(achievement);
            
            _mockUserRepo.Setup(repo => repo.UpdateAsync(It.IsAny<UserEntity>())).ReturnsAsync(true);

            var result = await _achievementService.AwardAchievementPointsAsync("user1", "ach1");

            Assert.That(result, Is.True);
            _mockUserRepo.Verify(repo => repo.UpdateAsync(It.IsAny<UserEntity>()), Times.Once);
        }

    }
}
