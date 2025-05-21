using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using NaviriaAPI.Services;
using NaviriaAPI.IRepositories;
using NaviriaAPI.DTOs.TaskDtos;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.DTOs.FeaturesDTOs;
using MongoDB.Bson;

namespace NaviriaAPI.Tests.Services
{
    [TestFixture]
    public class TaskServiceTests
    {
        private Mock<ITaskRepository> _taskRepoMock = null!;
        private Mock<IFolderRepository> _folderRepoMock = null!;
        private Mock<ILogger<TaskService>> _loggerMock = null!;
        private TaskService _service = null!;

        [SetUp]
        public void Setup()
        {
            _taskRepoMock = new Mock<ITaskRepository>();
            _folderRepoMock = new Mock<IFolderRepository>();
            _loggerMock = new Mock<ILogger<TaskService>>();
           // _service = new TaskService(_taskRepoMock.Object, _loggerMock.Object, _folderRepoMock.Object);
        }

        [Test]
        public async Task TC001_GetAllByUserAsync_ReturnsMappedTasks()
        {
            var userId = "user1";
            var tasks = new List<TaskEntity> { new TaskEntity { Id = "1", UserId = userId, Title = "Task 1" } };
            _taskRepoMock.Setup(r => r.GetAllByUserAsync(userId)).ReturnsAsync(tasks);

            var result = await _service.GetAllByUserAsync(userId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Title, Is.EqualTo("Task 1"));
        }

        [Test]
        public async Task TC002_GetByIdAsync_ReturnsDto_WhenFound()
        {
            var task = new TaskEntity { Id = "1", Title = "Task 1" };
            _taskRepoMock.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(task);

            var result = await _service.GetByIdAsync("1");

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Title, Is.EqualTo("Task 1"));
        }

        [Test]
        public async Task TC003_GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            _taskRepoMock.Setup(r => r.GetByIdAsync("1")).ReturnsAsync((TaskEntity?)null);

            var result = await _service.GetByIdAsync("1");

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TC004_CreateAsync_MapsAndSavesTask()
        {
            var dto = new TaskCreateDto { Title = "New Task", Description = "Desc" };
            TaskEntity? savedEntity = null;

            _taskRepoMock.Setup(r => r.CreateAsync(It.IsAny<TaskEntity>()))
                         .Callback<TaskEntity>(e => savedEntity = e)
                         .Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(dto);

            Assert.That(savedEntity, Is.Not.Null);
            Assert.That(savedEntity!.Title, Is.EqualTo(dto.Title));
            Assert.That(result.Title, Is.EqualTo(dto.Title));
        }

        [Test]
        public async Task TC005_UpdateAsync_ReturnsFalse_IfNotFound()
        {
            _taskRepoMock.Setup(r => r.GetByIdAsync("id")).ReturnsAsync((TaskEntity?)null);

            var result = await _service.UpdateAsync("id", new TaskUpdateDto());

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task TC006_UpdateAsync_UpdatesEntity_WhenFound()
        {
            var existing = new TaskEntity { Id = "id", Title = "Old" };
            var updated = new TaskUpdateDto { Title = "Updated" };

            _taskRepoMock.Setup(r => r.GetByIdAsync("id")).ReturnsAsync(existing);
            _taskRepoMock.Setup(r => r.UpdateAsync(It.IsAny<TaskEntity>())).ReturnsAsync(true);

            var result = await _service.UpdateAsync("id", updated);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task TC007_DeleteAsync_CallsRepository()
        {
            _taskRepoMock.Setup(r => r.DeleteAsync("id")).ReturnsAsync(true);

            var result = await _service.DeleteAsync("id");

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task TC008_GetGroupedTasksByFoldersAsync_GroupsCorrectly()
        {
            // Arrange
            var userId = "user1";
            var folderId = ObjectId.GenerateNewId().ToString();
            var tasks = new List<TaskEntity>
    {
        new TaskEntity { Id = "1", FolderId = folderId, Title = "T1" }
    };

            var folders = new List<FolderEntity>
    {
        new FolderEntity
        {
            Id = folderId,
            UserId = userId,
            Name = "Folder A",
            CreatedAt = DateTime.UtcNow
        }
    };

            _taskRepoMock.Setup(r => r.GetAllByUserAsync(userId)).ReturnsAsync(tasks);
            _folderRepoMock.Setup(r => r.GetAllByUserIdAsync(userId)).ReturnsAsync(folders);

            // Act
            var result = await _service.GetGroupedTasksByFoldersAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            var group = result.First();
            Assert.That(group.FolderId, Is.EqualTo(folderId));
            Assert.That(group.FolderName, Is.EqualTo("Folder A"));
            Assert.That(group.Tasks.Count, Is.EqualTo(1));
        }


        [Test]
        public async Task TC009_GetGroupedTasksByFoldersAsync_ReturnsUnknown_IfFolderMissing()
        {
            // Arrange
            var userId = "user1";
            var tasks = new List<TaskEntity>
    {
        new TaskEntity { Id = "1", FolderId = "unknown-folder", Title = "T1" }
    };

            _taskRepoMock.Setup(r => r.GetAllByUserAsync(userId)).ReturnsAsync(tasks);
            _folderRepoMock.Setup(r => r.GetAllByUserIdAsync(userId)).ReturnsAsync(new List<FolderEntity>());

            // Act
            var result = await _service.GetGroupedTasksByFoldersAsync(userId);

            // Assert
            Assert.That(result.First().FolderName, Is.EqualTo("Unknown"));
        }
      
    }
}