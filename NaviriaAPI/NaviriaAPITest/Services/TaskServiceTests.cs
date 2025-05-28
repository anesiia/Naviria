using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NaviriaAPI.Services;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.DTOs.Task.Create;
using NaviriaAPI.DTOs.Task.Update;
using NaviriaAPI.DTOs.Task.View;
using NaviriaAPI.Entities;
using NaviriaAPI.Exceptions;
using Microsoft.Extensions.Logging;
using System.Linq;
using NaviriaAPI.IServices.ISecurityService;
using NaviriaAPI.IServices.IUserServices;
using NaviriaAPI.IServices.IGamificationLogic;
using NaviriaAPI.DTOs.FeaturesDTOs;
using NaviriaAPI.DTOs.Task.Subtask.Create;
using NaviriaAPI.Helpers;
using System.ComponentModel.DataAnnotations;
using NaviriaAPI.Entities.EmbeddedEntities;

namespace NaviriaAPI.Tests.Services
{
    [TestFixture]
    public class TaskServiceTests
    {
        private Mock<ITaskRepository> _taskRepoMock = null!;
        private Mock<IFolderRepository> _folderRepoMock = null!;
        private Mock<IUserService> _userServiceMock = null!;
        private Mock<IMessageSecurityService> _messageSecurityMock = null!;
        private Mock<ITaskRewardService> _taskRewardMock = null!;
        private Mock<IAchievementManager> _achievementManagerMock = null!;
        private Mock<ILogger<TaskService>> _loggerMock = null!;
        private TaskService _taskService = null!;

        [SetUp]
        public void Setup()
        {
            _taskRepoMock = new Mock<ITaskRepository>();
            _folderRepoMock = new Mock<IFolderRepository>();
            _userServiceMock = new Mock<IUserService>();
            _messageSecurityMock = new Mock<IMessageSecurityService>();
            _taskRewardMock = new Mock<ITaskRewardService>();
            _achievementManagerMock = new Mock<IAchievementManager>();
            _loggerMock = new Mock<ILogger<TaskService>>();

            _taskService = new TaskService(
                _taskRepoMock.Object,
                _loggerMock.Object,
                _folderRepoMock.Object,
                _userServiceMock.Object,
                _messageSecurityMock.Object,
                _taskRewardMock.Object,
                _achievementManagerMock.Object);
        }

        [Test]
        public void TC001_GetAllByUserAsync_UserNotFound_ThrowsNotFoundException()
        {
            _userServiceMock.Setup(s => s.UserExistsAsync("user1")).ReturnsAsync(false);

            var ex = Assert.ThrowsAsync<NotFoundException>(async () =>
                await _taskService.GetAllByUserAsync("user1"));

            Assert.That(ex!.Message, Is.EqualTo("User with ID user1 not found."));
        }

        [Test]
        public void TC002_GetAllByUserAsync_UserHasNoTasks_ThrowsNotFoundException()
        {
            _userServiceMock.Setup(s => s.UserExistsAsync("user1")).ReturnsAsync(true);
            _taskRepoMock.Setup(r => r.GetAllByUserAsync("user1")).ReturnsAsync(new List<TaskEntity>());

            var ex = Assert.ThrowsAsync<NotFoundException>(async () =>
                await _taskService.GetAllByUserAsync("user1"));

            Assert.That(ex!.Message, Is.EqualTo("User with ID user1 has no tasks."));
        }

        [Test]
        public async Task TC003_GetAllByUserAsync_ValidUser_ReturnsTaskDtos()
        {
            _userServiceMock.Setup(s => s.UserExistsAsync("user1")).ReturnsAsync(true);

            var taskEntity = new TaskEntity
            {
                Id = "task1",
                UserId = "user1",
                CreatedAt = DateTime.UtcNow
            };
            _taskRepoMock.Setup(r => r.GetAllByUserAsync("user1")).ReturnsAsync(new List<TaskEntity> { taskEntity });

            var result = await _taskService.GetAllByUserAsync("user1");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Any(), Is.True);
            Assert.That(result.First().Id, Is.EqualTo("task1"));
        }

        [Test]
        public void TC004_GetByIdAsync_TaskNotFound_ThrowsNotFoundException()
        {
            _taskRepoMock.Setup(r => r.GetByIdAsync("task1")).ReturnsAsync((TaskEntity?)null);

            var ex = Assert.ThrowsAsync<NotFoundException>(async () =>
                await _taskService.GetByIdAsync("task1"));

            Assert.That(ex!.Message, Is.EqualTo("Task with ID task1 not found."));
        }

        [Test]
        public async Task TC005_GetByIdAsync_TaskFound_ReturnsTaskDto()
        {
            var taskEntity = new TaskEntity { Id = "task1", CreatedAt = DateTime.UtcNow };
            _taskRepoMock.Setup(r => r.GetByIdAsync("task1")).ReturnsAsync(taskEntity);

            var result = await _taskService.GetByIdAsync("task1");

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo("task1"));
        }

        [Test]
        public void TC006_CreateAsync_UserNotFound_ThrowsNotFoundException()
        {
            var dto = new TaskCreateDto { UserId = "user1" };
            _userServiceMock.Setup(s => s.UserExistsAsync("user1")).ReturnsAsync(false);

            var ex = Assert.ThrowsAsync<NotFoundException>(async () =>
                await _taskService.CreateAsync(dto));

            Assert.That(ex!.Message, Is.EqualTo("User with ID user1 not found."));
        }

        [Test]
        public void TC007_CreateAsync_SubtasksOnNonSubtaskType_ThrowsValidationException()
        {
            var dto = new TaskCreateDto
            {
                UserId = "user1",
                Type = "simple",
                Subtasks = new List<SubtaskCreateDtoBase> { new SubtaskStandardCreateDto() }
            };
            _userServiceMock.Setup(s => s.UserExistsAsync("user1")).ReturnsAsync(true);

            var ex = Assert.ThrowsAsync<ValidationException>(async () =>
                await _taskService.CreateAsync(dto));

            Assert.That(ex!.Message, Is.EqualTo("Only tasks with TYPE 'with_subtasks' can have subtasks"));
        }


        [Test]
        public void TC008_UpdateAsync_TaskNotFound_ThrowsNotFoundException()
        {
            _taskRepoMock.Setup(r => r.GetByIdAsync("task1")).ReturnsAsync((TaskEntity?)null);

            var dto = new TaskUpdateDto();

            var ex = Assert.ThrowsAsync<NotFoundException>(async () =>
                await _taskService.UpdateAsync("task1", dto));

            Assert.That(ex!.Message, Is.EqualTo("Task with ID task1 not found."));
        }


        [Test]
        public void TC009_DeleteAsync_TaskNotFound_ThrowsNotFoundException()
        {
            _taskRepoMock.Setup(r => r.GetByIdAsync("task1")).ReturnsAsync((TaskEntity?)null);

            var ex = Assert.ThrowsAsync<NotFoundException>(async () =>
                await _taskService.DeleteAsync("task1"));

            Assert.That(ex!.Message, Is.EqualTo("Task with ID task1 not found."));
        }

        [Test]
        public async Task TC010_DeleteAsync_TaskExists_ReturnsTrue()
        {
            var existing = new TaskEntity { Id = "task1" };
            _taskRepoMock.Setup(r => r.GetByIdAsync("task1")).ReturnsAsync(existing);
            _taskRepoMock.Setup(r => r.DeleteAsync("task1")).ReturnsAsync(true);

            var result = await _taskService.DeleteAsync("task1");

            Assert.That(result, Is.True);
        }

    }
}


