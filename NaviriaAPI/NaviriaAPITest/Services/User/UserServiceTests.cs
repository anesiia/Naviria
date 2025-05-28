//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Logging;
//using Moq;
//using NaviriaAPI.Entities;
//using NaviriaAPI.Entities.EmbeddedEntities;
//using NaviriaAPI.Exceptions;
//using NaviriaAPI.IServices.ICloudStorage;
//using NaviriaAPI.IServices.IGamificationLogic;
//using NaviriaAPI.IServices.IJwtService;
//using NaviriaAPI.IServices.IUserServices;
//using NaviriaAPI.IServices.ICleanupServices;
//using NaviriaAPI.Mappings;
//using NaviriaAPI.Repositories;
//using NaviriaAPI.Services.User;
//using NaviriaAPI.DTOs.User;
//using NUnit.Framework;
//using NaviriaAPI.Helpers;
//using NaviriaAPI.IRepositories;
//using NaviriaAPI.Services.Validation;

//namespace NaviriaAPI.Tests.Services.User
//{
//    [TestFixture]
//    public class UserServiceTests
//    {
//        private Mock<IUserRepository> _userRepositoryMock = null!;
//        private Mock<IPasswordHasher<UserEntity>> _passwordHasherMock = null!;
//        private Mock<UserValidationService> _validationMock = null!;
//        private Mock<IAchievementRepository> _achievementRepositoryMock = null!;
//        private Mock<ILevelService> _levelServiceMock = null!;
//        private Mock<IJwtService> _jwtServiceMock = null!;
//        private Mock<ILogger<UserService>> _loggerMock = null!;
//        private Mock<IAchievementManager> _achievementManagerMock = null!;
//        private Mock<IUserCleanupService> _userCleanupServiceMock = null!;
//        private Mock<ICloudinaryService> _cloudinaryServiceMock = null!;
//        private Mock<ILogger<UserValidationService>> _validationLoggerMock = null!;


//        private UserService _sut = null!;
//        [SetUp]
//        public void Setup()
//        {
//            _userRepositoryMock = new Mock<IUserRepository>();
//            _passwordHasherMock = new Mock<IPasswordHasher<UserEntity>>();
//            _achievementRepositoryMock = new Mock<IAchievementRepository>();
//            _levelServiceMock = new Mock<ILevelService>();
//            _jwtServiceMock = new Mock<IJwtService>();
//            _loggerMock = new Mock<ILogger<UserService>>();
//            _achievementManagerMock = new Mock<IAchievementManager>();
//            _userCleanupServiceMock = new Mock<IUserCleanupService>();
//            _cloudinaryServiceMock = new Mock<ICloudinaryService>();
//            _validationLoggerMock = new Mock<ILogger<UserValidationService>>();

//            // Cтворення справжнього UserValidationService
//            var realValidationService = new UserValidationService(
//                _userRepositoryMock.Object,
//                _validationLoggerMock.Object
//            );

//            _sut = new UserService(
//                _userRepositoryMock.Object,
//                _passwordHasherMock.Object,
//                realValidationService, // передаємо реальний інстанс
//                _achievementRepositoryMock.Object,
//                _levelServiceMock.Object,
//                _jwtServiceMock.Object,
//                _loggerMock.Object,
//                _achievementManagerMock.Object,
//                _userCleanupServiceMock.Object,
//                _cloudinaryServiceMock.Object);
//        }

//        [Test]
//        public async Task CreateAsync_UserEmailExists_ThrowsEmailAlreadyExistException()
//        {
//            // Arrange
//            var userDto = new UserCreateDto
//            {
//                Email = "test@example.com",
//                Nickname = "nick",
//                Password = "Password1",
//                FullName = "Full Name",
//                Gender = "m",
//                BirthDate = DateTime.UtcNow.AddYears(-20)
//            };

//            _userRepositoryMock.Setup(x => x.GetByEmailAsync(userDto.Email))
//                .ReturnsAsync(new UserEntity { Email = userDto.Email });

//            // Act & Assert
//            var ex = Assert.ThrowsAsync<EmailAlreadyExistException>(() => _sut.CreateAsync(userDto));
//            Assert.That(ex?.Message, Is.EqualTo("User with such email already exists"));
//        }

