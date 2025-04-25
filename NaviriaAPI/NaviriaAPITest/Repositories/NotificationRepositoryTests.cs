using MongoDB.Driver;
using Moq;
using NaviriaAPI.Data;
using NaviriaAPI.Entities;
using NaviriaAPI.Repositories;
using NaviriaAPI.IRepositories;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NaviriaAPI.Options;
using MongoDB.Bson;

namespace NaviriaAPI.Tests.Repositories
{
    [TestFixture]
    public class NotificationRepositoryTests
    {
        private IMongoDbContext _dbContext;
        private INotificationRepository _notificationRepository;
        private IMongoCollection<NotificationEntity> _notificationCollection;

        [SetUp]
        public void SetUp()
        {
            // Load configuration directly from MongoDbSettings.json file
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("MongoDbSettings.json")
                .Build();

            // Fetch MongoDB settings from configuration using MongoDbOptions
            var mongoDbOptions = configuration.GetSection("MongoDbSettings").Get<MongoDbOptions>();

            var options = Microsoft.Extensions.Options.Options.Create(mongoDbOptions);

            // Create MongoDbContext using the mock settings
            _dbContext = new MongoDbContext(options);
            _notificationRepository = new NotificationRepository(_dbContext);

            _notificationCollection = _dbContext.Notifications;

            // Ensure the collection is empty before each test to keep tests isolated
            _notificationCollection.DeleteMany(FilterDefinition<NotificationEntity>.Empty);
        }

        [TearDown]
        public void TearDown()
        {
            
            _notificationRepository = null;
        }

        [Test]
        public async Task TC001_CreateAsync_ShouldAddNotification()
        {
            var notification = new NotificationEntity
            {
                UserId = ObjectId.GenerateNewId().ToString(),
                Text = "Test notification",
                IsNew = true
            };

            await _notificationRepository.CreateAsync(notification);

            var result = await _notificationCollection.Find(n => n.UserId == notification.UserId).ToListAsync();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Text, Is.EqualTo("Test notification"));
        }

        [Test]
        public async Task TC002_GetAllAsync_ShouldReturnNotifications()
        {
            var notification1 = new NotificationEntity
            {
                UserId = ObjectId.GenerateNewId().ToString(),
                Text = "Test notification 1",
                IsNew = true
            };
            var notification2 = new NotificationEntity
            {
                UserId = ObjectId.GenerateNewId().ToString(),
                Text = "Test notification 2",
                IsNew = true
            };

            await _notificationRepository.CreateAsync(notification1);
            await _notificationRepository.CreateAsync(notification2);

            var result = await _notificationRepository.GetAllAsync();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(n => n.Text == "Test notification 1"));
            Assert.That(result.Any(n => n.Text == "Test notification 2"));
        }

        [Test]
        public async Task TC003_GetByIdAsync_ShouldReturnNotification()
        {
            var notification = new NotificationEntity
            {
                UserId = ObjectId.GenerateNewId().ToString(),
                Text = "Test notification",
                IsNew = true
            };

            await _notificationRepository.CreateAsync(notification);

            var result = await _notificationRepository.GetByIdAsync(notification.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Text, Is.EqualTo("Test notification"));
        }

        [Test]
        public async Task TC004_DeleteAsync_ShouldReturnTrue_WhenNotificationExists()
        {
            var notification = new NotificationEntity
            {
                UserId = ObjectId.GenerateNewId().ToString(),
                Text = "Test notification",
                IsNew = true
            };

            await _notificationRepository.CreateAsync(notification);

            var result = await _notificationRepository.DeleteAsync(notification.Id);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task TC005_DeleteAsync_ShouldReturnFalse_WhenNotificationDoesNotExist()
        {
            // Generate a valid ObjectId but don't insert it into the database
            var nonExistentId = ObjectId.GenerateNewId().ToString();

            // Try deleting a notification that doesn't exist
            var result = await _notificationRepository.DeleteAsync(nonExistentId);

            Assert.That(result, Is.False);
        }


        [Test]
        public async Task TC006_GetAllByUserAsync_ShouldReturnUserNotifications()
        {
            var userId1 = ObjectId.GenerateNewId().ToString();
            var userId2 = ObjectId.GenerateNewId().ToString();

            var notification1 = new NotificationEntity
            {
                UserId = userId1,
                Text = "Test notification 1",
                IsNew = true
            };
            var notification2 = new NotificationEntity
            {
                UserId = userId1,
                Text = "Test notification 2",
                IsNew = true
            };
            var notification3 = new NotificationEntity
            {
                UserId = userId2,
                Text = "Test notification 3",
                IsNew = true
            };

            await _notificationRepository.CreateAsync(notification1);
            await _notificationRepository.CreateAsync(notification2);
            await _notificationRepository.CreateAsync(notification3);

            var result = await _notificationRepository.GetAllByUserAsync(userId1.ToString());

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(n => n.Text == "Test notification 1"));
            Assert.That(result.Any(n => n.Text == "Test notification 2"));
        }

        [Test]
        public async Task TC007_UpdateStatusAsync_ShouldUpdateNotificationStatus()
        {
            var notification = new NotificationEntity
            {
                UserId = ObjectId.GenerateNewId().ToString(),
                Text = "Test notification",
                IsNew = true
            };

            await _notificationRepository.CreateAsync(notification);

            notification.IsNew = false;
            var result = await _notificationRepository.UpdateStatusAsync(notification);

            Assert.That(result, Is.True);

            var updatedNotification = await _notificationRepository.GetByIdAsync(notification.Id);
            Assert.That(updatedNotification.IsNew, Is.False);
        }

        [Test]
        public async Task TC008_MarkAllAsReadByUserAsync_ShouldMarkAllAsRead()
        {
            var userId = ObjectId.GenerateNewId().ToString();

            var notification1 = new NotificationEntity
            {
                UserId = userId,
                Text = "Test notification 1",
                IsNew = true
            };
            var notification2 = new NotificationEntity
            {
                UserId = userId,
                Text = "Test notification 2",
                IsNew = true
            };

            await _notificationRepository.CreateAsync(notification1);
            await _notificationRepository.CreateAsync(notification2);

            await _notificationRepository.MarkAllAsReadByUserAsync(userId.ToString());

            var updatedNotifications = await _notificationRepository.GetAllByUserAsync(userId.ToString());

            Assert.That(updatedNotifications.All(n => n.IsNew == false), Is.True);
        }
    }
}
