using Moq;
using NUnit.Framework;
using NaviriaAPI.Entities;
using NaviriaAPI.Entities.EmbeddedEntities;
using NaviriaAPI.Entities.EmbeddedEntities.Subtasks;
using NaviriaAPI.Helpers;
using NaviriaAPI.IServices.IGamificationLogic;
using NaviriaAPI.IServices.IUserServices;
using NaviriaAPI.DTOs.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NaviriaAPI.Services.GamificationLogic;

namespace NaviriaAPI.Tests.Services.GamificationLogic
{
    [TestFixture]
    public class TaskRewardServiceTests
    {
        private Mock<ILevelService> _levelServiceMock;
        private Mock<IUserService> _userServiceMock;
        private TaskRewardService _service;

        [SetUp]
        public void SetUp()
        {
            _levelServiceMock = new Mock<ILevelService>();
            _userServiceMock = new Mock<IUserService>();
            _service = new TaskRewardService(_levelServiceMock.Object, _userServiceMock.Object);
        }

        [Test]
        public async Task TC001_GrantTaskCompletionRewardsAsync_ShouldReturnPoints_AndUpdateUser_WhenCompletedInTime()
        {
            // Arrange
            var task = new TaskEntity
            {
                Status = CurrentTaskStatus.InProgress,
                Priority = 3,
                IsDeadlineOn = true,
                Deadline = DateTime.UtcNow.AddHours(1),
                Tags = new List<Tags> { new Tags { TagName = "test" }, new Tags { TagName = "urgent" } },
                Subtasks = new List<SubtaskBase>
                {
                    new SubtaskRepeatable
                    {
                        CheckedInDays = new List<DateTime> { DateTime.UtcNow.AddDays(-1), DateTime.UtcNow }
                    }
                }
            };

            var user = new UserDto
            {
                Id = "user123",
                LevelInfo = new LevelProgressInfo { TotalXp = 0 },
                Points = 0
            };

            _levelServiceMock.Setup(x => x.CalculateLevelProgressAsync(It.IsAny<UserDto>(), It.IsAny<int>()))
                .ReturnsAsync(new LevelProgressInfo { TotalXp = 100 });

            // Act
            int result = await _service.GrantTaskCompletionRewardsAsync(
                task,
                user,
                CurrentTaskStatus.InProgress,
                CurrentTaskStatus.CompletedInTime);

            // Assert
            Assert.That(result, Is.GreaterThan(0));
            Assert.That(task.Status, Is.EqualTo(CurrentTaskStatus.CompletedInTime));
            Assert.That(user.Points, Is.EqualTo(100));

        }

        [Test]
        public async Task TC002_GrantTaskCompletionRewardsAsync_ShouldSetCompletedNotInTime_WhenDeadlinePassed()
        {
            var task = new TaskEntity
            {
                Status = CurrentTaskStatus.InProgress,
                IsDeadlineOn = true,
                Deadline = DateTime.UtcNow.AddHours(-2)
            };

            var user = new UserDto { Id = "user123" };

            var points = await _service.GrantTaskCompletionRewardsAsync(task, user, CurrentTaskStatus.InProgress, CurrentTaskStatus.Completed);

            Assert.That(task.Status, Is.EqualTo(CurrentTaskStatus.CompletedNotInTime));
            Assert.That(points, Is.EqualTo(0));
        }

        [Test]
        public async Task TC003_GrantTaskCompletionRewardsAsync_ShouldSetCompleted_WhenNoDeadline()
        {
            var task = new TaskEntity
            {
                Status = CurrentTaskStatus.InProgress,
                IsDeadlineOn = false
            };

            var user = new UserDto { Id = "user123" };

            _levelServiceMock.Setup(x => x.CalculateLevelProgressAsync(It.IsAny<UserDto>(), It.IsAny<int>()))
                .ReturnsAsync(new LevelProgressInfo { TotalXp = 50 });

            int points = await _service.GrantTaskCompletionRewardsAsync(task, user, CurrentTaskStatus.InProgress, CurrentTaskStatus.Completed);

            Assert.That(task.Status, Is.EqualTo(CurrentTaskStatus.Completed));
            Assert.That(points, Is.GreaterThan(0));
        }

        [Test]
        public void TC004_CalculateTaskPoints_ShouldReturnCorrectPoints()
        {
            var task = new TaskEntity
            {
                Priority = 4,
                Tags = new List<Tags> { new Tags { TagName = "a" }, new Tags { TagName = "b" }, new Tags { TagName = "a" } },
                Subtasks = new List<SubtaskBase>
                {
                    new SubtaskRepeatable
                    {
                        CheckedInDays = new List<DateTime> { DateTime.UtcNow, DateTime.UtcNow.AddDays(-1) }
                    },
                    new SubtaskRepeatable
                    {
                        CheckedInDays = new List<DateTime> { DateTime.UtcNow }
                    }
                }
            };

            int expected = 10  // base
                          + 4 * 2 // priority = 8
                          + 2 * 3 // 2 subtasks = 6
                          + 2     // 2 distinct tags
                          + 3 * 2; // 3 CheckedInDays = 6

            int result = _service.CalculateTaskPoints(task);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public async Task TC005_GrantTaskCompletionRewardsAsync_ShouldSetCompletedNotInTime_WhenStatusWasDeadlineMissed()
        {
            var task = new TaskEntity();
            var user = new UserDto();

            var result = await _service.GrantTaskCompletionRewardsAsync(task, user, CurrentTaskStatus.DeadlineMissed, CurrentTaskStatus.Completed);

            Assert.That(task.Status, Is.EqualTo(CurrentTaskStatus.CompletedNotInTime));
            Assert.That(result, Is.EqualTo(0));
        }
    }
}