//        [Test]
//        public async Task CreateAsync_UserNicknameExists_ThrowsNicknameAlreadyExistException()
//        {
//            // Arrange
//            var userDto = new UserCreateDto
//            {
//                Email = "test@example.com",
//                Nickname = "nick",
//                Password = "Password1",
//                FullName = "Full Name",
//                Gender = "m",
//                BirthDate = DateTime.UtcNow.AddYears(-20)
//            };

//            _userRepositoryMock.Setup(x => x.GetByEmailAsync(userDto.Email))
//                .ReturnsAsync((UserEntity?)null);

//            _userRepositoryMock.Setup(x => x.GetByNicknameAsync(userDto.Nickname))
//                .ReturnsAsync(new UserEntity { Nickname = userDto.Nickname });

//            // Act & Assert
//            var ex = Assert.ThrowsAsync<NicknameAlreadyExistException>(() => _sut.CreateAsync(userDto));
//            Assert.That(ex?.Message, Is.EqualTo("User with such nickname already exists"));
//        }

//        [Test]
//        public async Task CreateAsync_ValidUser_ReturnsJwtToken()
//        {
//            // Arrange
//            var userDto = new UserCreateDto
//            {
//                Email = "newuser@example.com",
//                Nickname = "newnick",
//                Password = "Password1",
//                FullName = "New User",
//                Gender = "f",
//                BirthDate = DateTime.UtcNow.AddYears(-25)
//            };

//            _userRepositoryMock.Setup(x => x.GetByEmailAsync(userDto.Email))
//                .ReturnsAsync((UserEntity?)null);

//            _userRepositoryMock.Setup(x => x.GetByNicknameAsync(userDto.Nickname))
//                .ReturnsAsync((UserEntity?)null);

//            _validationMock.Setup(x => x.ValidateCreateAsync(userDto))
//                .Returns(Task.CompletedTask);

//            _passwordHasherMock.Setup(ph => ph.HashPassword(It.IsAny<UserEntity>(), userDto.Password))
//                .Returns("hashed_password");

//            _levelServiceMock.Setup(ls => ls.CalculateFirstLevelProgress(50))
//                .Returns(new LevelProgressInfo { Level = 1, TotalXp = 50, XpForNextLevel = 100 });

//            _userRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<UserEntity>()))
//                .Returns(Task.CompletedTask);

//            _achievementManagerMock
//    .Setup(am => am.EvaluateAsync(It.IsAny<string>(), AchievementTrigger.OnRegistration, null))
//    .Returns(Task.CompletedTask);


//            _jwtServiceMock.Setup(j => j.GenerateUserToken(It.IsAny<UserEntity>()))
//                .Returns("fake_jwt_token");

//            // Act
//            var token = await _sut.CreateAsync(userDto);

//            // Assert
//            Assert.That(token, Is.EqualTo("fake_jwt_token"));

//            _validationMock.Verify(x => x.ValidateCreateAsync(userDto), Times.Once);
//            _userRepositoryMock.Verify(x => x.CreateAsync(It.Is<UserEntity>(u =>
//                u.Email == userDto.Email &&
//                u.Nickname == userDto.Nickname &&
//                u.Password == "hashed_password")), Times.Once);

//            _achievementManagerMock.Verify(
//    am => am.EvaluateAsync(It.IsAny<string>(), AchievementTrigger.OnRegistration, null),
//    Times.Once);
//        }

//        [Test]
//        public async Task GetByIdAsync_UserExists_ReturnsUserDto()
//        {
//            // Arrange
//            var id = "123";
//            var userEntity = new UserEntity
//            {
//                Id = id,
//                Email = "user@example.com",
//                LastSeen = DateTime.UtcNow
//            };

//            _userRepositoryMock.Setup(x => x.GetByIdAsync(id))
//                .ReturnsAsync(userEntity);

//            // Act
//            var result = await _sut.GetByIdAsync(id);

//            // Assert
//            Assert.That(result, Is.Not.Null);
//            Assert.That(result?.Email, Is.EqualTo(userEntity.Email));
//        }

//        [Test]
//        public async Task GetByIdAsync_UserNotFound_ReturnsNull()
//        {
//            // Arrange
//            _userRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
//                .ReturnsAsync((UserEntity?)null);

//            // Act
//            var result = await _sut.GetByIdAsync("nonexistent_id");

//            // Assert
//            Assert.That(result, Is.Null);
//        }

//        [Test]
//        public async Task UserExistsAsync_ValidId_UserExists_ReturnsTrue()
//        {
//            // Arrange
//            var id = "user123";
//            _userRepositoryMock.Setup(x => x.GetByIdAsync(id))
//                .ReturnsAsync(new UserEntity { Id = id });

