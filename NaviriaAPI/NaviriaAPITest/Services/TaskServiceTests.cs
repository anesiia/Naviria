//using Moq;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using NaviriaAPI.Services;
//using NaviriaAPI.IRepositories;
//using NaviriaAPI.IServices;
//using NaviriaAPI.DTOs.Task.Create;
//using NaviriaAPI.DTOs.Task.Update;
//using NaviriaAPI.DTOs.Task.View;
//using NaviriaAPI.Entities;
//using NaviriaAPI.Exceptions;
//using Microsoft.Extensions.Logging;
//using System.Linq;
//using NaviriaAPI.IServices.ISecurityService;
//using NaviriaAPI.IServices.IUserServices;
//using NaviriaAPI.IServices.IGamificationLogic;
//using NaviriaAPI.DTOs.FeaturesDTOs;
//using NaviriaAPI.DTOs.Task.Subtask.Create;
//using NaviriaAPI.Helpers;
//using System.ComponentModel.DataAnnotations;

//namespace NaviriaAPI.Tests.Services
//{
//    [TestFixture]
//    public class TaskServiceTests
//    {
//        private Mock<ITaskRepository> _taskRepoMock = null!;
//        private Mock<IFolderRepository> _folderRepoMock = null!;
//        private Mock<IUserService> _userServiceMock = null!;
//        private Mock<IMessageSecurityService> _messageSecurityMock = null!;
//        private Mock<ITaskRewardService> _taskRewardMock = null!;
//        private Mock<IAchievementManager> _achievementManagerMock = null!;
//        private Mock<ILogger<TaskService>> _loggerMock = null!;
//        private TaskService _taskService = null!;

//        [SetUp]
//        public void Setup()
//        {
//            _taskRepoMock = new Mock<ITaskRepository>();
//            _folderRepoMock = new Mock<IFolderRepository>();
//            _userServiceMock = new Mock<IUserService>();
//            _messageSecurityMock = new Mock<IMessageSecurityService>();
//            _taskRewardMock = new Mock<ITaskRewardService>();
//            _achievementManagerMock = new Mock<IAchievementManager>();
//            _loggerMock = new Mock<ILogger<TaskService>>();

//            _taskService = new TaskService(
//                _taskRepoMock.Object,
//                _loggerMock.Object,
//                _folderRepoMock.Object,
//                _userServiceMock.Object,
//                _messageSecurityMock.Object,
//                _taskRewardMock.Object,
//                _achievementManagerMock.Object);
//        }

//        [Test]
//        public void GetAllByUserAsync_UserNotFound_ThrowsNotFoundException()
//        {
//            _userServiceMock.Setup(s => s.UserExistsAsync("user1")).ReturnsAsync(false);

//            var ex = Assert.ThrowsAsync<NotFoundException>(async () =>
//                await _taskService.GetAllByUserAsync("user1"));

//            Assert.That(ex!.Message, Is.EqualTo("User with ID user1 not found."));
//        }

//        [Test]
//        public void GetAllByUserAsync_UserHasNoTasks_ThrowsNotFoundException()
//        {
//            _userServiceMock.Setup(s => s.UserExistsAsync("user1")).ReturnsAsync(true);
//            _taskRepoMock.Setup(r => r.GetAllByUserAsync("user1")).ReturnsAsync(new List<TaskEntity>());

//            var ex = Assert.ThrowsAsync<NotFoundException>(async () =>
//                await _taskService.GetAllByUserAsync("user1"));

//            Assert.That(ex!.Message, Is.EqualTo("User with ID user1 has no tasks."));
//        }

//        [Test]
//        public async Task GetAllByUserAsync_ValidUser_ReturnsTaskDtos()
//        {
//            _userServiceMock.Setup(s => s.UserExistsAsync("user1")).ReturnsAsync(true);

//            var taskEntity = new TaskEntity
//            {
//                Id = "task1",
//                UserId = "user1",
//                CreatedAt = DateTime.UtcNow
//            };
//            _taskRepoMock.Setup(r => r.GetAllByUserAsync("user1")).ReturnsAsync(new List<TaskEntity> { taskEntity });

//            var result = await _taskService.GetAllByUserAsync("user1");

//            Assert.That(result, Is.Not.Null);
//            Assert.That(result.Any(), Is.True);
//            Assert.That(result.First().Id, Is.EqualTo("task1"));
//        }

//        [Test]
//        public void GetByIdAsync_TaskNotFound_ThrowsNotFoundException()
//        {
//            _taskRepoMock.Setup(r => r.GetByIdAsync("task1")).ReturnsAsync((TaskEntity?)null);

//            var ex = Assert.ThrowsAsync<NotFoundException>(async () =>
//                await _taskService.GetByIdAsync("task1"));

