using Moq;
using NUnit.Framework;
using NaviriaAPI.Services.StatisticServices;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Entities;
using NaviriaAPI.DTOs.FeaturesDTOs;
using NaviriaAPI.Exceptions;
using NaviriaAPI.Entities.EmbeddedEntities;

namespace NaviriaAPI.Tests.Services.StatisticServices
{
    [TestFixture]
    public class TaskStatisticByDateServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<ITaskRepository> _taskRepositoryMock;
        private TaskStatisticByDateService _service;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _service = new TaskStatisticByDateService(_userRepositoryMock.Object, _taskRepositoryMock.Object);
        }
        [Test]
        public async Task GetUserTasksCompletedPerMonthAsync_ReturnsAggregatedTasks()
        {
            // Arrange
            var userId = "user1";
            var tasks = new List<TaskEntity>
            {
                new TaskEntity { CompletedAt = new DateTime(2024, 1, 15) },
                new TaskEntity { CompletedAt = new DateTime(2024, 1, 20) },
                new TaskEntity { CompletedAt = new DateTime(2024, 2, 5) },
                new TaskEntity { CompletedAt = null } // should be ignored
            };

            _taskRepositoryMock.Setup(r => r.GetAllByUserAsync(userId)).ReturnsAsync(tasks);

            // Act
            var result = await _service.GetUserTasksCompletedPerMonthAsync(userId);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Year, Is.EqualTo(2024));
            Assert.That(result[0].Month, Is.EqualTo(1));
            Assert.That(result[0].CompletedCount, Is.EqualTo(2));

            Assert.That(result[1].Month, Is.EqualTo(2));
            Assert.That(result[1].CompletedCount, Is.EqualTo(1));
        }

        [Test]
        public async Task GetUserAndFriendsTasksCompletedPerMonthAsync_ReturnsCorrectStats()
        {
            // Arrange
            var userId = "user1";
            var friendId = "friend1";

            var user = new UserEntity
            {
                Id = userId,
                Friends = new List<UserFriendInfo>
        {
            new UserFriendInfo { UserId = friendId }
        }
            };

            var tasks = new List<TaskEntity>
    {
        new TaskEntity { UserId = userId, CompletedAt = new DateTime(2024, 3, 10) },
        new TaskEntity { UserId = friendId, CompletedAt = new DateTime(2024, 3, 11) },
        new TaskEntity { UserId = userId, CompletedAt = null } // should be ignored
    };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            _taskRepositoryMock.Setup(r => r.GetByUserIdsAsync(It.Is<List<string>>(ids =>
                ids.Contains(userId) && ids.Contains(friendId))))
                .ReturnsAsync(tasks);

            // Act
            var result = await _service.GetUserAndFriendsTasksCompletedPerMonthAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Year, Is.EqualTo(2024));
            Assert.That(result[0].Month, Is.EqualTo(3));
            Assert.That(result[0].CompletedCount, Is.EqualTo(2));
        }


        [Test]
        public void GetUserAndFriendsTasksCompletedPerMonthAsync_ThrowsNotFoundException_WhenUserNotFound()
        {
            // Arrange
            var userId = "nonexistent";
            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((UserEntity)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() =>
                _service.GetUserAndFriendsTasksCompletedPerMonthAsync(userId));
            Assert.That(ex.Message, Is.EqualTo("User not found"));
        }

        [Test]
        public async Task GetGlobalTasksCompletedPerMonthAsync_ReturnsAggregatedTasks()
        {
            // Arrange
            var tasks = new List<TaskEntity>
            {
                new TaskEntity { CompletedAt = new DateTime(2024, 4, 1) },
                new TaskEntity { CompletedAt = new DateTime(2024, 4, 15) },
                new TaskEntity { CompletedAt = new DateTime(2024, 5, 5) }
            };

            _taskRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);

            // Act
            var result = await _service.GetGlobalTasksCompletedPerMonthAsync();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Month, Is.EqualTo(4));
            Assert.That(result[0].CompletedCount, Is.EqualTo(2));
            Assert.That(result[1].Month, Is.EqualTo(5));
            Assert.That(result[1].CompletedCount, Is.EqualTo(1));
        }
    }
}

