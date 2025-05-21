using Moq;
using NUnit.Framework;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices.IAuthServices;
using NaviriaAPI.IServices.IJwtService;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using NaviriaAPI.Services.AuthServices;

namespace NaviriaAPI.Tests.Services.AuthServices
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IPasswordHasher<UserEntity>> _mockPasswordHasher;
        private Mock<IJwtService> _mockJwtService;
        private IAuthService _authService;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher<UserEntity>>();
            _mockJwtService = new Mock<IJwtService>();

            _authService = new AuthService(
                _mockUserRepository.Object,
                _mockPasswordHasher.Object,
                _mockJwtService.Object
            );
        }

        [Test]
        public async Task TC01_AuthenticateAsync_ShouldReturnToken_WhenValidCredentials()
        {
            // Arrange
            var email = "johndoe@example.com";
            var password = "password123";
            var userEntity = new UserEntity
            {
                Id = "123",
                Email = email,
                Password = "hashed-password"
            };

            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(email)).ReturnsAsync(userEntity);
            _mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword(userEntity, userEntity.Password, password))
                                .Returns(PasswordVerificationResult.Success);
            _mockJwtService.Setup(jwt => jwt.GenerateUserToken(userEntity)).Returns("fake-jwt-token");

            // Act
            var result = await _authService.AuthenticateAsync(email, password);

            // Assert
            Assert.That(result, Is.EqualTo("fake-jwt-token"));
        }

        [Test]
        public void TC02_AuthenticateAsync_ShouldThrowArgumentException_WhenUserNotFound()
        {
            // Arrange
            var email = "nonexistentuser@example.com";
            var password = "password123";

            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(email)).ReturnsAsync((UserEntity)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _authService.AuthenticateAsync(email, password));
            Assert.That(ex.Message, Is.EqualTo("User with such email does not exist"));
        }

        [Test]
        public void TC03_AuthenticateAsync_ShouldThrowUnauthorizedAccessException_WhenInvalidPassword()
        {
            // Arrange
            var email = "johndoe@example.com";
            var password = "wrongpassword";
            var userEntity = new UserEntity
            {
                Id = "123",
                Email = email,
                Password = "hashed-password"
            };

            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(email)).ReturnsAsync(userEntity);
            _mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword(userEntity, userEntity.Password, password))
                                .Returns(PasswordVerificationResult.Failed);

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.AuthenticateAsync(email, password));
            Assert.That(ex.Message, Is.EqualTo("Invalid email or password"));
        }

        [Test]
        public async Task TC04_AuthenticateAsync_ShouldCallGenerateUserToken_WhenValidUser()
        {
            // Arrange
            var email = "johndoe@example.com";
            var password = "password123";
            var userEntity = new UserEntity
            {
                Id = "123",
                Email = email,
                Password = "hashed-password"
            };

            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(email)).ReturnsAsync(userEntity);
            _mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword(userEntity, userEntity.Password, password))
                                .Returns(PasswordVerificationResult.Success);
            _mockJwtService.Setup(jwt => jwt.GenerateUserToken(userEntity)).Returns("fake-jwt-token");

            // Act
            var result = await _authService.AuthenticateAsync(email, password);

            // Assert
            _mockJwtService.Verify(jwt => jwt.GenerateUserToken(userEntity), Times.Once);
        }
    }
}