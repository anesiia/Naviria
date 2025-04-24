using Moq;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Services.Validation;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using NaviriaAPI.DTOs.UpdateDTOs;

namespace NaviriaAPI.Tests.Services
{
    [TestFixture]
    public class UserValidationServiceTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private UserValidationService _validationService;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _validationService = new UserValidationService(_mockUserRepository.Object);
        }

        [Test]
        public async Task TC01_ValidateAsync_ShouldThrowArgumentException_WhenEmailExists()
        {
            // Arrange
            var dto = new UserCreateDto
            {
                Email = "existing@example.com",
                Nickname = "newUser",
                BirthDate = new DateTime(2000, 1, 1)
            };

            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(dto.Email))
                               .ReturnsAsync(new NaviriaAPI.Entities.UserEntity());  // Simulate existing user with this email

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(() => _validationService.ValidateAsync(dto));
            Assert.That(exception.Message, Is.EqualTo("User with this email already exists"));
        }

        [Test]
        public async Task TC02_ValidateAsync_ShouldThrowArgumentException_WhenNicknameExists()
        {
            // Arrange
            var dto = new UserCreateDto
            {
                Email = "newUser@example.com",
                Nickname = "existingNickname",
                BirthDate = new DateTime(2000, 1, 1)
            };

            _mockUserRepository.Setup(repo => repo.GetByNicknameAsync(dto.Nickname))
                               .ReturnsAsync(new NaviriaAPI.Entities.UserEntity());  // Simulate existing user with this nickname

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(() => _validationService.ValidateAsync(dto));
            Assert.That(exception.Message, Is.EqualTo("User with this nickname already exists"));
            
        }

        [Test]
        public async Task TC03_ValidateAsync_ShouldThrowValidationException_WhenBirthDateIsInTheFuture()
        {
            // Arrange
            var dto = new UserCreateDto
            {
                Email = "newUser@example.com",
                Nickname = "newUser",
                BirthDate = DateTime.UtcNow.AddDays(1),  // Future date
            };

            // Act & Assert
            var exception = Assert.ThrowsAsync<ValidationException>(() => _validationService.ValidateAsync(dto));
            Assert.That(exception.Message, Is.EqualTo("Birth date cannot be in the future"));
        }

        [Test]
        public async Task TC04_ValidateAsync_ShouldThrowValidationException_WhenUserIsUnder18()
        {
            // Arrange
            var dto = new UserCreateDto
            {
                Email = "newUser@example.com",
                Nickname = "newUser",
                BirthDate = DateTime.UtcNow.AddYears(-17),  // 17 years old
            };

            // Act & Assert
            var exception = Assert.ThrowsAsync<ValidationException>(() => _validationService.ValidateAsync(dto));
            Assert.That(exception.Message, Is.EqualTo("User must be at least 18 years old"));
        }

        [Test]
        public async Task TC05_ValidateAsync_ShouldThrowValidationException_WhenUserIsOver120()
        {
            // Arrange
            var dto = new UserCreateDto
            {
                Email = "newUser@example.com",
                Nickname = "newUser",
                BirthDate = DateTime.UtcNow.AddYears(-121)  // 121 years old
            };


            // Act & Assert
            var exception = Assert.ThrowsAsync<ValidationException>(() => _validationService.ValidateAsync(dto));
            Assert.That(exception.Message, Is.EqualTo("User cannot be older than 120 years"));
        }

        

        [Test]
        public async Task TC06_ValidateAsync_ShouldNotThrowAnyException_WhenUserIsValid()
        {
            // Arrange
            var dto = new UserCreateDto
            {
                Email = "newUser@example.com",
                Nickname = "newUser",
                BirthDate = DateTime.UtcNow.AddYears(-25)  // Valid birth date (25 years old)
            };

            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(dto.Email))
                               .ReturnsAsync((NaviriaAPI.Entities.UserEntity?)null);  // Simulate email does not exist

            _mockUserRepository.Setup(repo => repo.GetByNicknameAsync(dto.Nickname))
                               .ReturnsAsync((NaviriaAPI.Entities.UserEntity?)null);  // Simulate nickname does not exist

            // Act & Assert
            Assert.DoesNotThrowAsync(() => _validationService.ValidateAsync(dto));
        }

        [Test]
        public void TC07_ValidateAsync_ShouldThrowValidationException_WhenLastSeenIsInTheFuture()
        {
            // Arrange
            var dto = new UserUpdateDto
            {
                LastSeen = DateTime.UtcNow.AddMinutes(5) // майбутній час
            };

            // Act & Assert
            var exception = Assert.Throws<ValidationException>(() =>
                UserValidationService.ValidateAsync(dto));
            Assert.That(exception.Message, Is.EqualTo("LastSeen cannot be in the future"));
        }

    }
}
