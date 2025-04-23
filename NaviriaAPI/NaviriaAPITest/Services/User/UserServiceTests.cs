using Moq;
using NaviriaAPI.Entities;
using NaviriaAPI.Entities.EmbeddedEntities;
using NUnit.Framework;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.IServices;
using NaviriaAPI.Exceptions;
using NaviriaAPI.Mappings;
using Microsoft.Extensions.Configuration;
using NaviriaAPI.Services;
using NaviriaAPI.IServices.ICloudStorage;
using NaviriaAPI.IServices.IGamificationLogic;
using NaviriaAPI.IServices.IJwtService;
using NaviriaAPI.Services.Validation;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NaviriaAPI.IRepositories;
using OpenAI.Chat;
using NaviriaAPI.Services.User;

namespace NaviriaAPI.Tests.Services.User
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepoMock;
        private Mock<IPasswordHasher<UserEntity>> _hasherMock;
        private Mock<IConfiguration> _configMock;
        private Mock<UserValidationService> _validationMock;
        private Mock<ICloudinaryService> _cloudinaryServiceMock;
        private Mock<IAchievementRepository> _achievementRepoMock;
        private Mock<ILevelService> _levelServiceMock;
        private Mock<IJwtService> _jwtServiceMock;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _hasherMock = new Mock<IPasswordHasher<UserEntity>>();
            _configMock = new Mock<IConfiguration>();
            _cloudinaryServiceMock = new Mock<ICloudinaryService>();
            _achievementRepoMock = new Mock<IAchievementRepository>();
            _levelServiceMock = new Mock<ILevelService>();
            _jwtServiceMock = new Mock<IJwtService>();

            _configMock.Setup(c => c["OpenAIKey"]).Returns("fake-key");

            var validation = new UserValidationService(_userRepoMock.Object);
            _userService = new UserService(
                _userRepoMock.Object,
                _hasherMock.Object,
                _configMock.Object,
                validation,
                _cloudinaryServiceMock.Object,
                _achievementRepoMock.Object,
                _levelServiceMock.Object,
                _jwtServiceMock.Object
            );
        }

        [Test]
        public async Task TC001_CreateAsync_ShouldReturnToken_WhenUserCreatedSuccessfully()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "John Doe",
                Email = "john.doe@example.com",
                Password = "password123",
                Photo = null, // Фото не додається
                LastSeen = DateTime.UtcNow
            };

            _userRepoMock.Setup(repo => repo.CreateAsync(It.IsAny<UserEntity>())).Returns(Task.CompletedTask);
            _hasherMock.Setup(h => h.HashPassword(It.IsAny<UserEntity>(), It.IsAny<string>())).Returns("hashedPassword");

            _jwtServiceMock.Setup(service => service.GenerateUserToken(It.IsAny<UserEntity>())).Returns("fake-jwt-token");

            // Act
            var result = await _userService.CreateAsync(userDto);

            // Assert
            Assert.That(result, Is.EqualTo("fake-jwt-token"));
            _userRepoMock.Verify(repo => repo.CreateAsync(It.IsAny<UserEntity>()), Times.Once);
        }

        [Test]
        public async Task TC002_UpdateAsync_ShouldUpdateUser_WhenUserExists()
        {
            // Arrange
            var userId = "12345";
            var userDto = new UserUpdateDto
            {
                FullName = "John Tre",
                Points = 100,
                LastSeen = DateTime.UtcNow
            };

            var existingUser = new UserEntity
            {
                Id = userId,
                FullName = "John Doe",
                Points = 50,
                LevelInfo = new LevelProgressInfo { Level = 1 },
                Achievements = new List<UserAchievementInfo>()
            };

            _userRepoMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);
            _userRepoMock.Setup(repo => repo.UpdateAsync(It.IsAny<UserEntity>())).ReturnsAsync(true);
            _levelServiceMock.Setup(service => service.CalculateLevelProgress(It.IsAny<int>())).Returns(new LevelProgressInfo { Level = 2 });
            // Act
            var result = await _userService.UpdateAsync(userId, userDto);

            // Assert
            Assert.That(result, Is.True);
            _userRepoMock.Verify(repo => repo.UpdateAsync(It.IsAny<UserEntity>()), Times.Once);
        }

        [Test]
        public async Task TC003_GetByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = "12345";
            var user = new UserEntity
            {
                Id = userId,
                FullName = "John Doe",
                Points = 50,
                LevelInfo = new LevelProgressInfo { Level = 1 },
                Achievements = new List<UserAchievementInfo>()
            };

            _userRepoMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetByIdAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Id, Is.EqualTo(userId));

        }

        [Test]
        public async Task TC004_GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = "12345";

            _userRepoMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((UserEntity?)null);

            // Act
            var result = await _userService.GetByIdAsync(userId);

            // Assert
            Assert.That(result, Is.Null);

        }

        [Test]
        public async Task TC005_DeleteAsync_ShouldReturnTrue_WhenUserDeletedSuccessfully()
        {
            // Arrange
            var userId = "12345";
            _userRepoMock.Setup(repo => repo.DeleteAsync(userId)).ReturnsAsync(true);

            // Act
            var result = await _userService.DeleteAsync(userId);

            // Assert
            Assert.That(result, Is.True);

            _userRepoMock.Verify(repo => repo.DeleteAsync(userId), Times.Once);
        }


        [Test]
        public async Task TC006_GiveAchievementAsync_ShouldReturnFalse_WhenAchievementAlreadyReceived()
        {
            // Arrange
            var userId = "12345";
            var achievementId = "achievement1";

            var existingUser = new UserEntity
            {
                Id = userId,
                Achievements = new List<UserAchievementInfo>
                {
                    new UserAchievementInfo { AchievementId = achievementId }
                }
            };

            _userRepoMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);

            // Act
            var result = await _userService.GiveAchievementAsync(userId, achievementId);

            // Assert
            Assert.That(result, Is.False);
            _userRepoMock.Verify(repo => repo.UpdateAsync(It.IsAny<UserEntity>()), Times.Never);
        }

        [Test]
        public async Task TC007_GiveAchievementAsync_ShouldReturnTrue_WhenAchievementNotReceivedAndUserExists()
        {
            // Arrange
            var userId = "12345";
            var achievementId = "achievement1";
            var achievementPoints = 50;

            var existingUser = new UserEntity
            {
                Id = userId,
                Achievements = new List<UserAchievementInfo>()
            };

            var achievement = new AchievementEntity
            {
                Id = achievementId,
                Points = achievementPoints
            };

            _userRepoMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);
            _achievementRepoMock.Setup(repo => repo.GetByIdAsync(achievementId)).ReturnsAsync(achievement);
            _userRepoMock.Setup(repo => repo.UpdateAsync(It.IsAny<UserEntity>())).ReturnsAsync(true);
            _levelServiceMock.Setup(service => service.CalculateLevelProgress(It.IsAny<int>())).Returns(new LevelProgressInfo { Level = 2 });

            // Act
            var result = await _userService.GiveAchievementAsync(userId, achievementId);

            // Assert
            Assert.That(result, Is.True);
            _userRepoMock.Verify(repo => repo.UpdateAsync(It.IsAny<UserEntity>()), Times.Once);
        }

        [Test]
        public async Task TC008_GiveAchievementAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = "12345";
            var achievementId = "achievement1";

            _userRepoMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((UserEntity)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(async () =>
                await _userService.GiveAchievementAsync(userId, achievementId));

            Assert.That(ex.Message, Is.EqualTo($"User with ID {userId} not found"));
        }

        [Test]
        public async Task TC009_GiveAchievementAsync_ShouldApplyPointsAndRecalculateLevel_WhenAchievementIsGiven()
        {
            // Arrange
            var userId = "12345";
            var achievementId = "achievement1";
            var achievementPoints = 50;

            var existingUser = new UserEntity
            {
                Id = userId,
                Points = 100,
                Achievements = new List<UserAchievementInfo>()
            };

            var achievement = new AchievementEntity
            {
                Id = achievementId,
                Points = achievementPoints
            };

            _userRepoMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);
            _achievementRepoMock.Setup(repo => repo.GetByIdAsync(achievementId)).ReturnsAsync(achievement);
            _userRepoMock.Setup(repo => repo.UpdateAsync(It.IsAny<UserEntity>())).ReturnsAsync(true);
            _levelServiceMock.Setup(service => service.CalculateLevelProgress(It.IsAny<int>())).Returns(new LevelProgressInfo { Level = 2 });

            // Act
            var result = await _userService.GiveAchievementAsync(userId, achievementId);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(existingUser.Points, Is.EqualTo(150));  // 100 + 50 points
            _levelServiceMock.Verify(service => service.CalculateLevelProgress(150), Times.Once);
            _userRepoMock.Verify(repo => repo.UpdateAsync(It.IsAny<UserEntity>()), Times.Once);
        }


        [Test]
        public async Task TC010_GetAllAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<UserEntity>
                {
                    new UserEntity { Id = "1", FullName = "User One", LastSeen = DateTime.UtcNow },
                    new UserEntity { Id = "2", FullName = "User Two", LastSeen = DateTime.UtcNow }
                };

            _userRepoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task TC011_GetUserOrThrowAsync_ShouldReturnUser_WhenExists()
        {
            // Arrange
            var userId = "user123";
            var user = new UserEntity { Id = userId };
            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserOrThrowAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(userId));
        }

        [Test]
        public void TC012_GetUserOrThrowAsync_ShouldThrowNotFoundException_WhenUserNotExists()
        {
            // Arrange
            var userId = "nonexistent";
            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((UserEntity?)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NaviriaAPI.Exceptions.NotFoundException>(() => _userService.GetUserOrThrowAsync(userId));
            Assert.That(ex!.Message, Does.Contain("User with ID nonexistent not found"));
        }

    }
    }
