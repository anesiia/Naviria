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
using NaviriaAPI.IServices.ISecurityService;
using NaviriaAPI.Exceptions;


namespace NaviriaAPI.Tests.Services
{
    [TestFixture]
    public class AssistantChatServiceTests
    {
        private Mock<IAssistantChatRepository> _mockChatRepository;
        private Mock<ILogger<AssistantChatService>> _mockLogger;
        private Mock<IUserService> _mockUserService;
        private Mock<IMessageSecurityService> _mockMessageSecurityService;
        private Mock<IFolderService> _mockFolderService;
        private Mock<ITaskService> _mockTaskService;
        private Mock<ICategoryService> _mockCategoryService;
        private AssistantChatService _service;

        [SetUp]
        public void SetUp()
        {
            _mockChatRepository = new Mock<IAssistantChatRepository>();
            _mockLogger = new Mock<ILogger<AssistantChatService>>();
            _mockUserService = new Mock<IUserService>();
            _mockMessageSecurityService = new Mock<IMessageSecurityService>();
            _mockFolderService = new Mock<IFolderService>();
            _mockTaskService = new Mock<ITaskService>();
            _mockCategoryService = new Mock<ICategoryService>();


            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["OpenAIKey"]).Returns("fake-api-key");

            _service = new AssistantChatService(
                _mockChatRepository.Object,
                mockConfig.Object,
                _mockLogger.Object,
                _mockFolderService.Object,
                _mockUserService.Object,
                _mockTaskService.Object,
                _mockMessageSecurityService.Object,
                _mockCategoryService.Object
            );
        }

        [Test]
        public async Task TC001_GetUserChatAsync_ShouldThrowArgumentException_WhenUserIdIsNullOrEmpty()
        {
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _service.GetUserChatAsync(string.Empty));
            Assert.That(ex.Message, Is.EqualTo("User ID cannot be null or empty. (Parameter 'userId')"));
        }


        [Test]
        public async Task TC002_SendMessageAsync_ShouldThrowArgumentException_WhenUserIdIsEmpty()
        {
            var dto = new AssistantChatMessageDto { UserId = "", Message = "Test message" };

            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _service.SendMessageAsync(dto));
            Assert.That(ex.Message, Is.EqualTo("User ID is required."));
        }

        [Test]
        public async Task TC003_GetUserChatAsync_ShouldReturnEmptyList_WhenNoMessagesExist()
        {
            var userId = "123";
            _mockUserService.Setup(s => s.UserExistsAsync(userId)).ReturnsAsync(true);
            _mockChatRepository.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(new List<AssistantChatMessageEntity>());

            var result = await _service.GetUserChatAsync(userId);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task TC004_GetUserChatAsync_ShouldReturnMessages_WhenUserIdIsValid()
        {
            var userId = "123";
            var chatMessages = new List<AssistantChatMessageEntity>
    {
        new AssistantChatMessageEntity { UserId = userId, Role = "user", Content = "Hello", CreatedAt = DateTime.UtcNow }
    };

            _mockUserService.Setup(u => u.UserExistsAsync(userId)).ReturnsAsync(true);
            _mockChatRepository.Setup(repo => repo.GetByUserIdAsync(userId)).ReturnsAsync(chatMessages);

            var result = await _service.GetUserChatAsync(userId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Message, Is.EqualTo("Hello"));
        }

        [Test]
        public void TC005_GetUserChatAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            var userId = "nonexistent";
            _mockUserService.Setup(s => s.UserExistsAsync(userId)).ReturnsAsync(false);

            var ex = Assert.ThrowsAsync<NotFoundException>(() => _service.GetUserChatAsync(userId));
            Assert.That(ex.Message, Is.EqualTo($"User with ID {userId} does not exist."));
        }

    }
}