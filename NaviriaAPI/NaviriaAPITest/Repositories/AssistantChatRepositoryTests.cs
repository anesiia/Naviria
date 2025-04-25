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
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("MongoDbSettings.json")
                .Build();

            var mongoDbOptions = configuration.GetSection("MongoDbSettings").Get<MongoDbOptions>();
            var options = Microsoft.Extensions.Options.Options.Create(mongoDbOptions); // Вказуємо правильний простір імен

            // Створюємо контекст з реальними налаштуваннями
            _dbContext = new MongoDbContext(options);  // Потрібно передавати IOptions<MongoDbOptions>

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

        [Test]
        public async Task TC001_AddMessageAsync_ShouldAddMessage()
        {
            var message = new AssistantChatMessageEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UserId = ObjectId.GenerateNewId().ToString(),
                Content = "Hello, this is a test message",
                CreatedAt = DateTime.UtcNow
            };

            // Додаємо повідомлення
            await _assistantChatRepository.AddMessageAsync(message);

            // Перевіряємо, чи повідомлення є в колекції
            var fetchedMessage = await _assistantChatRepository.GetByUserIdAsync(message.UserId);

            Assert.That(fetchedMessage.Count(), Is.EqualTo(1));
            Assert.That(fetchedMessage.First().Content, Is.EqualTo("Hello, this is a test message"));
        }


        [Test]
        public async Task TC002_GetByUserIdAsync_ShouldReturnMessagesForUser()
        {
            var message1 = new AssistantChatMessageEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UserId = _testUserId,
                Content = "First message",
                CreatedAt = DateTime.UtcNow.AddMinutes(-10)
            };
            var message2 = new AssistantChatMessageEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UserId = _testUserId,
                Content = "Second message",
                CreatedAt = DateTime.UtcNow
            };

            await _assistantChatRepository.AddMessageAsync(message1);
            await _assistantChatRepository.AddMessageAsync(message2);

            // Отримуємо всі повідомлення для користувача
            var messages = await _assistantChatRepository.GetByUserIdAsync(_testUserId);

            Assert.That(messages.Count(), Is.EqualTo(2));
            Assert.That(messages.First().Content, Is.EqualTo("First message"));
            Assert.That(messages.Last().Content, Is.EqualTo("Second message"));
        }

        [Test]
        public async Task TC003_DeleteAllForUserAsync_ShouldRemoveAllMessagesForUser()
        {
            var message1 = new AssistantChatMessageEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UserId = _testUserId,
                Content = "Message 1",
                CreatedAt = DateTime.UtcNow.AddMinutes(-10)
            };
            var message2 = new AssistantChatMessageEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UserId = _testUserId,
                Content = "Message 2",
                CreatedAt = DateTime.UtcNow
            };

            await _assistantChatRepository.AddMessageAsync(message1);
            await _assistantChatRepository.AddMessageAsync(message2);

            // Видаляємо всі повідомлення для користувача
            await _assistantChatRepository.DeleteAllForUserAsync(_testUserId);

            // Перевіряємо, що повідомлень для цього користувача більше не залишилось
            var messages = await _assistantChatRepository.GetByUserIdAsync(_testUserId);

            Assert.That(messages.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task TC004_CountByUserIdAsync_ShouldReturnCorrectMessageCount()
        {
            var message1 = new AssistantChatMessageEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UserId = _testUserId,
                Content = "Message 1",
                CreatedAt = DateTime.UtcNow.AddMinutes(-10)
            };
            var message2 = new AssistantChatMessageEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UserId = _testUserId,
                Content = "Message 2",
                CreatedAt = DateTime.UtcNow
            };

            await _assistantChatRepository.AddMessageAsync(message1);
            await _assistantChatRepository.AddMessageAsync(message2);

            // Перевіряємо кількість повідомлень для цього користувача
            var messageCount = await _assistantChatRepository.CountByUserIdAsync(_testUserId);

            Assert.That(messageCount, Is.EqualTo(2));
        }

        [Test]
        public async Task TC005_CountByUserIdAsync_ShouldReturnZeroWhenNoMessages()
        {
            // Перевіряємо кількість повідомлень для користувача без повідомлень
            var messageCount = await _assistantChatRepository.CountByUserIdAsync(_testUserId);

            Assert.That(messageCount, Is.EqualTo(0));
        }

        //[Test]
        //public async Task GetByUserIdAsync_NoMessages_ReturnsEmptyList()
        //{
        //    // Act
        //    var result = await _assistantChatRepository.GetByUserIdAsync(_testUserId);

        //    // Assert
        //    Assert.That(result, Is.Empty);
        //}

    }
}
