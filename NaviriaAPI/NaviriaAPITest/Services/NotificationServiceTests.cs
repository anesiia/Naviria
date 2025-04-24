using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using NaviriaAPI.Services;

using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs;
using NaviriaAPI.Entities.EmbeddedEntities;
using System;
using System.Threading.Tasks;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Entities;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.Exceptions;

namespace NaviriaAPI.Tests.Services
{
    [TestFixture]
    public class NotificationServiceTests
    {
        private Mock<ILogger<NotificationService>> _loggerMock;
        private Mock<INotificationRepository> _repositoryMock;
        private NotificationService _service;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<NotificationService>>();
            _repositoryMock = new Mock<INotificationRepository>();
            _service = new NotificationService(_loggerMock.Object, _repositoryMock.Object);
        }

        [Test]
        public async Task TC001_CreateAsync_ValidInput_ReturnsNotificationDto()
        {
            // Arrange
            var createDto = new NotificationCreateDto
            {
                UserId = "user123",
                Text = "Test notification",
                RecievedAt = DateTime.UtcNow
            };

            var entity = new NotificationEntity
            {
                Id = "notif001",
                UserId = createDto.UserId,
                Text = createDto.Text,
                RecievedAt = createDto.RecievedAt,
                IsNew = true
            };

            _repositoryMock
                .Setup(repo => repo.CreateAsync(It.IsAny<NotificationEntity>()))
                .Returns(Task.CompletedTask)
                .Callback<NotificationEntity>(n => n.Id = "notif001"); // simulate setting ID in DB

            // Act
            var result = await _service.CreateAsync(createDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo("notif001"));
            Assert.That(result.UserId, Is.EqualTo("user123"));
            Assert.That(result.Text, Is.EqualTo("Test notification"));
        }

