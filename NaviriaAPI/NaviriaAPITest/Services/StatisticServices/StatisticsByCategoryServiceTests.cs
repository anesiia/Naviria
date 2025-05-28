using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NaviriaAPI.DTOs.FeaturesDTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.Entities.EmbeddedEntities;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Services.StatisticServices;
using NUnit.Framework;

namespace NaviriaAPI.Tests.Services.StatisticServices
{
    [TestFixture]
    public class StatisticsByCategoryServiceTests
    {
        private Mock<IStatisticRepository> _statisticRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        private Mock<ITaskRepository> _taskRepoMock;
        private Mock<ICategoryRepository> _categoryRepoMock;
        private StatisticsByCategoryService _service;

        [SetUp]
        public void SetUp()
        {
            _statisticRepoMock = new Mock<IStatisticRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _taskRepoMock = new Mock<ITaskRepository>();
            _categoryRepoMock = new Mock<ICategoryRepository>();

            _service = new StatisticsByCategoryService(
                _statisticRepoMock.Object,
                _userRepoMock.Object,
                _taskRepoMock.Object,
                _categoryRepoMock.Object);
        }

        [Test]
        public async Task TC001_GetUserPieChartStatsAsync_ReturnsCorrectStats()
        {
            var userId = "user123";
            var tasks = new List<TaskEntity>
            {
                new TaskEntity { CategoryId = "cat1" },
                new TaskEntity { CategoryId = "cat1" },
                new TaskEntity { CategoryId = "cat2" }
            };

            var categories = new List<CategoryEntity>
            {
                new CategoryEntity { Id = "cat1", Name = "Work" },
                new CategoryEntity { Id = "cat2", Name = "Study" }
            };

            _taskRepoMock.Setup(r => r.GetAllByUserAsync(userId)).ReturnsAsync(tasks);
            _categoryRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

            var result = await _service.GetUserPieChartStatsAsync(userId);

            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0].CategoryId, Is.EqualTo("cat1"));
            Assert.That(result[0].Value, Is.EqualTo(67).Within(0.01));
            Assert.That(result[1].CategoryName, Is.EqualTo("Study"));
        }

        [Test]
        public async Task TC002_GetUserAndFriendsPieChartStatsAsync_ReturnsStatsForUserAndFriends()
        {
            // Arrange
            var userId = "me";
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
                new TaskEntity { CategoryId = "cat1" },
                new TaskEntity { CategoryId = "cat2" },
                new TaskEntity { CategoryId = "cat1" } 
            };

            var categories = new List<CategoryEntity>
            {
                new CategoryEntity { Id = "cat1", Name = "Gym" },
                new CategoryEntity { Id = "cat2", Name = "Chill" }
            };

            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            _taskRepoMock.Setup(r => r.GetByUserIdsAsync(It.Is<List<string>>(l => l.Contains(userId) && l.Contains(friendId))))
                .ReturnsAsync(tasks);
            _categoryRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

            // Act
            var result = await _service.GetUserAndFriendsPieChartStatsAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));

            var gymStat = result.FirstOrDefault(x => x.CategoryName == "Gym");
            var chillStat = result.FirstOrDefault(x => x.CategoryName == "Chill");

            Assert.That(gymStat, Is.Not.Null);

            Assert.That(chillStat, Is.Not.Null);
        }

        [Test]
        public void TC003_GetUserAndFriendsPieChartStatsAsync_ThrowsNotFoundException_WhenUserMissing()
        {
            var userId = "ghost";
            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((UserEntity?)null);

            var ex = Assert.ThrowsAsync<NotFoundException>(() => _service.GetUserAndFriendsPieChartStatsAsync(userId));

            Assert.That(ex, Is.Not.Null);
            Assert.That(ex!.Message, Is.EqualTo("User not found"));
        }

        [Test]
        public async Task TC004_GetGlobalPieChartStatsAsync_ReturnsStats()
        {
            var tasks = new List<TaskEntity>
            {
                new TaskEntity { CategoryId = "cat1" },
                new TaskEntity { CategoryId = "cat2" },
                new TaskEntity { CategoryId = "cat2" }
            };

            var categories = new List<CategoryEntity>
            {
                new CategoryEntity { Id = "cat1", Name = "Home" },
                new CategoryEntity { Id = "cat2", Name = "Work" }
            };

            _taskRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);
            _categoryRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

            var result = await _service.GetGlobalPieChartStatsAsync();

            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0].CategoryName, Is.EqualTo("Work"));
            Assert.That(result[0].Value, Is.EqualTo(67).Within(0.01));
        }

        [Test]
        public async Task TC005_GetUserPieChartStatsAsync_ReturnsEmptyList_WhenNoTasks()
        {
            var userId = "emptyUser";
            var tasks = new List<TaskEntity>();

            var categories = new List<CategoryEntity>
            {
                new CategoryEntity { Id = "cat1", Name = "Work" }
            };

            _taskRepoMock.Setup(r => r.GetAllByUserAsync(userId)).ReturnsAsync(tasks);
            _categoryRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

            var result = await _service.GetUserPieChartStatsAsync(userId);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task TC006_GetUserPieChartStatsAsync_UnknownCategory_ReturnsEmptyCategoryName()
        {
            var userId = "user1";
            var tasks = new List<TaskEntity>
    {
        new TaskEntity { CategoryId = "unknownCat" }
    };
            var categories = new List<CategoryEntity>(); 

            _taskRepoMock.Setup(r => r.GetAllByUserAsync(userId)).ReturnsAsync(tasks);
            _categoryRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

            var result = await _service.GetUserPieChartStatsAsync(userId);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].CategoryName, Is.EqualTo(""));
            Assert.That(result[0].CategoryId, Is.EqualTo("unknownCat"));
            Assert.That(result[0].Value, Is.EqualTo(100));
        }

        [Test]
        public async Task TC007_ResultsAreSortedByValueDescending()
        {
            var userId = "sortedUser";
            var tasks = new List<TaskEntity>
    {
        new TaskEntity { CategoryId = "c1" },
        new TaskEntity { CategoryId = "c2" },
        new TaskEntity { CategoryId = "c2" },
        new TaskEntity { CategoryId = "c3" },
        new TaskEntity { CategoryId = "c2" }
    };
            var categories = new List<CategoryEntity>
    {
        new CategoryEntity { Id = "c1", Name = "Reading" },
        new CategoryEntity { Id = "c2", Name = "Gaming" },
        new CategoryEntity { Id = "c3", Name = "Cooking" }
    };

            _taskRepoMock.Setup(r => r.GetAllByUserAsync(userId)).ReturnsAsync(tasks);
            _categoryRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

            var result = await _service.GetUserPieChartStatsAsync(userId);

            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result[0].CategoryName, Is.EqualTo("Gaming")); // має найбільше завдань
            Assert.That(result[1].Value, Is.LessThanOrEqualTo(result[0].Value));
            Assert.That(result[2].Value, Is.LessThanOrEqualTo(result[1].Value));
        }

    }
}