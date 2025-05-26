using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NaviriaAPI.Entities;
using NaviriaAPI.Entities.EmbeddedEntities.Subtasks;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Services.StatisticServices;
using NUnit.Framework;

namespace NaviriaAPI.Tests.Services.StatisticServices
{
    [TestFixture]
    public class RepeatableTaskStatisticServiceTests
    {
        private Mock<ITaskRepository> _taskRepositoryMock;
        private RepeatableTaskStatisticService _service;

        [SetUp]
        public void SetUp()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _service = new RepeatableTaskStatisticService(_taskRepositoryMock.Object);
        }

        [Test]
        public async Task TC001_GetTotalCheckedInDaysCountForUserAsync_ShouldReturnCorrectSum()
        {
            // Arrange
            string userId = "user123";
            var tasks = new List<TaskEntity>
            {
                new TaskEntity
                {
                    Subtasks = new List<SubtaskBase>
                    {
                        new SubtaskRepeatable
                        {
                            Id = "sub1",
                            CheckedInDays = new List<DateTime> { DateTime.Now, DateTime.Now.AddDays(-1) }
                        },
                        new SubtaskRepeatable
                        {
                            Id = "sub2",
                            CheckedInDays = new List<DateTime> { DateTime.Now }
                        },
                        new SubtaskStandard { Id = "not_included" }
                    }
                },
                new TaskEntity
                {
                    Subtasks = new List<SubtaskBase>
                    {
                        new SubtaskRepeatable
                        {
                            Id = "sub3",
                            CheckedInDays = null
                        }
                    }
                }
            };

            _taskRepositoryMock.Setup(r => r.GetAllByUserAsync(userId))
                .ReturnsAsync(tasks);

            // Act
            var result = await _service.GetTotalCheckedInDaysCountForUserAsync(userId);

            // Assert
            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public async Task TC002_GetCheckedInDaysCountForSubtaskAsync_ShouldReturnCorrectCount_WhenSubtaskExists()
        {
            // Arrange
            string userId = "user123";
            string subtaskId = "target_sub";
            var tasks = new List<TaskEntity>
            {
                new TaskEntity
                {
                    Subtasks = new List<SubtaskBase>
                    {
                        new SubtaskRepeatable
                        {
                            Id = "target_sub",
                            CheckedInDays = new List<DateTime>
                            {
                                DateTime.Now,
                                DateTime.Now.AddDays(-2),
                                DateTime.Now.AddDays(-5)
                            }
                        }
                    }
                }
            };

            _taskRepositoryMock.Setup(r => r.GetAllByUserAsync(userId))
                .ReturnsAsync(tasks);

            // Act
            var result = await _service.GetCheckedInDaysCountForSubtaskAsync(userId, subtaskId);

            // Assert
            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public async Task TC003_GetCheckedInDaysCountForSubtaskAsync_ShouldReturnZero_WhenSubtaskNotFound()
        {
            // Arrange
            string userId = "user123";
            string subtaskId = "nonexistent_sub";
            var tasks = new List<TaskEntity>
            {
                new TaskEntity
                {
                    Subtasks = new List<SubtaskBase>
                    {
                        new SubtaskRepeatable
                        {
                            Id = "other_sub",
                            CheckedInDays = new List<DateTime> { DateTime.Now }
                        }
                    }
                }
            };

            _taskRepositoryMock.Setup(r => r.GetAllByUserAsync(userId))
                .ReturnsAsync(tasks);

            // Act
            var result = await _service.GetCheckedInDaysCountForSubtaskAsync(userId, subtaskId);

            // Assert
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public async Task TC004_GetCheckedInDaysCountForSubtaskAsync_ShouldReturnZero_WhenCheckedInDaysIsNull()
        {
            // Arrange
            string userId = "user123";
            string subtaskId = "sub_with_null_days";
            var tasks = new List<TaskEntity>
            {
                new TaskEntity
                {
                    Subtasks = new List<SubtaskBase>
                    {
                        new SubtaskRepeatable
                        {
                            Id = "sub_with_null_days",
                            CheckedInDays = null
                        }
                    }
                }
            };

            _taskRepositoryMock.Setup(r => r.GetAllByUserAsync(userId))
                .ReturnsAsync(tasks);

            // Act
            var result = await _service.GetCheckedInDaysCountForSubtaskAsync(userId, subtaskId);

            // Assert
            Assert.That(result, Is.EqualTo(0));
        }
    }
}