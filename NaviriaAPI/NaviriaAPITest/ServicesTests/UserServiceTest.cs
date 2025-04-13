using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NaviriaAPI.Services;
using NaviriaAPI.DTOs;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Services.Validation;
using NaviriaAPI.IServices.ICloudStorage;

namespace NaviriaAPITest.ServicesTests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepoMock;
        private Mock<IPasswordHasher<UserEntity>> _hasherMock;
        private Mock<IConfiguration> _configMock;
        private Mock<UserValidationService> _validationMock;
        private Mock<ICloudinaryService> _cloudinaryServiceMock; //
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _hasherMock = new Mock<IPasswordHasher<UserEntity>>();
            _configMock = new Mock<IConfiguration>();
            _cloudinaryServiceMock = new Mock<ICloudinaryService>();

            _configMock.Setup(c => c["OpenAIKey"]).Returns("fake-key");

            var validation = new UserValidationService(_userRepoMock.Object); // реальний інстанс
            _userService = new UserService(
                _userRepoMock.Object,
                _hasherMock.Object,
                _configMock.Object,
                validation,
                _cloudinaryServiceMock.Object

            );
        }

        [Test]
        public async Task TC01_CreateAsync_ShouldReturnUserDto_WhenUserIsCreated()
        {
            // Arrange
            var createDto = new UserCreateDto
            {
                FullName = "John Doe",
                Nickname = "johnd",
                Email = "john@example.com",
                Password = "Pass123!",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                LastSeen = DateTime.UtcNow
            };

            _userRepoMock.Setup(r => r.GetByEmailAsync(createDto.Email)).ReturnsAsync((UserEntity?)null);
            _userRepoMock.Setup(r => r.GetByNicknameAsync(createDto.Nickname)).ReturnsAsync((UserEntity?)null);
            _hasherMock.Setup(h => h.HashPassword(It.IsAny<UserEntity>(), createDto.Password))
                       .Returns("hashedPassword");
            _userRepoMock.Setup(r => r.CreateAsync(It.IsAny<UserEntity>())).Returns(Task.CompletedTask);

            // Act
            var result = await _userService.CreateAsync(createDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Nickname, Is.EqualTo(createDto.Nickname));
        }


        [Test]
        public async Task TC02_GetByIdAsync_ShouldReturnUserDto_WhenUserExists()
        {
            // Arrange
            var entity = new UserEntity
            {
                Id = "1",
                Nickname = "jane",
                Email = "jane@example.com",
                LastSeen = DateTime.UtcNow
            };

            _userRepoMock.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(entity);

            // Act
            var result = await _userService.GetByIdAsync("1");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Nickname, Is.EqualTo("jane"));
        }
        [Test]
        public async Task TC03_UpdateAsync_ShouldReturnTrue_WhenUpdateSucceeds()
        {
            // Arrange
            var updateDto = new UserUpdateDto
            {
                Nickname = "updated",
                LastSeen = DateTime.UtcNow
            };

            _userRepoMock.Setup(r => r.UpdateAsync(It.IsAny<UserEntity>())).ReturnsAsync(true);

            // Act
            var result = await _userService.UpdateAsync("1", updateDto);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task TC04_DeleteAsync_ShouldReturnTrue_WhenUserDeleted()
        {
            // Arrange
            _userRepoMock.Setup(r => r.DeleteAsync("1")).ReturnsAsync(true);

            // Act
            var result = await _userService.DeleteAsync("1");

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task TC05_GetAllAsync_ShouldReturnListOfUserDtos()
        {
            // Arrange
            var users = new List<UserEntity>
            {
                new UserEntity { Id = "1", Nickname = "a", LastSeen = DateTime.UtcNow },
                new UserEntity { Id = "2", Nickname = "b", LastSeen = DateTime.UtcNow }
            };

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.Any(u => u.Nickname == "a"), Is.True);
            Assert.That(result.Any(u => u.Nickname == "b"), Is.True);
        }
    }
}
