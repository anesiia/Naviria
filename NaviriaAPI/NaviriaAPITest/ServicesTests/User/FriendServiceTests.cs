using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NaviriaAPI.DTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.IServices;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Services.User;
using NUnit.Framework;
using NaviriaAPI.Entities.EmbeddedEntities;

namespace NaviriaAPITest.ServicesTests.User
{
    [TestFixture]
    public class FriendServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IUserService> _userServiceMock;
        private FriendService _friendService;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userServiceMock = new Mock<IUserService>();
            _friendService = new FriendService(_userRepositoryMock.Object, _userServiceMock.Object);
        }

        [Test]
        public async Task TC01_GetUserFriendsAsync_ShouldReturnFriendDtos()
        {
            // Arrange
            var userId = "user1";
            var friend1 = new UserEntity { Id = "f1", Email = "f1@test.com" };
            var friend2 = new UserEntity { Id = "f2", Email = "f2@test.com" };

            var user = new UserEntity
            {
                Id = userId,
                Friends = new List<UserFriendInfo>
                {
                    new UserFriendInfo { UserId = "f1" },
                    new UserFriendInfo { UserId = "f2" }
                }
            };

            _userServiceMock.Setup(s => s.GetUserOrThrowAsync(userId)).ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.GetManyByIdsAsync(It.Is<List<string>>(ids => ids.Contains("f1") && ids.Contains("f2"))))
                .ReturnsAsync(new List<UserEntity> { friend1, friend2 });

            // Act
            var result = (await _friendService.GetUserFriendsAsync(userId)).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(f => f.Id == "f1" && f.Email == "f1@test.com"), Is.True);
            Assert.That(result.Any(f => f.Id == "f2" && f.Email == "f2@test.com"), Is.True);
        }

        [Test]
        public async Task TC02_DeleteFriendAsync_ShouldRemoveBothUsersAndReturnTrue()
        {
            // Arrange
            var userId = "user1";
            var friendId = "user2";

            var user = new UserEntity
            {
                Id = userId,
                Friends = new List<UserFriendInfo> { new UserFriendInfo { UserId = friendId } }
            };

            var friend = new UserEntity
            {
                Id = friendId,
                Friends = new List<UserFriendInfo> { new UserFriendInfo { UserId = userId } }
            };

            _userServiceMock.Setup(s => s.GetUserOrThrowAsync(userId)).ReturnsAsync(user);
            _userServiceMock.Setup(s => s.GetUserOrThrowAsync(friendId)).ReturnsAsync(friend);

            _userRepositoryMock.Setup(r => r.UpdateAsync(It.Is<UserEntity>(u => u.Id == userId && u.Friends.Count == 0)))
                .ReturnsAsync(true);
            _userRepositoryMock.Setup(r => r.UpdateAsync(It.Is<UserEntity>(u => u.Id == friendId && u.Friends.Count == 0)))
                .ReturnsAsync(true);

            // Act
            var result = await _friendService.DeleteFriendAsync(userId, friendId);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(user.Friends.Any(f => f.UserId == friendId), Is.False);
            Assert.That(friend.Friends.Any(f => f.UserId == userId), Is.False);
        }

        [Test]
        public async Task TC03_DeleteFriendAsync_ShouldReturnFalse_IfUpdateFails()
        {
            // Arrange
            var userId = "user1";
            var friendId = "user2";

            var user = new UserEntity
            {
                Id = userId,
                Friends = new List<UserFriendInfo> { new UserFriendInfo { UserId = friendId } }
            };

            var friend = new UserEntity
            {
                Id = friendId,
                Friends = new List<UserFriendInfo> { new UserFriendInfo { UserId = userId } }
            };

            _userServiceMock.Setup(s => s.GetUserOrThrowAsync(userId)).ReturnsAsync(user);
            _userServiceMock.Setup(s => s.GetUserOrThrowAsync(friendId)).ReturnsAsync(friend);

            _userRepositoryMock.Setup(r => r.UpdateAsync(user)).ReturnsAsync(false);
            _userRepositoryMock.Setup(r => r.UpdateAsync(friend)).ReturnsAsync(true);

            // Act
            var result = await _friendService.DeleteFriendAsync(userId, friendId);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}