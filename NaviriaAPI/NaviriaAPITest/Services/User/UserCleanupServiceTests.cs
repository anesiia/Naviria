using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices.ICleanupServices;
using NUnit.Framework;
using NaviriaAPI.Services.CleanupServices;

namespace NaviriaAPI.Tests.Services.User
{
    [TestFixture]
    public class UserCleanupServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IFolderRepository> _folderRepositoryMock;
        private Mock<INotificationRepository> _notificationRepositoryMock;
        private Mock<IFriendRequestRepository> _friendRequestRepositoryMock;
        private Mock<IAssistantChatRepository> _assistantChatRepositoryMock;
        private Mock<ITaskRepository> _taskRepositoryMock;
        private Mock<ILogger<UserCleanupService>> _loggerMock;
        private UserCleanupService _service;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _folderRepositoryMock = new Mock<IFolderRepository>();
            _notificationRepositoryMock = new Mock<INotificationRepository>();
            _friendRequestRepositoryMock = new Mock<IFriendRequestRepository>();
            _assistantChatRepositoryMock = new Mock<IAssistantChatRepository>();
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _loggerMock = new Mock<ILogger<UserCleanupService>>();

            _service = new UserCleanupService(
                _userRepositoryMock.Object,
                _folderRepositoryMock.Object,
                _notificationRepositoryMock.Object,
                _friendRequestRepositoryMock.Object,
                _assistantChatRepositoryMock.Object,
             
                _taskRepositoryMock.Object
            );
        }

        [Test]
        public async Task TC001_DeleteUserAndRelatedDataAsync_UserNotFound_ReturnsFalse()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync("123")).ReturnsAsync((UserEntity?)null);

            var result = await _service.DeleteUserAndRelatedDataAsync("123");

            Assert.That(result, Is.False); 
            _loggerMock.VerifyLog(LogLevel.Warning, "User with ID", Times.Once());
        }

        [Test]
        public async Task TC002_DeleteUserAndRelatedDataAsync_DeleteSuccessful_ReturnsTrue()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync("123")).ReturnsAsync(new UserEntity());
            _userRepositoryMock.Setup(r => r.DeleteAsync("123")).ReturnsAsync(true);

            _folderRepositoryMock.Setup(r => r.DeleteManyByUserIdAsync("123")).Returns(Task.CompletedTask);
            _notificationRepositoryMock.Setup(r => r.DeleteManyByUserIdAsync("123")).Returns(Task.CompletedTask);
            _friendRequestRepositoryMock.Setup(r => r.DeleteManyByUserIdAsync("123")).Returns(Task.CompletedTask);
            _assistantChatRepositoryMock.Setup(r => r.DeleteManyByUserIdAsync("123")).Returns(Task.CompletedTask);
            _taskRepositoryMock.Setup(r => r.DeleteManyByUserIdAsync("123")).Returns(Task.CompletedTask);

            var result = await _service.DeleteUserAndRelatedDataAsync("123");

            Assert.That(result, Is.True);
            
        }

        [Test]
        public async Task TC003_DeleteUserAndRelatedDataAsync_DeleteFailed_ReturnsFalse()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync("123")).ReturnsAsync(new UserEntity());
            _userRepositoryMock.Setup(r => r.DeleteAsync("123")).ReturnsAsync(false);

            var result = await _service.DeleteUserAndRelatedDataAsync("123");

            Assert.That(result, Is.False);
            _loggerMock.VerifyLog(LogLevel.Error, "Failed to delete user", Times.Once());
        }

        [Test]
        public void TC004_DeleteUserAndRelatedDataAsync_ExceptionThrown_LogsAndThrows()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync("123")).ReturnsAsync(new UserEntity());
            _folderRepositoryMock.Setup(r => r.DeleteManyByUserIdAsync("123")).ThrowsAsync(new Exception("DB error"));

            var ex = Assert.ThrowsAsync<Exception>(() => _service.DeleteUserAndRelatedDataAsync("123"));

            Assert.That(ex?.Message, Is.EqualTo("DB error"));
            _loggerMock.VerifyLog(LogLevel.Error, "Error during cascade deletion for user 123", Times.Once());

        }
    }

    // Extension methods for verifying log calls
    public static class LoggerExtensions
    {
        public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, string messageFragment, Times times)
        {
            loggerMock.Verify(
                x => x.Log(
                    level,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains(messageFragment)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                times
            );
        }

        public static void VerifyLog(this Mock<ILogger> loggerMock, LogLevel level, string messageFragment, Times times)
        {
            loggerMock.Verify(
                x => x.Log(
                    level,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains(messageFragment)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                times
            );
        }
    }
}