//            // Act
//            var exists = await _sut.UserExistsAsync(id);

//            // Assert
//            Assert.That(exists, Is.True);
//        }

//        [Test]
//        public async Task UserExistsAsync_InvalidOrEmptyId_ReturnsFalse()
//        {
//            var existsNull = await _sut.UserExistsAsync(null!);
//            var existsEmpty = await _sut.UserExistsAsync(string.Empty);
//            var existsWhitespace = await _sut.UserExistsAsync("  ");

//            Assert.That(existsNull, Is.False);
//            Assert.That(existsEmpty, Is.False);
//            Assert.That(existsWhitespace, Is.False);
//        }

//        [Test]
//        public async Task GiveAchievementAsync_UserAlreadyHasAchievement_ThrowsAlreadyExistException()
//        {
//            // Arrange
//            var userId = "user1";
//            var achievementId = "ach1";
//            var user = new UserEntity
//            {
//                Id = userId,
//                Achievements = new List<UserAchievementInfo>
//                {
//                    new() { AchievementId = achievementId }
//                }
//            };

//            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
//                .ReturnsAsync(user);

//            // Act & Assert
//            var ex = Assert.ThrowsAsync<AlreadyExistException>(() => _sut.GiveAchievementAsync(userId, achievementId));
//            Assert.That(ex?.Message, Is.EqualTo($"User already has achievement {achievementId}"));
//        }

//        [Test]
//        public async Task GiveAchievementAsync_AchievementNotFound_ThrowsNotFoundException()
//        {
//            // Arrange
//            var userId = "user1";
//            var achievementId = "ach_missing";

//            var user = new UserEntity { Id = userId, Achievements = new List<UserAchievementInfo>() };

//            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
//                .ReturnsAsync(user);

//            _achievementRepositoryMock.Setup(x => x.GetByIdAsync(achievementId))
//                .ReturnsAsync((AchievementEntity?)null);

//            // Act & Assert
//            var ex = Assert.ThrowsAsync<NotFoundException>(() => _sut.GiveAchievementAsync(userId, achievementId));
//            Assert.That(ex?.Message, Is.EqualTo($"Achievement {achievementId} not found"));
//        }

//        [Test]
//        public async Task GiveAchievementAsync_ValidInputs_AddsAchievementAndSaves()
//        {
//            // Arrange
//            var userId = "user1";
//            var achievementId = "ach1";

//            var user = new UserEntity { Id = userId, Achievements = new List<UserAchievementInfo>() };

//            var achievement = new AchievementEntity
//            {
//                Id = achievementId,
//                Description = "Desc",
//                Points = 1,
//                IsRare = false,
//            };

//            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
//                .ReturnsAsync(user);

//            _achievementRepositoryMock.Setup(x => x.GetByIdAsync(achievementId))
//                .ReturnsAsync(achievement);

//            _userRepositoryMock.Setup(x => x.UpdateAsync(user))
//                .ReturnsAsync(true);

//            // Act
//            await _sut.GiveAchievementAsync(userId, achievementId);

//            // Assert
//            Assert.That(user.Achievements, Has.Exactly(1).Items);
//            Assert.That(user.Achievements[0].AchievementId, Is.EqualTo(achievementId));

//            _userRepositoryMock.Verify(x => x.UpdateAsync(user), Times.Once);
//        }

//        [Test]
//        public async Task DeleteAsync_UserExists_CallsDelete()
//        {
//            // Arrange
//            var userId = "userToDelete";

//            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
//                .ReturnsAsync(new UserEntity { Id = userId });

//            _userRepositoryMock.Setup(x => x.DeleteAsync(userId))
//                .ReturnsAsync(true)
//                .Verifiable();

//            // Act
//            await _sut.DeleteAsync(userId);

//            // Assert
//            _userRepositoryMock.Verify(x => x.DeleteAsync(userId), Times.Once);
//        }

//        [Test]
//        public void DeleteAsync_UserDoesNotExist_ThrowsNotFoundException()
//        {
//            // Arrange
//            var userId = "nonexistentUser";

//            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
//                .ReturnsAsync((UserEntity?)null);

//            // Act & Assert
//            var ex = Assert.ThrowsAsync<NotFoundException>(() => _sut.DeleteAsync(userId));
//            Assert.That(ex?.Message, Is.EqualTo($"User {userId} not found"));
//        }
//    }
//}



