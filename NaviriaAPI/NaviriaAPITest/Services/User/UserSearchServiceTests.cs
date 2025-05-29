using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NaviriaAPI.DTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.Entities.EmbeddedEntities;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.IServices.ISecurityService;
using NaviriaAPI.IServices.IUserServices;
using NaviriaAPI.Services.User;
using NUnit.Framework;

namespace NaviriaAPI.Tests.Services.User
{

    public class UserSearchServiceTests
    {
        private Mock<ITaskRepository> _taskRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        private Mock<IFriendRequestRepository> _friendRequestRepoMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<IMessageSecurityService> _securityServiceMock;
        private UserSearchService _service;

        [SetUp]
        public void Setup()
        {
            _taskRepoMock = new Mock<ITaskRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _friendRequestRepoMock = new Mock<IFriendRequestRepository>();
            _userServiceMock = new Mock<IUserService>();
            _securityServiceMock = new Mock<IMessageSecurityService>();

            _service = new UserSearchService(
                _taskRepoMock.Object,
                _userRepoMock.Object,
                _friendRequestRepoMock.Object,
                _userServiceMock.Object,
                _securityServiceMock.Object);
        }

        [Test]
        public async Task TC001_SearchAllUsersAsync_ShouldReturnMatchingUsers_WhenQueryMatchesNicknameOrFullName()
        {
            // Arrange
            var currentUserId = "user1";
            var users = new List<UserEntity>
            {
                new UserEntity { Id = "user2", Nickname = "AnnaK", FullName = "Anna Karlsson" },
                new UserEntity { Id = "user3", Nickname = "johnny", FullName = "John Smith" },
                new UserEntity { Id = "user1", Nickname = "me", FullName = "Myself" } // should be excluded
            };

            _userRepoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);
            _userRepoMock.Setup(repo => repo.GetManyByIdsAsync(It.IsAny<IEnumerable<string>>()))
                         .ReturnsAsync((IEnumerable<string> ids) => users.Where(u => ids.Contains(u.Id)).ToList());

            var query = "Anna";

            // Act
            var result = await _service.SearchAllUsersAsync(currentUserId, null, query);

            // Assert
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].Nickname, Is.EqualTo("AnnaK"));
        }

        [Test]
        public async Task TC002_SearchFriendsAsync_ShouldReturnOnlyFriends_WhenCalled()
        {
            // Arrange
            var currentUserId = "me";
            var friendId = "friend1";
            var me = new UserEntity
            {
                Id = currentUserId,
                Friends = new List<UserFriendInfo> { new() { UserId = friendId } }
            };

            var users = new List<UserEntity>
            {
                me,
                new UserEntity { Id = friendId, Nickname = "BFF", FullName = "Best Friend" }
            };

            _userServiceMock.Setup(s => s.GetUserOrThrowAsync(currentUserId)).ReturnsAsync(me);
            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
            _userRepoMock.Setup(r => r.GetManyByIdsAsync(It.IsAny<IEnumerable<string>>()))
                         .ReturnsAsync((IEnumerable<string> ids) => users.Where(u => ids.Contains(u.Id)).ToList());

            // Act
            var result = await _service.SearchFriendsAsync(currentUserId, null, null);

            // Assert
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result.First().FullName, Is.EqualTo("Best Friend"));
        }

        [Test]
        public async Task TC003_SearchIncomingFriendRequestsAsync_ShouldReturnOnlyRequestSenders()
        {
            // Arrange
            var currentUserId = "me";
            var senderId = "user_sender";
            var friendRequests = new List<FriendRequestEntity>
            {
                new() { FromUserId = senderId, ToUserId = currentUserId, Status = "Pending" }
            };

            var users = new List<UserEntity>
            {
                new() { Id = senderId, Nickname = "Requester", FullName = "Friend Request Sender" }
            };

            _friendRequestRepoMock.Setup(r => r.GetByReceiverIdAsync(currentUserId)).ReturnsAsync(friendRequests);
            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
            _userRepoMock.Setup(r => r.GetManyByIdsAsync(It.IsAny<IEnumerable<string>>()))
                         .ReturnsAsync((IEnumerable<string> ids) => users.Where(u => ids.Contains(u.Id)).ToList());

            // Act
            var result = await _service.SearchIncomingFriendRequestsAsync(currentUserId, null, null);

            // Assert
            Assert.That(result, Has.Count.EqualTo(1));
            //Assert.That(result.First().Nickname, Is.EqualTo("Requester"));
        }

        [Test]
        public void TC004_SearchAllUsersAsync_ShouldThrowNotFoundException_WhenNoUsersFound()
        {
            // Arrange
            var currentUserId = "user1";
            var users = new List<UserEntity> { new() { Id = "user2", Nickname = "Nick", FullName = "Another" } };

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
            _userRepoMock.Setup(r => r.GetManyByIdsAsync(It.IsAny<IEnumerable<string>>())).ReturnsAsync(new List<UserEntity>());

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() =>
                _service.SearchAllUsersAsync(currentUserId, null, "doesnotexist"));

            Assert.That(ex.Message, Is.EqualTo("No users found."));
        }

        [Test]
        public async Task TC005_SearchAllUsersAsync_ShouldCallMessageSecurityValidation_WhenQueryProvided()
        {
            // Arrange
            var currentUserId = "user1";
            var query = "searchText";

            var users = new List<UserEntity>
            {
                new() { Id = "user2", Nickname = "searchText", FullName = "searchText" }
            };

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
            _userRepoMock.Setup(r => r.GetManyByIdsAsync(It.IsAny<IEnumerable<string>>()))
                         .ReturnsAsync(users);

            // Act
            await _service.SearchAllUsersAsync(currentUserId, null, query);

            // Assert
            _securityServiceMock.Verify(s => s.Validate(currentUserId, query), Times.Once);
        }
    }
}
