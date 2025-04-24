using NaviriaAPI.DTOs.FeaturesDTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.Mappings;
using OpenAI.Chat;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NaviriaAPI.Services;
using Microsoft.Extensions.Configuration;


namespace NaviriaAPI.Tests.Services
{
    [TestFixture]
    public class AssistantChatServiceTests
    {
        private Mock<IAssistantChatRepository> _mockChatRepository;
        private Mock<ILogger<AssistantChatService>> _mockLogger;
        private AssistantChatService _service;

        [SetUp]
        public void SetUp()
        {
            _mockChatRepository = new Mock<IAssistantChatRepository>();
            _mockLogger = new Mock<ILogger<AssistantChatService>>();
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["OpenAIKey"]).Returns("fake-api-key");

            _service = new AssistantChatService(
                _mockChatRepository.Object,
                mockConfig.Object,
                _mockLogger.Object
            );
        }

        [Test]
        public async Task GetUserChatAsync_ShouldThrowArgumentException_WhenUserIdIsNullOrEmpty()
        {
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _service.GetUserChatAsync(string.Empty));
            Assert.That(ex.Message, Is.EqualTo("User ID cannot be null or empty. (Parameter 'userId')"));
        }

        [Test]
        public async Task GetUserChatAsync_ShouldReturnMessages_WhenUserIdIsValid()
        {
            var userId = "123";
            var chatMessages = new List<AssistantChatMessageEntity>
            {
                new AssistantChatMessageEntity { UserId = userId, Role = "user", Content = "Hello", CreatedAt = DateTime.UtcNow }
            };

            _mockChatRepository.Setup(repo => repo.GetByUserIdAsync(userId)).ReturnsAsync(chatMessages);

            var result = await _service.GetUserChatAsync(userId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Message, Is.EqualTo("Hello"));
        }

        [Test]
        public async Task SendMessageAsync_ShouldThrowArgumentException_WhenUserIdIsEmpty()
        {
            var dto = new AssistantChatMessageDto { UserId = "", Message = "Test message" };

            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _service.SendMessageAsync(dto));
            Assert.That(ex.Message, Is.EqualTo("User ID is required."));
        }

    //    [Test]
    //    public async Task SendMessageAsync_ShouldClearMessagesIfLimitExceeded()
    //    {
    //        var userId = "123";
    //        var dto = new AssistantChatMessageDto { UserId = userId, Message = "Message after limit" };

    //        // Мокаємо, що лічильник повідомлень досяг ліміту
    //        _mockChatRepository.Setup(repo => repo.CountByUserIdAsync(userId)).ReturnsAsync(20);
    //        _mockChatRepository.Setup(repo => repo.DeleteAllForUserAsync(userId)).Returns(Task.CompletedTask);

    //        // Мокаємо повернення повідомлень після очищення
    //        var mockHistory = new List<AssistantChatMessageEntity>
    //{
    //    new AssistantChatMessageEntity { UserId = userId, Role = "user", Content = "Previous message", CreatedAt = DateTime.UtcNow }
    //};
    //        _mockChatRepository.Setup(repo => repo.GetByUserIdAsync(userId)).ReturnsAsync(mockHistory);

    //        await _service.SendMessageAsync(dto);

    //        // Перевіряємо, чи викликана функція для видалення всіх повідомлень
    //        _mockChatRepository.Verify(repo => repo.DeleteAllForUserAsync(userId), Times.Once);

    //        // Перевірка, чи запит до OpenAI відправляється з історією
    //        _mockChatRepository.Verify(repo => repo.GetByUserIdAsync(userId), Times.Once);
    //    }




    }
}