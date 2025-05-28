using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using NaviriaAPI.Entities;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Services.CleanupServices;

namespace NaviriaAPI.Tests.Services.CleanupServices
{
    [TestFixture]
    public class UserCleanupServiceTests
    {
        private Mock<IUserRepository> _userRepoMock = null!;
        private Mock<IFolderRepository> _folderRepoMock = null!;
        private Mock<INotificationRepository> _notificationRepoMock = null!;
        private Mock<IFriendRequestRepository> _friendRequestRepoMock = null!;
        private Mock<IAssistantChatRepository> _assistantChatRepoMock = null!;
        private Mock<ITaskRepository> _taskRepoMock = null!;
        private UserCleanupService _service = null!;

        [SetUp]
        public void SetUp()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _folderRepoMock = new Mock<IFolderRepository>();
            _notificationRepoMock = new Mock<INotificationRepository>();
            _friendRequestRepoMock = new Mock<IFriendRequestRepository>();
            _assistantChatRepoMock = new Mock<IAssistantChatRepository>();
            _taskRepoMock = new Mock<ITaskRepository>();

            _service = new UserCleanupService(
                _userRepoMock.Object,
                _folderRepoMock.Object,
                _notificationRepoMock.Object,
                _friendRequestRepoMock.Object,
                _assistantChatRepoMock.Object,
                _taskRepoMock.Object
            );
        }

        [Test]
        public void TC001_DeleteUserAndRelatedDataAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = "abc123";
            _userRepoMock.Setup(r => r.GetByIdAsync(userId))
                .ReturnsAsync((UserEntity?)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() =>
                _service.DeleteUserAndRelatedDataAsync(userId));

            Assert.That(ex!.Message, Is.EqualTo("User with ID abc123 not found."));
        }

        [Test]
        public async Task TC002_DeleteUserAndRelatedDataAsync_ShouldReturnTrue_WhenDeletionSucceeds()
        {
            // Arrange
            var userId = "user456";
            var user = new UserEntity { Id = userId, FullName = "Test User" };

            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            _userRepoMock.Setup(r => r.FindAllHavingFriendAsync(userId))
                .ReturnsAsync(new List<UserEntity>());
            _folderRepoMock.Setup(r => r.DeleteManyByUserIdAsync(userId)).Returns(Task.CompletedTask);
            _notificationRepoMock.Setup(r => r.DeleteManyByUserIdAsync(userId)).Returns(Task.CompletedTask);
            _friendRequestRepoMock.Setup(r => r.DeleteManyByUserIdAsync(userId)).Returns(Task.CompletedTask);
            _assistantChatRepoMock.Setup(r => r.DeleteManyByUserIdAsync(userId)).Returns(Task.CompletedTask);
            _taskRepoMock.Setup(r => r.DeleteManyByUserIdAsync(userId)).Returns(Task.CompletedTask);
            _userRepoMock.Setup(r => r.DeleteAsync(userId)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteUserAndRelatedDataAsync(userId);

            // Assert
            Assert.That(result, Is.True);
            _folderRepoMock.Verify(r => r.DeleteManyByUserIdAsync(userId), Times.Once);
            _notificationRepoMock.Verify(r => r.DeleteManyByUserIdAsync(userId), Times.Once);
            _friendRequestRepoMock.Verify(r => r.DeleteManyByUserIdAsync(userId), Times.Once);
            _assistantChatRepoMock.Verify(r => r.DeleteManyByUserIdAsync(userId), Times.Once);
            _taskRepoMock.Verify(r => r.DeleteManyByUserIdAsync(userId), Times.Once);
            _userRepoMock.Verify(r => r.DeleteAsync(userId), Times.Once);
        }

    }
}