//            Assert.That(ex!.Message, Is.EqualTo("Task with ID task1 not found."));
//        }

//        [Test]
//        public async Task GetByIdAsync_TaskFound_ReturnsTaskDto()
//        {
//            var taskEntity = new TaskEntity { Id = "task1", CreatedAt = DateTime.UtcNow };
//            _taskRepoMock.Setup(r => r.GetByIdAsync("task1")).ReturnsAsync(taskEntity);

//            var result = await _taskService.GetByIdAsync("task1");

//            Assert.That(result, Is.Not.Null);
//            Assert.That(result!.Id, Is.EqualTo("task1"));
//        }

//        [Test]
//        public void CreateAsync_UserNotFound_ThrowsNotFoundException()
//        {
//            var dto = new TaskCreateDto { UserId = "user1" };
//            _userServiceMock.Setup(s => s.UserExistsAsync("user1")).ReturnsAsync(false);

//            var ex = Assert.ThrowsAsync<NotFoundException>(async () =>
//                await _taskService.CreateAsync(dto));

//            Assert.That(ex!.Message, Is.EqualTo("User with ID user1 not found."));
//        }

//        [Test]
//        public void CreateAsync_SubtasksOnNonSubtaskType_ThrowsValidationException()
//        {
//            var dto = new TaskCreateDto
//            {
//                UserId = "user1",
//                Type = "simple",
//                Subtasks = new List<SubtaskCreateDtoBase> { new SubtaskCreateDtoBase() }
//            };
//            _userServiceMock.Setup(s => s.UserExistsAsync("user1")).ReturnsAsync(true);

//            var ex = Assert.ThrowsAsync<ValidationException>(async () =>
//                await _taskService.CreateAsync(dto));

//            Assert.That(ex!.Message, Is.EqualTo("Only tasks with TYPE 'with_subtasks' can have subtasks"));
//        }

//        [Test]
//        public async Task CreateAsync_ValidInput_CreatesTask()
//        {
//            var dto = new TaskCreateDto
//            {
//                UserId = "user1",
//                Type = "simple",
//                Title = "Title",
//                Description = "Description",
//                Tags = new List<string>()
//            };
//            _userServiceMock.Setup(s => s.UserExistsAsync("user1")).ReturnsAsync(true);
//            _messageSecurityMock.Setup(m => m.Validate("user1", "Title"));
//            _messageSecurityMock.Setup(m => m.Validate("user1", "Description"));
//            _taskRepoMock.Setup(r => r.CreateAsync(It.IsAny<TaskEntity>())).Returns(Task.CompletedTask);

//            var result = await _taskService.CreateAsync(dto);

//            Assert.That(result, Is.Not.Null);
//            Assert.That(result.Title, Is.EqualTo("Title"));
//            _taskRepoMock.Verify(r => r.CreateAsync(It.IsAny<TaskEntity>()), Times.Once);
//        }

//        [Test]
//        public void UpdateAsync_TaskNotFound_ThrowsNotFoundException()
//        {
//            _taskRepoMock.Setup(r => r.GetByIdAsync("task1")).ReturnsAsync((TaskEntity?)null);

//            var dto = new TaskUpdateDto();

//            var ex = Assert.ThrowsAsync<NotFoundException>(async () =>
//                await _taskService.UpdateAsync("task1", dto));

//            Assert.That(ex!.Message, Is.EqualTo("Task with ID task1 not found."));
//        }

//        [Test]
//        public async Task UpdateAsync_ValidUpdate_ReturnsTrue()
//        {
//            var existing = new TaskEntity
//            {
//                Id = "task1",
//                UserId = "user1",
//                Status = CurrentTaskStatus.InProgress
//            };

//            _taskRepoMock.Setup(r => r.GetByIdAsync("task1")).ReturnsAsync(existing);
//            _userServiceMock.Setup(s => s.GetByIdAsync("user1")).ReturnsAsync(new { Id = "user1" }); // could use a proper User DTO
//            _taskRewardMock.Setup(t => t.GrantTaskCompletionRewardsAsync(existing, It.IsAny<object>(), It.IsAny<CurrentTaskStatus>(), It.IsAny<CurrentTaskStatus>())).Returns(Task.CompletedTask);
//            _achievementManagerMock.Setup(a => a.EvaluateAsync("user1", AchievementTrigger.OnFirstTaskCompleted)).Returns(Task.CompletedTask);
//            _achievementManagerMock.Setup(a => a.EvaluateAsync("user1", AchievementTrigger.On5TaskInWeekTaskCompleted)).Returns(Task.CompletedTask);
//            _achievementManagerMock.Setup(a => a.EvaluateAsync("user1", AchievementTrigger.OnLongTaskCompleted, "task1")).Returns(Task.CompletedTask);
//            _taskRepoMock.Setup(r => r.UpdateAsync(It.IsAny<TaskEntity>())).ReturnsAsync(true);

