using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using NaviriaAPI.Entities;
using NaviriaAPI.Entities.EmbeddedEntities;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IServices.ICloudStorage;
using NaviriaAPI.IServices.IGamificationLogic;
using NaviriaAPI.IServices.IJwtService;
using NaviriaAPI.IServices.IUserServices;
using NaviriaAPI.IServices.ICleanupServices;
using NaviriaAPI.Mappings;
using NaviriaAPI.Repositories;
using NaviriaAPI.Services.User;
using NaviriaAPI.DTOs.User;
using NUnit.Framework;
using NaviriaAPI.Helpers;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Services.Validation;
using System.ComponentModel.DataAnnotations;

namespace NaviriaAPI.Tests.Services.User
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IPasswordHasher<UserEntity>> _passwordHasherMock;
        private Mock<IJwtService> _jwtServiceMock;
        private Mock<IAchievementRepository> _achievementRepositoryMock;
        private Mock<IAchievementManager> _achievementManagerMock;
        private Mock<ILevelService> _levelServiceMock;
        private Mock<IUserCleanupService> _userCleanupServiceMock;
        private Mock<ICloudinaryService> _cloudinaryServiceMock;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher<UserEntity>>();
            _jwtServiceMock = new Mock<IJwtService>();
            _achievementRepositoryMock = new Mock<IAchievementRepository>();
            _achievementManagerMock = new Mock<IAchievementManager>();
            _levelServiceMock = new Mock<ILevelService>();
            _userCleanupServiceMock = new Mock<IUserCleanupService>();
            _cloudinaryServiceMock = new Mock<ICloudinaryService>();

            var validationService = new UserValidationService(_userRepositoryMock.Object);

            _userService = new UserService(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object,
                validationService,
                _achievementRepositoryMock.Object,
                _levelServiceMock.Object,
                _jwtServiceMock.Object,
                Mock.Of<ILogger<UserService>>(),
                _achievementManagerMock.Object,
                _userCleanupServiceMock.Object,
                _cloudinaryServiceMock.Object
            );
        }

        [Test]
        public void TC001_CreateAsync_WhenUserUnder18_ShouldThrowValidationException()
        {
            var dto = new UserCreateDto
            {
                FullName = "Test User",
                Email = "test@example.com",
                Nickname = "testuser",
                Password = "Password123",
                BirthDate = DateTime.UtcNow.AddYears(-17) // < 18 років
            };

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((NaviriaAPI.Entities.UserEntity?)null);
            _userRepositoryMock.Setup(x => x.GetByNicknameAsync(It.IsAny<string>()))
                .ReturnsAsync((NaviriaAPI.Entities.UserEntity?)null);

            var ex = Assert.ThrowsAsync<ValidationException>(async () =>
                await _userService.CreateAsync(dto));

            Assert.That(ex!.Message, Is.EqualTo("User must be at least 18 years old."));
        }

        [Test]
        public void TC002_CreateAsync_WhenInvalidEmail_ShouldThrowValidationException()
        {
            var dto = new UserCreateDto
            {
                FullName = "Test User",
                Email = "invalid-email",
                Nickname = "testuser",
                Password = "Password123",
                BirthDate = DateTime.UtcNow.AddYears(-20)
            };

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((NaviriaAPI.Entities.UserEntity?)null);
            _userRepositoryMock.Setup(x => x.GetByNicknameAsync(It.IsAny<string>()))
                .ReturnsAsync((NaviriaAPI.Entities.UserEntity?)null);

            var ex = Assert.ThrowsAsync<ValidationException>(async () =>
                await _userService.CreateAsync(dto));

            Assert.That(ex!.Message, Is.EqualTo("Email format is invalid."));
        }

        [Test]
        public void TC003_CreateAsync_WhenPasswordTooWeak_ShouldThrowValidationException()
        {
            var dto = new UserCreateDto
            {
                FullName = "Test User",
                Email = "test@example.com",
                Nickname = "testuser",
                Password = "weak",
                BirthDate = DateTime.UtcNow.AddYears(-20)
            };

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((NaviriaAPI.Entities.UserEntity?)null);
            _userRepositoryMock.Setup(x => x.GetByNicknameAsync(It.IsAny<string>()))
                .ReturnsAsync((NaviriaAPI.Entities.UserEntity?)null);

            var ex = Assert.ThrowsAsync<ValidationException>(async () =>
                await _userService.CreateAsync(dto));

            Assert.That(ex!.Message, Is.EqualTo("Password must be at least 8 characters long."));
        }

        [Test]
        public void TC004_CreateAsync_WhenEmailAlreadyExists_ShouldThrowEmailAlreadyExistException()
        {
            var dto = new UserCreateDto
            {
                FullName = "Test User",
                Email = "test@example.com",
                Nickname = "testuser",
                Password = "Password123",
                BirthDate = DateTime.UtcNow.AddYears(-20)
            };

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync(new UserEntity()); 

            var ex = Assert.ThrowsAsync<EmailAlreadyExistException>(async () =>
                await _userService.CreateAsync(dto));

            Assert.That(ex!.Message, Is.EqualTo("User with such email already exists"));
        }

        [Test]
        public void TC005_CreateAsync_WhenNicknameAlreadyExists_ShouldThrowNicknameAlreadyExistException()
        {
            var dto = new UserCreateDto
            {
                FullName = "Test User",
                Email = "test@example.com",
                Nickname = "testuser",
                Password = "Password123",
                BirthDate = DateTime.UtcNow.AddYears(-20)
            };

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync((UserEntity?)null);

            _userRepositoryMock.Setup(x => x.GetByNicknameAsync(dto.Nickname))
                .ReturnsAsync(new UserEntity()); 

            var ex = Assert.ThrowsAsync<NicknameAlreadyExistException>(async () =>
                await _userService.CreateAsync(dto));

            Assert.That(ex!.Message, Is.EqualTo("User with such nickname already exists"));
        }


    }
}