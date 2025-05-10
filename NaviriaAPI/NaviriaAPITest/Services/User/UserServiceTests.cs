using Moq; // For mocking objects
using NaviriaAPI.Entities; // For entities like UserEntity, etc.
using NaviriaAPI.Entities.EmbeddedEntities; // For embedded entities (if needed)
using NUnit.Framework; // For writing NUnit tests
using NaviriaAPI.DTOs.CreateDTOs; // For CreateDTOs (if used in your test)
using NaviriaAPI.DTOs.UpdateDTOs; // For UpdateDTOs (if used in your test)
using NaviriaAPI.IServices; // For services that you are mocking (like IUserRepository, IJwtService)
using NaviriaAPI.Exceptions; // For exceptions (if you're testing exception handling)
using NaviriaAPI.Mappings; // For any mapping services
using Microsoft.Extensions.Configuration; // For IConfiguration
using NaviriaAPI.Services; // For actual service classes like UserService
using NaviriaAPI.IServices.ICloudStorage; // For cloud storage services (if used)
using NaviriaAPI.IServices.IGamificationLogic; // For gamification services (if used)
using NaviriaAPI.IServices.IJwtService; // For JWT services (if used)
using NaviriaAPI.Services.Validation; // For validation services (like UserValidationService)
using System; // For basic system types like exceptions
using System.Threading.Tasks; // For async tasks in tests
using Microsoft.AspNetCore.Identity; // For Identity-related services like IPasswordHasher
using NaviriaAPI.IRepositories; // For repositories like IUserRepository
using OpenAI.Chat; // For OpenAI integration (if used in the service)
using NaviriaAPI.Services.User; // For UserService and related classes
using Microsoft.Extensions.Logging; // For ILogger<UserService>


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
        private Mock<ILogger<UserService>> _loggerMock;
        private Mock<IAchievementManager> _achievementManagerMock;
        private Mock<IUserCleanupService> _userCleanupServiceMock;

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
            _loggerMock = new Mock<ILogger<UserService>>();
            _achievementManagerMock = new Mock<IAchievementManager>();
            _userCleanupServiceMock = new Mock<IUserCleanupService>();

            _configMock.Setup(c => c["OpenAIKey"]).Returns("fake-key");

            var validation = new UserValidationService(_userRepoMock.Object);
            _userService = new UserService(
                _userRepoMock.Object,
                _hasherMock.Object,
                _configMock.Object,
                validation,
                //_cloudinaryServiceMock.Object,
                _achievementRepoMock.Object,
                _levelServiceMock.Object,
                _jwtServiceMock.Object,
                 _loggerMock.Object,
                 _achievementManagerMock.Object,
                _userCleanupServiceMock.Object

            );
        }

        private UserEntity GetTestUser(string id = "123")
        {
            return new UserEntity
            {
                Id = id,
                FullName = "Test User",
                Nickname = "testuser",
                Gender = "Male",  // або інше значення, яке вам потрібно
                BirthDate = new DateTime(1990, 1, 1),
                Description = "Test description",
                Email = "test@example.com",
                Password = "hashedpass",
                Points = 0,
                LevelInfo = new LevelProgressInfo(),  // Якщо потрібні значення, додайте їх
                Friends = new List<UserFriendInfo>(),  // Якщо потрібні значення, додайте їх
                Achievements = new List<UserAchievementInfo>(),  // За потреби додайте ачивки
                FutureMessage = "Test future message",
                Photo = "https://example.com/photo.jpg",
                RegitseredAt = DateTime.UtcNow,
                LastSeen = DateTime.UtcNow,  // Можна налаштувати конвертацію часу при необхідності
                IsOnline = true,
                IsProUser = false
            };
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
                BirthDate = DateTime.UtcNow.AddYears(-25)
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
            //_levelServiceMock.Setup(service => service.CalculateLevelProgress(It.IsAny<int>())).Returns(new LevelProgressInfo { Level = 2 });
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
            _userCleanupServiceMock.Setup(service => service.DeleteUserAndRelatedDataAsync(userId)).ReturnsAsync(true);

            // Act
            var result = await _userService.DeleteAsync(userId);

            // Assert
            Assert.That(result, Is.True);
            _userCleanupServiceMock.Verify(service => service.DeleteUserAndRelatedDataAsync(userId), Times.Once);
        }



        [Test]
        public void TC006_GiveAchievementAsync_ShouldThrow_WhenAchievementAlreadyReceived()
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

            // Act & Assert
            var ex = Assert.ThrowsAsync<AlreadyExistException>(async () =>
                await _userService.GiveAchievementAsync(userId, achievementId));

            Assert.That(ex.Message, Does.Contain("already has achievement"));
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
            //_levelServiceMock.Setup(service => service.CalculateLevelProgress(It.IsAny<int>())).Returns(new LevelProgressInfo { Level = 2 });

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

        [Test]
        public async Task TC013_DeleteAsync_ShouldReturnFalse_WhenUserDeletionFails()
        {
            // Arrange
            var userId = "54321";
            _userCleanupServiceMock.Setup(service => service.DeleteUserAndRelatedDataAsync(userId)).ReturnsAsync(false);

            // Act
            var result = await _userService.DeleteAsync(userId);

            // Assert
            Assert.That(result, Is.False);
            _userCleanupServiceMock.Verify(service => service.DeleteUserAndRelatedDataAsync(userId), Times.Once);
        }


        [Test]
        public async Task TC014_UserExistsAsync_ShouldReturnTrue_WhenUserExists()
        {
            // Arrange
            var userId = "validUser";
            _userRepoMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(new UserEntity());

            // Act
            var result = await _userService.UserExistsAsync(userId);

            // Assert
            Assert.That(result, Is.True);
            _userRepoMock.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public async Task TC015_UserExistsAsync_ShouldReturnFalse_WhenUserIdIsNullOrWhitespace(string? invalidId)
        {
            // Act
            var result = await _userService.UserExistsAsync(invalidId);

            // Assert
            Assert.That(result, Is.False);
            _userRepoMock.Verify(repo => repo.GetByIdAsync(It.IsAny<string>()), Times.Never);
        }       


    }

}

