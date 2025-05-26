using NUnit.Framework;
using Moq;
using NaviriaAPI.IServices;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Entities;
using NaviriaAPI.Entities.EmbeddedEntities.Subtasks;
using NaviriaAPI.Services;
using NaviriaAPI.DTOs.Task.Subtask.Create;
using NaviriaAPI.DTOs.Task.Subtask.Update;
using NaviriaAPI.DTOs.Task.Subtask.View;
using NaviriaAPI.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace NaviriaAPI.Tests.Services
{
    public class SubtaskServiceTests
    {
        private Mock<ITaskRepository> _taskRepositoryMock;
        private SubtaskService _subtaskService;

        [SetUp]
        public void Setup()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _subtaskService = new SubtaskService(_taskRepositoryMock.Object);
        }

        [Test]
        public async Task TC001_AddSubtaskAsync_ShouldAddSubtask_WhenTaskExists()
        {
            var taskId = "task123";
            var task = new TaskEntity { Id = taskId };
            var dto = new SubtaskStandardCreateDto
            {
                Title = "Subtask",
                Description = "Desc",
                IsCompleted = false
            };


            _taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);
            _taskRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<TaskEntity>())).ReturnsAsync(true);

            var result = await _subtaskService.AddSubtaskAsync(taskId, dto);

            Assert.That(result, Is.True);
            Assert.That(task.Subtasks.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task TC002_AddSubtaskAsync_ShouldReturnFalse_WhenTaskNotFound()
        {
            _taskRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((TaskEntity?)null);

            var dto = new SubtaskStandardCreateDto
            {
                Title = "Subtask",
                Description = "Description",
                IsCompleted = false
            };

            var result = await _subtaskService.AddSubtaskAsync("id", dto);


            Assert.That(result, Is.False);
        }

        [Test]
        public async Task TC003_UpdateSubtaskAsync_ShouldUpdate_WhenSubtaskExists()
        {
            var taskId = "taskId";
            var subtaskId = "subtaskId";

            var dto = new SubtaskStandardUpdateDto
            {
                Title = "NewTitle",
                Description = "NewDesc",
                IsCompleted = true
            };



            var task = new TaskEntity
            {
                Id = taskId,
                Subtasks = new List<SubtaskBase> { new SubtaskStandard { Id = subtaskId } }
            };

            _taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);
            _taskRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<TaskEntity>())).ReturnsAsync(true);

            var result = await _subtaskService.UpdateSubtaskAsync(taskId, subtaskId, dto);

            Assert.That(result, Is.True);
            Assert.That(task.Subtasks[0].Title, Is.EqualTo("NewTitle"));
        }

        [Test]
        public async Task TC004_UpdateSubtaskAsync_ShouldReturnFalse_WhenSubtaskNotFound()
        {
            var task = new TaskEntity { Subtasks = new List<SubtaskBase>() };
            _taskRepositoryMock.Setup(r => r.GetByIdAsync("task")).ReturnsAsync(task);

            var result = await _subtaskService.UpdateSubtaskAsync("task", "nonexistent", new SubtaskStandardUpdateDto());

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task TC005_RemoveSubtaskAsync_ShouldRemove_WhenExists()
        {
            var task = new TaskEntity
            {
                Subtasks = new List<SubtaskBase> { new SubtaskStandard { Id = "subtask" } }
            };

            _taskRepositoryMock.Setup(r => r.GetByIdAsync("task")).ReturnsAsync(task);
            _taskRepositoryMock.Setup(r => r.UpdateAsync(task)).ReturnsAsync(true);

            var result = await _subtaskService.RemoveSubtaskAsync("task", "subtask");

            Assert.That(result, Is.True);
            Assert.That(task.Subtasks, Is.Empty);
        }

        [Test]
        public async Task TC006_RemoveSubtaskAsync_ShouldReturnFalse_WhenSubtaskNotFound()
        {
            var task = new TaskEntity { Subtasks = new List<SubtaskBase>() };
            _taskRepositoryMock.Setup(r => r.GetByIdAsync("task")).ReturnsAsync(task);

            var result = await _subtaskService.RemoveSubtaskAsync("task", "missing");

            Assert.That(result, Is.False);
        }

        [Test]
        public void TC007_MarkRepeatableSubtaskCheckedInAsync_ShouldThrow_WhenTaskNotFound()
        {
            _taskRepositoryMock.Setup(r => r.GetByIdAsync("task")).ReturnsAsync((TaskEntity?)null);

            var ex = Assert.ThrowsAsync<NotFoundException>(() =>
                _subtaskService.MarkRepeatableSubtaskCheckedInAsync("task", "subtask", DateTime.Now));

            Assert.That(ex!.Message, Does.Contain("not found"));
        }

        [Test]
        public void TC008_MarkRepeatableSubtaskCheckedInAsync_ShouldThrow_WhenSubtaskNotRepeatable()
        {
            var task = new TaskEntity
            {
                Subtasks = new List<SubtaskBase> { new SubtaskStandard { Id = "subtask" } }
            };
            _taskRepositoryMock.Setup(r => r.GetByIdAsync("task")).ReturnsAsync(task);

            var ex = Assert.ThrowsAsync<ValidationException>(() =>
                _subtaskService.MarkRepeatableSubtaskCheckedInAsync("task", "subtask", DateTime.Now));

            Assert.That(ex!.Message, Does.Contain("not of type 'repeatable'"));
        }

        [Test]
        public void TC009_MarkRepeatableSubtaskCheckedInAsync_ShouldThrow_WhenDateNotAllowed()
        {
            var repeatable = new SubtaskRepeatable
            {
                Id = "subtask",
                RepeatDays = new List<DayOfWeek> { DayOfWeek.Monday }
            };

            var task = new TaskEntity { Subtasks = new List<SubtaskBase> { repeatable } };
            _taskRepositoryMock.Setup(r => r.GetByIdAsync("task")).ReturnsAsync(task);

            var invalidDate = new DateTime(2024, 10, 19); // Saturday

            var ex = Assert.ThrowsAsync<ValidationException>(() =>
                _subtaskService.MarkRepeatableSubtaskCheckedInAsync("task", "subtask", invalidDate));

            Assert.That(ex!.Message, Does.Contain("is not allowed"));
        }

        [Test]
        public async Task TC010_MarkRepeatableSubtaskCheckedInAsync_ShouldAddDate_WhenValid()
        {
            var date = new DateTime(2024, 10, 21); // Monday
            var repeatable = new SubtaskRepeatable
            {
                Id = "subtask",
                RepeatDays = new List<DayOfWeek> { date.DayOfWeek }
            };

            var task = new TaskEntity { Subtasks = new List<SubtaskBase> { repeatable } };
            _taskRepositoryMock.Setup(r => r.GetByIdAsync("task")).ReturnsAsync(task);
            _taskRepositoryMock.Setup(r => r.UpdateAsync(task)).ReturnsAsync(true);

            var result = await _subtaskService.MarkRepeatableSubtaskCheckedInAsync("task", "subtask", date);

            Assert.That(repeatable.CheckedInDays, Does.Contain(date.Date));
            Assert.That(result.Type, Is.EqualTo("repeatable"));
        }
    }
}