        [Test]
        public void TC002_CreateAsync_NullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _service.CreateAsync(null!));
            Assert.That(ex.ParamName, Is.EqualTo("notificationDto"));
        }

        [Test]
        public async Task TC003_GetAllAsync_WhenNoNotifications_ReturnsEmptyList()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<NotificationEntity>());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task TC004_GetAllAsync_WhenNotificationsExist_ReturnsNotificationDtos()
        {
            // Arrange
            var notifications = new List<NotificationEntity>
        {
            new NotificationEntity
            {
                Id = "notif001",
                UserId = "user1",
                Text = "Hello!",
                RecievedAt = DateTime.UtcNow,
                IsNew = true
            }
        };
            _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(notifications);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.First().Id, Is.EqualTo("notif001"));
        }

        [Test]
        public async Task TC005_GetByIdAsync_ValidId_ReturnsNotification()
        {
            // Arrange
            var id = "notif001";
            var entity = new NotificationEntity
            {
                Id = id,
                UserId = "user1",
                Text = "Hello!",
                RecievedAt = DateTime.UtcNow,
                IsNew = true
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(entity);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Id, Is.EqualTo(id));
        }

        [Test]
        public void TC006_GetByIdAsync_NullId_ThrowsArgumentException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.GetByIdAsync(""));
            Assert.That(ex.ParamName, Is.EqualTo("id"));
        }

        [Test]
        public async Task TC007_DeleteAsync_ValidId_ReturnsTrue()
        {
            // Arrange
            var id = "notif001";
            _repositoryMock.Setup(repo => repo.DeleteAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task TC008_DeleteAsync_FailedToDelete_ReturnsFalse()
        {
            // Arrange
            var id = "notif001";
            _repositoryMock.Setup(repo => repo.DeleteAsync(id)).ReturnsAsync(false);

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void TC009_DeleteAsync_EmptyId_ThrowsArgumentException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.DeleteAsync(" "));
            Assert.That(ex.ParamName, Is.EqualTo("id"));
        }

        [Test]
        public async Task TC010_MarkAllUserNotificationsAsReadAsync_ValidUserId_CallsRepositoryMethod()
        {
            // Arrange
            var userId = "user123";
            _repositoryMock.Setup(repo => repo.MarkAllAsReadByUserAsync(userId)).Returns(Task.CompletedTask);

            // Act
            await _service.MarkAllUserNotificationsAsReadAsync(userId);

            // Assert
            _repositoryMock.Verify(repo => repo.MarkAllAsReadByUserAsync(userId), Times.Once);
        }

        [Test]
        public void TC011_MarkAllUserNotificationsAsReadAsync_EmptyUserId_ThrowsArgumentException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.MarkAllUserNotificationsAsReadAsync(" "));
            Assert.That(ex.ParamName, Is.EqualTo("userId"));
        }

        [Test]
        public async Task TC012_UpdateStatusAsync_ValidId_ReturnsTrue()
        {
            // Arrange
            var id = "notif001";
            var updateDto = new NotificationUpdateDto { IsNew = false, UserId = "user1" }; // Added UserId
            var entity = new NotificationEntity
            {
                Id = id,
                UserId = "user1",
                Text = "Hello!",
                RecievedAt = DateTime.UtcNow,
                IsNew = true
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(entity);
            _repositoryMock.Setup(repo => repo.UpdateStatusAsync(It.IsAny<NotificationEntity>())).ReturnsAsync(true);

            // Act
            var result = await _service.UpdateStatusAsync(id, updateDto);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task TC013_UpdateStatusAsync_FailedToUpdate_ThrowsFailedToUpdateException()
        {
            // Arrange
            var id = "notif001";
            var updateDto = new NotificationUpdateDto { IsNew = false, UserId = "user1" }; // Added UserId
            var entity = new NotificationEntity
            {
                Id = id,
                UserId = "user1",
                Text = "Hello!",
                RecievedAt = DateTime.UtcNow,
                IsNew = true
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(entity);
            _repositoryMock.Setup(repo => repo.UpdateStatusAsync(It.IsAny<NotificationEntity>())).ReturnsAsync(false);

            // Act & Assert
            var ex = Assert.ThrowsAsync<FailedToUpdateException>(async () =>
                await _service.UpdateStatusAsync(id, updateDto));

            Assert.That(ex.Message, Is.EqualTo("Failed to update notification status."));
        }

        [Test]
        public async Task TC014_UpdateStatusAsync_EmptyId_ThrowsArgumentException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.UpdateStatusAsync("", new NotificationUpdateDto { IsNew = false, UserId = "user1" }));
            Assert.That(ex.ParamName, Is.EqualTo("id"));
        }

        [Test]
        public async Task TC015_GetAllUserNotificationsAsync_EmptyUserId_ThrowsArgumentException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.GetAllUserNotificationsAsync(""));
            Assert.That(ex.ParamName, Is.EqualTo("userId"));
        }

        [Test]
        public async Task TC016_GetAllUserNotificationsAsync_ValidUserId_ReturnsNotifications()
        {
            // Arrange
            var userId = "user123";
            var notifications = new List<NotificationEntity>
        {
            new NotificationEntity
            {
                Id = "notif001",
                UserId = userId,
                Text = "Hello!",
                RecievedAt = DateTime.UtcNow,
                IsNew = true
            }
        };

            _repositoryMock.Setup(repo => repo.GetAllByUserAsync(userId)).ReturnsAsync(notifications);

            // Act
            var result = await _service.GetAllUserNotificationsAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.First().UserId, Is.EqualTo(userId));
        }

        [Test]
        public async Task TC017_GetAllUserNotificationsAsync_NoNotifications_ReturnsEmptyList()
        {
            // Arrange
            var userId = "user123";
            _repositoryMock.Setup(repo => repo.GetAllByUserAsync(userId)).ReturnsAsync(new List<NotificationEntity>());

            // Act
            var result = await _service.GetAllUserNotificationsAsync(userId);

            // Assert
            Assert.That(result, Is.Empty);
        }
    }
}