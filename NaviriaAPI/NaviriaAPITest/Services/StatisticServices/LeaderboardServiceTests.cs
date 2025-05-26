using Moq;
using NaviriaAPI.DTOs.FeaturesDTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.Entities.EmbeddedEntities;
using NaviriaAPI.Helpers;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Services.StatisticServices;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaviriaAPI.Tests.Services.StatisticServices
{
    [TestFixture]
    public class LeaderboardServiceTests
    {
        private Mock<IUserRepository> _userRepoMock;
        private Mock<ITaskRepository> _taskRepoMock;
        private LeaderboardService _leaderboardService;

        [SetUp]
        public void SetUp()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _taskRepoMock = new Mock<ITaskRepository>();
            _leaderboardService = new LeaderboardService(_userRepoMock.Object, _taskRepoMock.Object);
        }

        [Test]
        public async Task TC001_GetTopLeaderboardUsersAsync_ReturnsCorrectlySortedAndLimitedList()
        {
            // Arrange
            var users = new List<UserEntity>
            {
                new UserEntity
                {
                    Id = "1",
                    FullName = "Alice Smith",
                    Nickname = "alice",
                    LevelInfo = new LevelProgressInfo { Level = 5 },
                    Points = 100,
                    Achievements = new List<UserAchievementInfo> { new(), new() },
                    Photo = "alice_photo"
                },
                new UserEntity
                {
                    Id = "2",
                    FullName = "Bob Johnson",
                    Nickname = "bobby",
                    LevelInfo = new LevelProgressInfo { Level = 5 },
                    Points = 150,
                    Achievements = new List<UserAchievementInfo> { new() },
                    Photo = "bob_photo"
                },
                new UserEntity
                {
                    Id = "3",
                    FullName = "Charlie Brown",
                    Nickname = "charlie",
                    LevelInfo = new LevelProgressInfo { Level = 6 },
                    Points = 80,
                    Achievements = new List<UserAchievementInfo>(),
                    Photo = "charlie_photo"
                }
            };

            var tasks = new List<TaskEntity>
            {
                new TaskEntity { UserId = "1", Status = CurrentTaskStatus.Completed },
                new TaskEntity { UserId = "1", Status = CurrentTaskStatus.InProgress },
                new TaskEntity { UserId = "2", Status = CurrentTaskStatus.CompletedInTime },
                new TaskEntity { UserId = "2", Status = CurrentTaskStatus.Completed },
                new TaskEntity { UserId = "3", Status = CurrentTaskStatus.CompletedNotInTime }
            };

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
            _taskRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);

            // Act
            var result = await _leaderboardService.GetTopLeaderboardUsersAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(3));

            // Sorted by: Level DESC, Points DESC, etc.
            Assert.That(result[0].UserId, Is.EqualTo("3")); // Highest level
            Assert.That(result[1].UserId, Is.EqualTo("2")); // Same level as 1, more points
            Assert.That(result[2].UserId, Is.EqualTo("1"));

            // Check calculated fields
            var user1 = result.First(r => r.UserId == "1");
            Assert.That(user1.CompletionRate, Is.EqualTo(0.5));
            Assert.That(user1.AchievementsCount, Is.EqualTo(2));

            var user2 = result.First(r => r.UserId == "2");
            Assert.That(user2.CompletionRate, Is.EqualTo(1.0));
            Assert.That(user2.AchievementsCount, Is.EqualTo(1));

            var user3 = result.First(r => r.UserId == "3");
            Assert.That(user3.CompletionRate, Is.EqualTo(1.0));
            Assert.That(user3.AchievementsCount, Is.EqualTo(0));
        }

        [Test]
        public async Task TC002_GetTopLeaderboardUsersAsync_ReturnsMax10Users()
        {
            // Arrange
            var users = Enumerable.Range(1, 15).Select(i => new UserEntity
            {
                Id = i.ToString(),
                FullName = $"User {i}",
                Nickname = $"user{i}",
                LevelInfo = new LevelProgressInfo { Level = i },
                Points = i * 10,
                Achievements = new List<UserAchievementInfo>(),
                Photo = $"photo{i}"
            }).ToList();

            var tasks = users.Select(u => new TaskEntity
            {
                UserId = u.Id,
                Status = CurrentTaskStatus.Completed
            }).ToList();

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
            _taskRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);

            // Act
            var result = await _leaderboardService.GetTopLeaderboardUsersAsync();

            // Assert
            Assert.That(result.Count, Is.EqualTo(10));
            Assert.That(result.Select(u => int.Parse(u.UserId)), Is.Ordered.Descending);
        }
    }
}