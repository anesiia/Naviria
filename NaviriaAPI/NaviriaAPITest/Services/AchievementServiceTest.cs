using Moq;
using NUnit.Framework;
using NaviriaAPI.DTOs;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Services;
using FluentAssertions;
using System.Threading.Tasks;
using System.Collections.Generic;
using NaviriaAPI.Entities;

namespace NaviriaAPI.Tests.Services
{
    [TestFixture]
    public class AchievementServiceTest
    {
        private Mock<IAchievementRepository> _mockRepository;
        private AchievementService _achievementService;

        [SetUp]
        public void Setup()
        {
            // Ініціалізація мок репозиторію та сервісу
            _mockRepository = new Mock<IAchievementRepository>();
            _achievementService = new AchievementService(_mockRepository.Object);
        }

        // TC-01 Create Achievement with valid data
        [Test]
        public async Task TC01_CreateAchievement_ShouldReturnAchievementDto_WhenValidDataIsPassed()
        {
            // Arrange
            var createDto = new AchievementCreateDto
            {
                Name = "New Achievement",
                Description = "Description of new achievement"
            };

            // Mock the repository's CreateAsync method to return an entity with a valid Id
            var entity = new NaviriaAPI.Entities.AchievementEntity
            {
                Id = "1", 
                Name = createDto.Name,
                Description = createDto.Description
            };

            _mockRepository
                .Setup(repo => repo.CreateAsync(It.IsAny<NaviriaAPI.Entities.AchievementEntity>()))
                .Callback<NaviriaAPI.Entities.AchievementEntity>(e => e.Id = "1") // Ensure the entity gets an Id
                .Returns(Task.CompletedTask); // Return Task.CompletedTask as CreateAsync is void

            // Act
            var result = await _achievementService.CreateAsync(createDto);

            // Assert
            result.Id.Should().Be("1"); // The returned DTO should have Id = "1"
            result.Name.Should().Be(createDto.Name); // Ensure Name is correctly mapped
            result.Description.Should().Be(createDto.Description); // Ensure Description is correctly mapped

            _mockRepository.Verify(repo => repo.CreateAsync(It.IsAny<NaviriaAPI.Entities.AchievementEntity>()), Times.Once); // Verify the method was called once
        }

        // TC-02 Update Achievement with valid data
        [Test]
        public async Task TC02_UpdateAchievement_ShouldReturnTrue_WhenValidDataIsPassed()
        {
            // Arrange
            var updateDto = new AchievementUpdateDto
            {
                Name = "Updated Achievement",
                Description = "Updated description of achievement"
            };

            var mockRepo = new Mock<IAchievementRepository>();  // Мокуємо репозиторій
            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<AchievementEntity>()))  // Налаштовуємо мок для методу UpdateAsync
                .ReturnsAsync(true);  // Повертаємо true як результат виконання

            var service = new AchievementService(mockRepo.Object);  // Створюємо сервіс з мокованим репозиторієм

            // Act
            var result = await service.UpdateAsync("1", updateDto);  // Викликаємо метод сервісу

            // Assert
            result.Should().BeTrue();  // Перевіряємо, що результат true
            mockRepo.Verify(repo => repo.UpdateAsync(It.IsAny<AchievementEntity>()), Times.Once);  // Перевірка виклику методу UpdateAsync
        }

        // TC-03 Get Achievement by valid ID
        [Test]
        public async Task TC03_GetAchievement_ShouldReturnAchievementDto_WhenValidIdIsPassed()
        {
            // Arrange
            var achievement = new NaviriaAPI.Entities.AchievementEntity
            {
                Id = "1",
                Name = "First Achievement",
                Description = "This is the first achievement"
            };

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(achievement);

            // Act
            var result = await _achievementService.GetByIdAsync("1");

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be("1");
            result.Name.Should().Be("First Achievement");
            result.Description.Should().Be("This is the first achievement");

            _mockRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<string>()), Times.Once);
        }

        // TC-04 Get Achievement by invalid ID
        [Test]
        public async Task TC04_GetAchievement_ShouldReturnNull_WhenInvalidIdIsPassed()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((NaviriaAPI.Entities.AchievementEntity)null);

            // Act
            var result = await _achievementService.GetByIdAsync("99999");

            // Assert
            result.Should().BeNull();
            _mockRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<string>()), Times.Once);
        }

        // TC-05 Delete Achievement by valid ID
        [Test]
        public async Task TC05_DeleteAchievement_ShouldReturnTrue_WhenValidIdIsPassed()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.DeleteAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await _achievementService.DeleteAsync("1");

            // Assert
            result.Should().BeTrue();
            _mockRepository.Verify(repo => repo.DeleteAsync(It.IsAny<string>()), Times.Once);
        }

        // TC-06 Delete Achievement by invalid ID
        [Test]
        public async Task TC06_DeleteAchievement_ShouldReturnFalse_WhenInvalidIdIsPassed()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.DeleteAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var result = await _achievementService.DeleteAsync("99999");

            // Assert
            result.Should().BeFalse();
            _mockRepository.Verify(repo => repo.DeleteAsync(It.IsAny<string>()), Times.Once);
        }

        // TC-07 Get all Achievements
        [Test]
        public async Task TC07_GetAllAchievements_ShouldReturnListOfAchievements()
        {
            // Arrange
            var achievements = new List<NaviriaAPI.Entities.AchievementEntity>
    {
        new NaviriaAPI.Entities.AchievementEntity { Id = "1", Name = "First Achievement", Description = "This is the first achievement" },
        new NaviriaAPI.Entities.AchievementEntity { Id = "2", Name = "Second Achievement", Description = "This is the second achievement" }
    };

            _mockRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(achievements); // Return list of entities

            var service = new AchievementService(_mockRepository.Object); // Use your service with the mock repository

            // Act
            var result = await service.GetAllAsync();

            // Assert
            result.Should().HaveCount(2); // Check if the list has 2 items
            result.ToList()[0].Id.Should().Be("1"); // Convert to list for indexing and check the first item
            result.ToList()[1].Id.Should().Be("2"); // Check the second item

            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once); // Verify the method was called once
        }

    }
}
