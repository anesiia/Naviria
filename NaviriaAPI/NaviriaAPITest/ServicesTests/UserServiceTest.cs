using Moq;
using NUnit.Framework;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.Services;
using NaviriaAPI.Services.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace NaviriaAPITest.ServicesTests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IPasswordHasher<UserEntity>> _mockPasswordHasher;
        private Mock<UserValidationService> _mockValidationService;
        private Mock<IConfiguration> _mockConfig;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher<UserEntity>>();
            _mockValidationService = new Mock<UserValidationService>();
            _mockConfig = new Mock<IConfiguration>();

            // Mock the configuration to return a fake OpenAI key
            _mockConfig.Setup(config => config["OpenAIKey"]).Returns("fake-openai-key");

            _userService = new UserService(
                _mockUserRepository.Object,
                _mockPasswordHasher.Object,
                _mockConfig.Object,
                _mockValidationService.Object
            );
        }

        //[Test]
        //public async Task CreateAsync_ShouldReturnUserDto_WhenValidUser()
        //{
        //    // Arrange
        //    var newUserDto = new UserCreateDto
        //    {
        //        FullName = "John Doe",
        //        Nickname = "JohnD",
        //        Email = "johndoe@example.com",
        //        Password = "Password123",
        //        LastSeen = DateTime.UtcNow
        //    };

        //    var userEntity = new UserEntity
        //    {
        //        Id = "123",
        //        FullName = "John Doe",
        //        Nickname = "JohnD",
        //        Email = "johndoe@example.com",
        //        Password = "hashed-password"
        //    };

        //    _mockPasswordHasher.Setup(ph => ph.HashPassword(It.IsAny<UserEntity>(), It.IsAny<string>())).Returns("hashed-password");
        //    _mockUserRepository.Setup(repo => repo.CreateAsync(It.IsAny<UserEntity>())).Returns(Task.CompletedTask);

        //    // Act
        //    var result = await _userService.CreateAsync(newUserDto);

        //    // Assert
        //    Assert.That(result, Is.Not.Null);
        //    Assert.That(result.FullName, Is.EqualTo("John Doe"));
        //    Assert.That(result.Nickname, Is.EqualTo("JohnD"));
        //    Assert.That(result.Email, Is.EqualTo("johndoe@example.com"));
        //}

        //[Test]
        //public async Task UpdateAsync_ShouldReturnTrue_WhenUserUpdated()
        //{
        //    // Arrange
        //    var userUpdateDto = new UserUpdateDto
        //    {
        //        FullName = "John Updated",
        //        Nickname = "JohnU",
        //        Email = "johnupdated@example.com",
        //        LastSeen = DateTime.UtcNow
        //    };

        //    var userEntity = new UserEntity
        //    {
        //        Id = "123",
        //        FullName = "John Doe",
        //        Nickname = "JohnD",
        //        Email = "johndoe@example.com",
        //        Password = "hashed-password"
        //    };

        //    _mockUserRepository.Setup(repo => repo.UpdateAsync(It.IsAny<UserEntity>())).ReturnsAsync(true);

        //    // Act
        //    var result = await _userService.UpdateAsync("123", userUpdateDto);

        //    // Assert
        //    Assert.That(result, Is.True);
        //}

        //[Test]
        //public async Task GetAiAnswerAsync_ShouldReturnAnswer_WhenValidQuestion()
        //{
        //    // Arrange
        //    var question = "What's the weather like today?";
        //    var mockAnswer = "The weather is sunny.";

        //    var mockChatClient = new Mock<ChatClient>("gpt-4o-mini", "fake-openai-key");
        //    var chatMessage = new ChatMessage { Text = mockAnswer };
        //    var chatCompletion = new ChatCompletion { Content = new List<ChatMessage> { chatMessage } };

        //    mockChatClient.Setup(client => client.CompleteChat(It.IsAny<string>())).ReturnsAsync(new ClientResult<ChatCompletion>
        //    {
        //        Value = chatCompletion
        //    });

        //    // Act
        //    var result = await _userService.GetAiAnswerAsync(question);

        //    // Assert
        //    Assert.AreEqual(mockAnswer, result);
        //}
    }
}
