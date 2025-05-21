using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NaviriaAPI.DTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Services.User;
using NUnit.Framework;
using NaviriaAPI.IServices.IUserServices;
using NaviriaAPI.Entities.EmbeddedEntities;
using NaviriaAPI.IServices.ISecurityService;

namespace NaviriaAPI.Tests.Services.User
{
    [TestFixture]
    public class FriendServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<IMessageSecurityService> _messageSecurityServiceMock;
        private FriendService _friendService;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userServiceMock = new Mock<IUserService>();
            _messageSecurityServiceMock = new Mock<IMessageSecurityService>();
            _friendService = new FriendService(
                _userRepositoryMock.Object,
                _userServiceMock.Object,
                _messageSecurityServiceMock.Object
            );
        }

        [Test]
        public async Task TC01_GetUserFriendsAsync_ShouldReturnFriendDtos()
        {
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

            var result = (await _friendService.GetUserFriendsAsync(userId)).ToList();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(f => f.Id == "f1"));
            Assert.That(result.Any(f => f.Id == "f2"));
        }

        [Test]
        public async Task TC02_DeleteFriendAsync_ShouldRemoveBothUsersAndReturnTrue()
        {
            var userId = "user1";
            var friendId = "user2";

            var user = new UserEntity { Id = userId, Friends = new List<UserFriendInfo> { new UserFriendInfo { UserId = friendId } } };
            var friend = new UserEntity { Id = friendId, Friends = new List<UserFriendInfo> { new UserFriendInfo { UserId = userId } } };

            _userServiceMock.Setup(s => s.GetUserOrThrowAsync(userId)).ReturnsAsync(user);
            _userServiceMock.Setup(s => s.GetUserOrThrowAsync(friendId)).ReturnsAsync(friend);

            _userRepositoryMock.Setup(r => r.UpdateAsync(It.Is<UserEntity>(u => u.Friends.Count == 0))).ReturnsAsync(true);

            var result = await _friendService.DeleteFriendAsync(userId, friendId);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task TC03_DeleteFriendAsync_ShouldReturnFalse_IfUpdateFails()
        {
            var userId = "user1";
            var friendId = "user2";

            var user = new UserEntity { Id = userId, Friends = new List<UserFriendInfo> { new UserFriendInfo { UserId = friendId } } };
            var friend = new UserEntity { Id = friendId, Friends = new List<UserFriendInfo> { new UserFriendInfo { UserId = userId } } };

            _userServiceMock.Setup(s => s.GetUserOrThrowAsync(userId)).ReturnsAsync(user);
            _userServiceMock.Setup(s => s.GetUserOrThrowAsync(friendId)).ReturnsAsync(friend);

            _userRepositoryMock.Setup(r => r.UpdateAsync(user)).ReturnsAsync(false);
            _userRepositoryMock.Setup(r => r.UpdateAsync(friend)).ReturnsAsync(true);

            var result = await _friendService.DeleteFriendAsync(userId, friendId);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task TC04_GetPotentialFriendsAsync_ShouldReturnOnlyNonFriends()
        {
            var userId = "user1";
            var user = new UserEntity
            {
                Id = userId,
                Friends = new List<UserFriendInfo>
                {
                    new UserFriendInfo { UserId = "f1" },
                    new UserFriendInfo { UserId = "f2" }
                }
            };

            var allUsers = new List<UserEntity>
            {
                user,
                new UserEntity { Id = "f1" },
                new UserEntity { Id = "f2" },
                new UserEntity { Id = "p1" },
                new UserEntity { Id = "p2" }
            };

            _userServiceMock.Setup(s => s.GetUserOrThrowAsync(userId)).ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(allUsers);

            var result = (await _friendService.GetPotentialFriendsAsync(userId)).ToList();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.All(r => r.Id == "p1" || r.Id == "p2"));
        }

        [Test]
        public async Task TC05_SearchUsersByNicknameAsync_ShouldReturnMatchingNonFriends()
        {
            var userId = "user1";
            var query = "Ali";

            var user = new UserEntity
            {
                Id = userId,
                Friends = new List<UserFriendInfo> { new UserFriendInfo { UserId = "friend1" } }
            };

            var allUsers = new List<UserEntity>
            {
                user,
                new UserEntity { Id = "friend1", Nickname = "Alice" },
                new UserEntity { Id = "other1", Nickname = "Alina" },
                new UserEntity { Id = "other2", Nickname = "Bob" }
            };

            _userServiceMock.Setup(s => s.GetUserOrThrowAsync(userId)).ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(allUsers);
            _messageSecurityServiceMock.Setup(v => v.Validate(userId, query));

            var result = (await _friendService.SearchUsersByNicknameAsync(userId, query)).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Nickname.Contains("Ali"));
        }

        //[Test]
        //public async Task TC06_SearchFriendsByNicknameAsync_ShouldReturnMatchingFriends()
        //{
        //    var userId = "user1";
        //    var query = "Ann";

        //    var user = new UserEntity
        //    {
        //        Id = userId,
        //        Friends = new List<UserFriendInfo> { new UserFriendInfo { UserId = "f1" } }
        //    };

        //    var friends = new List<UserEntity>
        //    {
        //        new UserEntity { Id = "f1", Nickname = "Anna" },
        //        new UserEntity { Id = "f2", Nickname = "Mark" }
        //    };

        //    _userServiceMock.Setup(s => s.GetUserOrThrowAsync(userId)).ReturnsAsync(user);
        //    _userRepositoryMock.Setup(r => r.GetManyByIdsAsync(It.Is<List<string>>(ids => ids.Contains("f1"))))
        //        .ReturnsAsync(friends);
        //    _messageSecurityServiceMock.Setup(v => v.Validate(userId, query));

        //    var result = (await _friendService.SearchFriendsByNicknameAsync(userId, query)).ToList();

        //    Assert.That(result.Count, Is.EqualTo(1));
        //    Assert.That(result.First().Nickname, Is.EqualTo("Anna"));
        //}

    //    [Test]
    //    public async Task TC07_SearchFriendsByNicknameAsync_ShouldReturnEmpty_IfNoFriends()
    //    {
    //        var userId = "user1";
    //        var query = "test";

    //        var user = new UserEntity
    //        {
    //            Id = userId,
    //            Friends = new List<UserFriendInfo>()
    //        };

    //        _userServiceMock.Setup(s => s.GetUserOrThrowAsync(userId)).ReturnsAsync(user);
    //        _messageSecurityServiceMock.Setup(v => v.Validate(userId, query));

    //        var result = await _friendService.SearchFriendsByNicknameAsync(userId, query);

    //        Assert.That(result, Is.Empty);
    //    }
    }
}