//            var dto = new TaskUpdateDto
//            {
//                Status = CurrentTaskStatus.Completed
//            };

//            var result = await _taskService.UpdateAsync("task1", dto);

//            Assert.That(result, Is.True);
//            _taskRepoMock.Verify(r => r.UpdateAsync(It.IsAny<TaskEntity>()), Times.Once);
//        }

//        [Test]
//        public void DeleteAsync_TaskNotFound_ThrowsNotFoundException()
//        {
//            _taskRepoMock.Setup(r => r.GetByIdAsync("task1")).ReturnsAsync((TaskEntity?)null);

//            var ex = Assert.ThrowsAsync<NotFoundException>(async () =>
//                await _taskService.DeleteAsync("task1"));

//            Assert.That(ex!.Message, Is.EqualTo("Task with ID task1 not found."));
//        }

//        [Test]
//        public async Task DeleteAsync_TaskExists_ReturnsTrue()
//        {
//            var existing = new TaskEntity { Id = "task1" };
//            _taskRepoMock.Setup(r => r.GetByIdAsync("task1")).ReturnsAsync(existing);
//            _taskRepoMock.Setup(r => r.DeleteAsync("task1")).ReturnsAsync(true);

//            var result = await _taskService.DeleteAsync("task1");

//            Assert.That(result, Is.True);
//        }

//        [Test]
//        public async Task GetGroupedTasksByFoldersAsync_ValidInput_ReturnsGroupedTasks()
//        {
//            var userId = "user1";
//            _userServiceMock.Setup(s => s.UserExistsAsync(userId)).ReturnsAsync(true);

//            var tasks = new List<TaskEntity>
//            {
//                new TaskEntity { Id = "t1", FolderId = "f1", UserId = userId, CreatedAt = DateTime.UtcNow },
//                new TaskEntity { Id = "t2", FolderId = "f2", UserId = userId, CreatedAt = DateTime.UtcNow }
//            };
//            var folders = new List<FolderWithTasksDto>
//            {
//                new FolderWithTasksDto { FolderId = "f1", FolderName = "Folder 1", Tasks = new List<TaskDto>() },
//                new FolderWithTasksDto { FolderId = "f2", FolderName = "Folder 2", Tasks = new List<TaskDto>() }
//            };

//            _taskRepoMock.Setup(r => r.GetAllByUserAsync(userId)).ReturnsAsync(tasks);
//            _folderRepoMock.Setup(r => r.GetAllByUserIdAsync(userId)).ReturnsAsync(folders.Select(f => new { Id = f.FolderId, Name = f.FolderName }).ToList());

//            // Since FolderWithTasksDto requires Id and Name, you might need to mock FolderRepository to return a concrete class.
//            // For now, mock with a class that has Id and Name properties:
//            var folderEntities = new List<dynamic> {
//                new { Id = "f1", Name = "Folder 1" },
//                new { Id = "f2", Name = "Folder 2" }
//            };
//            _folderRepoMock.Setup(r => r.GetAllByUserIdAsync(userId)).ReturnsAsync(folderEntities);

//            var result = await _taskService.GetGroupedTasksByFoldersAsync(userId);

//            Assert.That(result, Is.Not.Null);
//            Assert.That(result.Count(), Is.EqualTo(2));
//        }

//        [Test]
//        public void GetTasksWithDeadlineAsync_NoTasks_ThrowsNotFoundException()
//        {
//            var date = DateTime.Today;
//            _taskRepoMock.Setup(r => r.GetTasksWithDeadlineAsync(date)).ReturnsAsync(new List<TaskEntity>());

//            var ex = Assert.ThrowsAsync<NotFoundException>(async () =>
//                await _taskService.GetTasksWithDeadlineAsync(date));

//            Assert.That(ex!.Message, Is.EqualTo("No tasks with deadline on " + date.ToShortDateString()));
//        }

//        [Test]
//        public async Task GetTasksWithDeadlineAsync_TasksFound_ReturnsTasks()
//        {
//            var date = DateTime.Today;
//            var tasks = new List<TaskEntity>
//            {
//                new TaskEntity { Id = "task1", Deadline = date }
//            };
//            _taskRepoMock.Setup(r => r.GetTasksWithDeadlineAsync(date)).ReturnsAsync(tasks);

//            var result = await _taskService.GetTasksWithDeadlineAsync(date);

//            Assert.That(result, Is.Not.Null);
//            Assert.That(result.Any(), Is.True);
//        }
//    }
//}


