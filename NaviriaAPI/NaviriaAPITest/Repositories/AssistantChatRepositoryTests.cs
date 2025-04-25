using MongoDB.Driver;
using NaviriaAPI.Data;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NaviriaAPI.Options;
using NaviriaAPI.Repositories;
using Moq;
using MongoDB.Bson;

namespace NaviriaAPI.Tests.Repositories
{
    [TestFixture]
    public class AssistantChatRepositoryTests
    {
        private IMongoDbContext _dbContext;
        private IAssistantChatRepository _assistantChatRepository;
        private IMongoCollection<AssistantChatMessageEntity> _aiChatMessagesCollection;
        private string _testUserId;

        [SetUp]
        public void SetUp()
        {
            // Завантажуємо налаштування з файлу MongoDbSettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("MongoDbSettings.json")
                .Build();

            var mongoDbOptions = configuration.GetSection("MongoDbSettings").Get<MongoDbOptions>();

            // Створюємо IOptions для передачі в MongoDbContext
            var mockOptions = new Mock<IOptions<MongoDbOptions>>();
            mockOptions.Setup(o => o.Value).Returns(mongoDbOptions);

            // Створюємо реальний контекст MongoDB
            _dbContext = new MongoDbContext(mockOptions.Object);

            // Ініціалізація репозиторію
            _assistantChatRepository = new AssistantChatRepository(_dbContext);

            // Отримуємо колекцію повідомлень
            _aiChatMessagesCollection = _dbContext.AssistantChatMessages;

            // Очищаємо колекцію перед кожним тестом
            _aiChatMessagesCollection.DeleteMany(FilterDefinition<AssistantChatMessageEntity>.Empty);

            // Генеруємо тестовий userId
            _testUserId = ObjectId.GenerateNewId().ToString();
        }

        [TearDown]
        public void TearDown()
        {
            // Очищаємо після кожного тесту
            _assistantChatRepository = null;
            _dbContext = null;
            _aiChatMessagesCollection = null;
        }

        //[Test]
        //public async Task AddMessageAsync_ValidMessage_AddsMessageToCollection()
        //{
        //    // Arrange
        //    var message = new AssistantChatMessageEntity
        //    {
        //        Id = "message1",
        //        UserId = _testUserId,
        //        Content = "Hello, how can I help?",
        //        CreatedAt = DateTime.UtcNow
        //    };

        //    // Act
        //    await _assistantChatRepository.AddMessageAsync(message);

        //    // Assert
        //    var insertedMessage = await _aiChatMessagesCollection
        //        .Find(m => m.UserId == _testUserId && m.Id == "message1")
        //        .FirstOrDefaultAsync();

        //    Assert.That(insertedMessage, Is.Not.Null);
        //    Assert.That(insertedMessage.Content, Is.EqualTo("Hello, how can I help?"));
        //}

        //[Test]
        //public async Task GetByUserIdAsync_ValidUserId_ReturnsMessages()
        //{
        //    // Arrange
        //    var message = new AssistantChatMessageEntity
        //    {
        //        Id = "message1",
        //        UserId = _testUserId,
        //        Content = "Hello",
        //        CreatedAt = DateTime.UtcNow
        //    };
        //    await _assistantChatRepository.AddMessageAsync(message);

        //    // Act
        //    var result = await _assistantChatRepository.GetByUserIdAsync(_testUserId);

        //    // Assert
        //    Assert.That(result, Is.Not.Empty);
        //    Assert.That(result.First().UserId, Is.EqualTo(_testUserId));
        //}

        [Test]
        public async Task GetByUserIdAsync_NoMessages_ReturnsEmptyList()
        {
            // Act
            var result = await _assistantChatRepository.GetByUserIdAsync(_testUserId);

            // Assert
            Assert.That(result, Is.Empty);
        }

        //[Test]
        //public async Task DeleteAllForUserAsync_ValidUserId_DeletesMessages()
        //{
        //    // Arrange
        //    var message = new AssistantChatMessageEntity
        //    {
        //        Id = "message1",
        //        UserId = _testUserId,
        //        Content = "Hello",
        //        CreatedAt = DateTime.UtcNow
        //    };
        //    await _assistantChatRepository.AddMessageAsync(message);

        //    // Act
        //    await _assistantChatRepository.DeleteAllForUserAsync(_testUserId);

        //    // Assert
        //    var result = await _aiChatMessagesCollection
        //        .Find(m => m.UserId == _testUserId)
        //        .ToListAsync();

        //    Assert.That(result, Is.Empty);
        //}

        //[Test]
        //public async Task CountByUserIdAsync_ValidUserId_ReturnsCorrectCount()
        //{
        //    // Arrange
        //    var message1 = new AssistantChatMessageEntity
        //    {
        //        Id = "message1",
        //        UserId = _testUserId,
        //        Content = "Hello",
        //        CreatedAt = DateTime.UtcNow
        //    };
        //    var message2 = new AssistantChatMessageEntity
        //    {
        //        Id = "message2",
        //        UserId = _testUserId,
        //        Content = "How are you?",
        //        CreatedAt = DateTime.UtcNow
        //    };
        //    await _assistantChatRepository.AddMessageAsync(message1);
        //    await _assistantChatRepository.AddMessageAsync(message2);

        //    // Act
        //    var result = await _assistantChatRepository.CountByUserIdAsync(_testUserId);

        //    // Assert
        //    Assert.That(result, Is.EqualTo(2));
        //}
    }
}
