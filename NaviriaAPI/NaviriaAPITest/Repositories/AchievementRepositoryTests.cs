using MongoDB.Driver;
using NaviriaAPI.Data;
using NaviriaAPI.Entities;
using NaviriaAPI.Repositories;
using NUnit.Framework;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NaviriaAPI.IRepositories;
using MongoDB.Bson;
using Microsoft.Extensions.Configuration;
using NaviriaAPI.Options;

namespace NaviriaAPI.Tests.Repositories
{
    [TestFixture]
    public class AchievementRepositoryTests
    {
        private IMongoDbContext _dbContext;
        private IAchievementRepository _achievementRepository;
        private IMongoCollection<AchievementEntity> _achievementCollection;

        [SetUp]
        public void SetUp()
        {
            // Load configuration directly from MongoDbSettings.json file
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("MongoDbSettings.json")
                .Build();

            // Fetch MongoDB settings from configuration using MongoDbOptions
            var mongoDbOptions = configuration.GetSection("MongoDbSettings").Get<MongoDbOptions>();

            // Mock the IOptions<MongoDbOptions> object
            var mockOptions = new Mock<IOptions<MongoDbOptions>>();
            mockOptions.Setup(o => o.Value).Returns(mongoDbOptions);

            // Create MongoDbContext using the mock settings
            _dbContext = new MongoDbContext(mockOptions.Object);
            _achievementRepository = new AchievementRepository(_dbContext);

            _achievementCollection = _dbContext.Achievements;

            // Ensure the collection is empty before each test to keep tests isolated
            _achievementCollection.DeleteMany(FilterDefinition<AchievementEntity>.Empty);
        }

        [TearDown]
        public void TearDown()
        {
            _achievementRepository = null;
        }

        [Test]
        public async Task TC001_CreateAsync_And_GetByIdAsync_ShouldWorkCorrectly()
        {
            var achievement = new AchievementEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "First Win",
                Description = "Awarded for the first win",
                Points = 100
            };

            await _achievementRepository.CreateAsync(achievement);
            var fetched = await _achievementRepository.GetByIdAsync(achievement.Id);

            Assert.That(fetched, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(fetched.Name, Is.EqualTo(achievement.Name));
                Assert.That(fetched.Description, Is.EqualTo(achievement.Description));
                Assert.That(fetched.Points, Is.EqualTo(100));
                
            });
        }

        [Test]
        public async Task TC002_GetAllAsync_ShouldReturnAllAchievements()
        {
            var achievement1 = new AchievementEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Test A",
                Description = "A",
                Points = 50
                
            };

            var achievement2 = new AchievementEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Test B",
                Description = "B",
                Points = 150
            };

            await _achievementRepository.CreateAsync(achievement1);
            await _achievementRepository.CreateAsync(achievement2);

            var all = await _achievementRepository.GetAllAsync();

            Assert.That(all.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task TC003_UpdateAsync_ShouldModifyAchievement()
        {
            var achievement = new AchievementEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Starter",
                Description = "Beginner Level",
                Points = 10
            };

            await _achievementRepository.CreateAsync(achievement);

            achievement.Description = "Updated Description";
            achievement.Points = 20;
           

            var updated = await _achievementRepository.UpdateAsync(achievement);
            var fetched = await _achievementRepository.GetByIdAsync(achievement.Id);

            Assert.That(updated, Is.True);
            Assert.Multiple(() =>
            {
                Assert.That(fetched.Description, Is.EqualTo("Updated Description"));
                Assert.That(fetched.Points, Is.EqualTo(20));
        
            });
        }

        [Test]
        public async Task TC004_DeleteAsync_ShouldRemoveAchievement()
        {
            var achievement = new AchievementEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "To Delete",
                Description = "Delete Me",
                Points = 0
            };

            await _achievementRepository.CreateAsync(achievement);

            var deleted = await _achievementRepository.DeleteAsync(achievement.Id);
            var fetched = await _achievementRepository.GetByIdAsync(achievement.Id);

            Assert.That(deleted, Is.True);
            Assert.That(fetched, Is.Null);
        }
    }
}