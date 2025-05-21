using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using NaviriaAPI.Entities;
using NaviriaAPI.Exceptions;
using NaviriaAPI.Services.CleanupServices;
using NaviriaAPI.IRepositories;

namespace NaviriaAPI.Tests.Services.CleanupServices
{
    [TestFixture]
    public class FolderCleanupServiceTests
    {
        private Mock<IFolderRepository> _folderRepositoryMock;
        private Mock<ITaskRepository> _taskRepositoryMock;
        private FolderCleanupService _folderCleanupService;

        [SetUp]
        public void SetUp()
        {
            _folderRepositoryMock = new Mock<IFolderRepository>();
            _taskRepositoryMock = new Mock<ITaskRepository>();

            _folderCleanupService = new FolderCleanupService(
                _folderRepositoryMock.Object,
                _taskRepositoryMock.Object);
        }

        [Test]
        public void TC001_DeleteFolderAndTasksAsync_WhenFolderDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var folderId = "nonexistent-folder-id";
            _folderRepositoryMock.Setup(repo => repo.GetByIdAsync(folderId))
                                 .ReturnsAsync((FolderEntity)null!);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() =>
                _folderCleanupService.DeleteFolderAndTasksAsync(folderId));

            Assert.That(ex!.Message, Is.EqualTo($" Folder with ID {folderId} not found."));
        }

        [Test]
        public async Task TC002_DeleteFolderAndTasksAsync_WhenFolderExists_DeletesTasksAndFolder()
        {
            // Arrange
            var folderId = "valid-folder-id";
            var folder = new FolderEntity { Id = folderId, Name = "My Folder", UserId = "user1" };

            _folderRepositoryMock.Setup(repo => repo.GetByIdAsync(folderId))
                                 .ReturnsAsync(folder);

            _taskRepositoryMock.Setup(repo => repo.DeleteManyByFolderIdAsync(folderId))
                               .Returns(Task.CompletedTask);

            _folderRepositoryMock.Setup(repo => repo.DeleteAsync(folderId))
                                 .ReturnsAsync(true);

            // Act
            var result = await _folderCleanupService.DeleteFolderAndTasksAsync(folderId);

            // Assert
            _taskRepositoryMock.Verify(repo => repo.DeleteManyByFolderIdAsync(folderId), Times.Once);
            _folderRepositoryMock.Verify(repo => repo.DeleteAsync(folderId), Times.Once);
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task TC003_DeleteFolderAndTasksAsync_WhenDeleteFolderReturnsFalse_ReturnsFalse()
        {
            // Arrange
            var folderId = "valid-folder-id";
            var folder = new FolderEntity { Id = folderId, Name = "My Folder", UserId = "user1" };

            _folderRepositoryMock.Setup(repo => repo.GetByIdAsync(folderId))
                                 .ReturnsAsync(folder);

            _taskRepositoryMock.Setup(repo => repo.DeleteManyByFolderIdAsync(folderId))
                               .Returns(Task.CompletedTask);

            _folderRepositoryMock.Setup(repo => repo.DeleteAsync(folderId))
                                 .ReturnsAsync(false);

            // Act
            var result = await _folderCleanupService.DeleteFolderAndTasksAsync(folderId);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}