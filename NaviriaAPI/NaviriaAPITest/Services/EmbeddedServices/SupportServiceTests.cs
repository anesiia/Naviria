using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Services.EmbeddedServices;
using NUnit.Framework;

namespace NaviriaAPI.Tests.Services.EmbeddedServices
{
    [TestFixture]
    public class SupportServiceTests
    {
        private Mock<IQuoteRepository> _quoteRepoMock;
        private Mock<INotificationRepository> _notificationRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        private Mock<ILogger<SupportService>> _loggerMock;
        private SupportService _supportService;

        [SetUp]
        public void Setup()
        {
            _quoteRepoMock = new Mock<IQuoteRepository>();
            _notificationRepoMock = new Mock<INotificationRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<SupportService>>();
            _supportService = new SupportService(
                _quoteRepoMock.Object,
                _notificationRepoMock.Object,
                _userRepoMock.Object,
                _loggerMock.Object
            );
        }


        [Test]
        public void TC001_SendSupportAsync_EmptyUserIds_ThrowsArgumentException()
        {
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _supportService.SendSupportAsync("", null));

            Assert.That(ex!.Message, Is.EqualTo("User IDs cannot be null or empty."));
        }
        [Test]
        public async Task SendSupportAsync_ValidData_SendsNotification()
        {
            // Arrange
            var senderId = "sender123";
            var receiverId = "receiver456";
            var sender = new UserEntity { Id = senderId, FullName = "Sender", Nickname = "Helper" };
            var receiver = new UserEntity { Id = receiverId, FullName = "Receiver", Nickname = "Needy" };
            var quotes = new List<QuoteEntity> { new QuoteEntity { Text = "Keep going!" } };
            var notifications = new List<NotificationEntity>();

            _userRepoMock.Setup(r => r.GetByIdAsync(senderId)).ReturnsAsync(sender);
            _userRepoMock.Setup(r => r.GetByIdAsync(receiverId)).ReturnsAsync(receiver);
            _quoteRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(quotes);
            _notificationRepoMock.Setup(r => r.GetAllByUserAsync(receiverId)).ReturnsAsync(notifications);
            _notificationRepoMock.Setup(r => r.CreateAsync(It.IsAny<NotificationEntity>())).Returns(Task.CompletedTask);

            // Act
            await _supportService.SendSupportAsync(senderId, receiverId);

            // Assert
            _notificationRepoMock.Verify(repo => repo.CreateAsync(It.Is<NotificationEntity>(
                n => n.UserId == receiverId && n.Text.Contains("Helper"))), Times.Once);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("Support sent")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once
            );
        }

        [TestCase(null, "receiverId")]
        [TestCase("senderId", null)]
        [TestCase(" ", "receiverId")]
        [TestCase("senderId", "")]
        public void TC002_SendSupportAsync_InvalidIds_ThrowsArgumentException(object? senderIdObj, object? receiverIdObj)
        {
            var senderId = senderIdObj as string;
            var receiverId = receiverIdObj as string;

            var ex = Assert.ThrowsAsync<ArgumentException>(() =>
                _supportService.SendSupportAsync(senderId!, receiverId!));

            Assert.That(ex!.Message, Is.EqualTo("User IDs cannot be null or empty."));
        }


        [Test]
        public void TC003_SendSupportAsync_UserNotFound_ThrowsArgumentException()
        {
            _userRepoMock.Setup(r => r.GetByIdAsync("sender")).ReturnsAsync((UserEntity?)null);
            _userRepoMock.Setup(r => r.GetByIdAsync("receiver")).ReturnsAsync(new UserEntity());

            var ex = Assert.ThrowsAsync<ArgumentException>(() =>
                _supportService.SendSupportAsync("sender", "receiver"));

            Assert.That(ex!.Message, Is.EqualTo("One or both users not found."));
        }

        [Test]
        public void TC004_SendSupportAsync_NoQuotes_ThrowsInvalidOperationException()
        {
            _userRepoMock.Setup(r => r.GetByIdAsync("sender")).ReturnsAsync(new UserEntity { Nickname = "Nick" });
            _userRepoMock.Setup(r => r.GetByIdAsync("receiver")).ReturnsAsync(new UserEntity());
            _quoteRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<QuoteEntity>());
            _notificationRepoMock.Setup(r => r.GetAllByUserAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<NotificationEntity>());

            var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
                _supportService.SendSupportAsync("sender", "receiver"));

            Assert.That(ex!.Message, Is.EqualTo("No quotes available to send."));
        }

        [Test]
        public void TC005_SendSupportAsync_AlreadySentRecently_ThrowsInvalidOperationException()
        {
            var sender = new UserEntity { Id = "1", Nickname = "Supporter" };
            var receiver = new UserEntity { Id = "2" };

            _userRepoMock.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(sender);
            _userRepoMock.Setup(r => r.GetByIdAsync("2")).ReturnsAsync(receiver);

            var recentNotification = new NotificationEntity
            {
                UserId = "2",
                Text = "Some text with Supporter",
                RecievedAt = DateTime.UtcNow.AddHours(-2)
            };

            _notificationRepoMock.Setup(r => r.GetAllByUserAsync("2")).ReturnsAsync(new List<NotificationEntity> { recentNotification });

            var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
                _supportService.SendSupportAsync("1", "2"));

            Assert.That(ex!.Message, Does.Contain("once every 24 hours"));
        }


    }
}