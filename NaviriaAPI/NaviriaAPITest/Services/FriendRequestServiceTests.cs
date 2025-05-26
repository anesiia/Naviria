using Moq;
using NaviriaAPI.DTOs.FriendRequest;
using NaviriaAPI.DTOs;
using NaviriaAPI.Entities.EmbeddedEntities;
using NaviriaAPI.Entities;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices.IUserServices;
using NaviriaAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CloudinaryDotNet.Actions;
using NaviriaAPI.IServices.IGamificationLogic;

namespace NaviriaAPI.Tests.Services
{
    [TestFixture]
    public class FriendRequestServiceTests
    {
        private Mock<IFriendRequestRepository> _friendRequestRepository;
        private Mock<IUserService> _userService;
        private Mock<ILogger<FriendRequestService>> _logger;
        private Mock<IUserRepository> _userRepository;
        private Mock<IAchievementManager> _achievementManager;
        private FriendRequestService _service;
        
        [SetUp]
        public void SetUp()
        {
            _friendRequestRepository = new Mock<IFriendRequestRepository>();
            _userService = new Mock<IUserService>();
            _logger = new Mock<ILogger<FriendRequestService>>();
            _userRepository = new Mock<IUserRepository>();
            _achievementManager = new Mock<IAchievementManager>();

            _service = new FriendRequestService(_friendRequestRepository.Object, _userService.Object, _logger.Object, _userRepository.Object, _achievementManager.Object);
        }

        [Test]
        public async Task TC001_CreateAsync_ShouldCreateRequest()
        {
            var dto = new FriendRequestCreateDto { FromUserId = "1", ToUserId = "2" };

            FriendRequestEntity capturedEntity = null;
            _friendRequestRepository.Setup(r => r.CreateAsync(It.IsAny<FriendRequestEntity>()))
                .Callback<FriendRequestEntity>(e => capturedEntity = e)
                .Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(capturedEntity.FromUserId, Is.EqualTo(dto.FromUserId));
            Assert.That(capturedEntity.ToUserId, Is.EqualTo(dto.ToUserId));
        }


        [Test]
        public void TC002_UpdateAsync_ShouldThrow_WhenRequestNotFound()
        {
            _friendRequestRepository.Setup(r => r.GetByIdAsync("invalid")).ReturnsAsync((FriendRequestEntity)null);

            var dto = new FriendRequestUpdateDto { Status = "accepted" };

            Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync("invalid", dto));
        }

        
        [Test]
        public async Task TC003_UpdateAsync_ShouldReturnTrue_WhenRequestUpdatedSuccessfully()
        {
            // Arrange
            var requestId = "123";
            var entity = new FriendRequestEntity
            {
                Id = requestId,
                Status = "new request",
                FromUserId = "1",
                ToUserId = "2"
            };

            var dto = new FriendRequestUpdateDto { Status = "accepted" };

            _friendRequestRepository.Setup(r => r.GetByIdAsync(requestId)).ReturnsAsync(entity);
            _friendRequestRepository.Setup(r => r.UpdateAsync(It.IsAny<FriendRequestEntity>())).ReturnsAsync(true);
            _friendRequestRepository.Setup(r => r.DeleteAsync(It.IsAny<string>())).ReturnsAsync(true);

            // Метод UpdateAsync викликає AddToFriendsAsync, то треба його змокати
            _userService.Setup(s => s.GetUserOrThrowAsync("1"))
                .ReturnsAsync(new UserEntity { Id = "1", Friends = new List<UserFriendInfo>() });
            _userService.Setup(s => s.GetUserOrThrowAsync("2"))
                .ReturnsAsync(new UserEntity { Id = "2", Friends = new List<UserFriendInfo>() });
            _userRepository.Setup(r => r.UpdateAsync(It.IsAny<UserEntity>())).ReturnsAsync(true);

            // Act
            var result = await _service.UpdateAsync(requestId, dto);

            // Assert
            Assert.That(result, Is.True);
        }


        [Test]
        public async Task TC004_UpdateAsync_ShouldReturnFalse_WhenExceptionOccurs()
        {
            var requestId = "123";  
            var entity = new FriendRequestEntity { Id = requestId, Status = "new request" };

            _friendRequestRepository.Setup(r => r.GetByIdAsync(requestId)).ReturnsAsync(entity);
            _friendRequestRepository.Setup(r => r.UpdateAsync(It.IsAny<FriendRequestEntity>())).ReturnsAsync(true);
            _friendRequestRepository.Setup(r => r.DeleteAsync(It.IsAny<string>())).ThrowsAsync(new Exception("Test exception"));


            var dto = new FriendRequestUpdateDto { Status = "rejected" };

            var result = await _service.UpdateAsync(requestId, dto);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task TC005_AddToFriendsAsync_ShouldAddUsersToFriendsList_WhenTheyAreNotFriends()
        {
            // Arrange
            var fromUser = new UserEntity
            {
                Id = "1",
                Nickname = "Alice",
                Friends = new List<UserFriendInfo>()
            };

            var toUser = new UserEntity
            {
                Id = "2",
                Nickname = "Bob",
                Friends = new List<UserFriendInfo>()
            };

            _userService.Setup(x => x.GetUserOrThrowAsync("1")).ReturnsAsync(fromUser);
            _userService.Setup(x => x.GetUserOrThrowAsync("2")).ReturnsAsync(toUser);

            _userRepository.Setup(x => x.UpdateAsync(It.IsAny<UserEntity>())).ReturnsAsync(true);

            // Act
            var result = await _service.AddToFriendsAsync("1", "2");

            // Assert
            Assert.That(result, Is.True);
            Assert.That(fromUser.Friends.Any(f => f.UserId == "2"), Is.True);
            Assert.That(toUser.Friends.Any(f => f.UserId == "1"), Is.True);
        }

        [Test]
        public async Task TC006_AddToFriendsAsync_ShouldThrowAlreadyExistException_WhenUsersAreAlreadyFriends()
        {
            // Arrange
            var fromUser = new UserEntity
            {
                Id = "1",
                Nickname = "Alice",
                Friends = new List<UserFriendInfo>
        {
            new UserFriendInfo { UserId = "2", Nickname = "Bob" }
        }
            };

            var toUser = new UserEntity
            {
                Id = "2",
                Nickname = "Bob",
                Friends = new List<UserFriendInfo>
        {
            new UserFriendInfo { UserId = "1", Nickname = "Alice" }
        }
            };

            _userService.Setup(x => x.GetUserOrThrowAsync("1")).ReturnsAsync(fromUser);
            _userService.Setup(x => x.GetUserOrThrowAsync("2")).ReturnsAsync(toUser);

            // Act & Assert
            var ex = Assert.ThrowsAsync<AlreadyExistException>(async () =>
                await _service.AddToFriendsAsync("1", "2"));

            Assert.That(ex.Message, Is.EqualTo("Failed to add friends. These users are already friends"));
        }



        [Test]
        public async Task TC007_AddToFriendsAsync_ShouldReturnFalse_WhenRepositoryFailsToUpdate()
        {
            // Arrange
            var fromUser = new UserEntity
            {
                Id = "1",
                Nickname = "Alice",
                Friends = new List<UserFriendInfo>()
            };

            var toUser = new UserEntity
            {
                Id = "2",
                Nickname = "Bob",
                Friends = new List<UserFriendInfo>()
            };

            _userService.Setup(x => x.GetUserOrThrowAsync("1")).ReturnsAsync(fromUser);
            _userService.Setup(x => x.GetUserOrThrowAsync("2")).ReturnsAsync(toUser);

            _userRepository.Setup(x => x.UpdateAsync(It.IsAny<UserEntity>())).ReturnsAsync(false);

            // Act
            var result = await _service.AddToFriendsAsync("1", "2");

            // Assert
            Assert.That(result, Is.False);
        }


        [Test]
        public async Task TC008_GetByIdAsync_ShouldReturnRequest_WhenRequestExists()
        {
            var requestId = "123";
            var entity = new FriendRequestEntity { Id = requestId, Status = "new request" };

            _friendRequestRepository.Setup(r => r.GetByIdAsync(requestId)).ReturnsAsync(entity);

            var result = await _service.GetByIdAsync(requestId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(requestId));
        }

        [Test]
        public async Task TC009_GetByIdAsync_ShouldReturnNull_WhenRequestDoesNotExist()
        {
            var requestId = "invalid";
            _friendRequestRepository.Setup(r => r.GetByIdAsync(requestId)).ReturnsAsync((FriendRequestEntity)null);

            var result = await _service.GetByIdAsync(requestId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TC010_DeleteAsync_ShouldReturnTrue_WhenRequestDeletedSuccessfully()
        {
            var requestId = "123";
            _friendRequestRepository.Setup(r => r.DeleteAsync(requestId)).ReturnsAsync(true);

            var result = await _service.DeleteAsync(requestId);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task TC011_DeleteAsync_ShouldReturnFalse_WhenDeletionFails()
        {
            var requestId = "123";
            _friendRequestRepository.Setup(r => r.DeleteAsync(requestId)).ReturnsAsync(false);

            var result = await _service.DeleteAsync(requestId);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task TC012_GetAllAsync_ShouldReturnAllRequests()
        {
            var requests = new List<FriendRequestEntity>
        {
            new FriendRequestEntity { Id = "1", Status = "new request" },
            new FriendRequestEntity { Id = "2", Status = "accepted" }
        };

            _friendRequestRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(requests);

            var result = await _service.GetAllAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task TC013_GetIncomingRequestsAsync_ShouldReturnSenders()
        {
            var requests = new List<FriendRequestEntity>
            {
                new() { FromUserId = "1", ToUserId = "2" },
                new() { FromUserId = "3", ToUserId = "2" }
            };

            var senders = new List<UserEntity>
            {
                new() { Id = "1", Nickname = "Alice" },
                new() { Id = "3", Nickname = "Bob" }
            };

            _friendRequestRepository.Setup(r => r.GetByReceiverIdAsync("2")).ReturnsAsync(requests);
            _userRepository.Setup(r => r.GetManyByIdsAsync(It.IsAny<List<string>>())).ReturnsAsync(senders);

            var result = await _service.GetIncomingRequestsAsync("2");

            Assert.That(result.Count(), Is.EqualTo(2));
            //Assert.That(result.Any(u => u.Nickname == "Alice"), Is.True);
            //Assert.That(result.Any(u => u.Nickname == "Bob"), Is.True);
        }

        [Test]
        public async Task TC014_GetAllAsync_ShouldReturnListOfDtos()
        {
            var entities = new List<FriendRequestEntity>
            {
                new() { Id = "1", FromUserId = "1", ToUserId = "2" },
                new() { Id = "2", FromUserId = "3", ToUserId = "4" }
            };

            _friendRequestRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);

            var result = await _service.GetAllAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task TC015_UpdateAsync_ShouldOnlyUpdate_WhenStatusIsNotAcceptedOrRejected()
        {
            var requestId = "234";
            var entity = new FriendRequestEntity
            {
                Id = requestId,
                Status = "new request",
                FromUserId = "user1",
                ToUserId = "user2"
            };

            var dto = new FriendRequestUpdateDto { Status = "ignored" };

            _friendRequestRepository.Setup(r => r.GetByIdAsync(requestId)).ReturnsAsync(entity);
            _friendRequestRepository.Setup(r => r.UpdateAsync(It.IsAny<FriendRequestEntity>())).ReturnsAsync(true);

            var result = await _service.UpdateAsync(requestId, dto);

            Assert.That(result, Is.True);
            _friendRequestRepository.Verify(r => r.DeleteAsync(It.IsAny<string>()), Times.Never);
            _userRepository.Verify(r => r.UpdateAsync(It.IsAny<UserEntity>()), Times.Never);
        }

        [Test]
        public async Task TC016_UpdateAsync_ShouldUpdateAndAddFriends_WhenStatusIsAccepted()
        {
            // Arrange
            var requestId = "req123";
            var fromUserId = "user1";
            var toUserId = "user2";

            var friendRequest = new FriendRequestEntity
            {
                Id = requestId,
                Status = "new request",
                FromUserId = fromUserId,
                ToUserId = toUserId
            };

            var dto = new FriendRequestUpdateDto { Status = "accepted" };

            var fromUser = new UserEntity
            {
                Id = fromUserId,
                Friends = new List<UserFriendInfo>()
            };

            var toUser = new UserEntity
            {
                Id = toUserId,
                Friends = new List<UserFriendInfo>()
            };

            _friendRequestRepository.Setup(r => r.GetByIdAsync(requestId)).ReturnsAsync(friendRequest);
            _friendRequestRepository.Setup(r => r.UpdateAsync(It.IsAny<FriendRequestEntity>())).ReturnsAsync(true);
            _friendRequestRepository.Setup(r => r.DeleteAsync(requestId)).ReturnsAsync(true);

            _userService.Setup(s => s.GetUserOrThrowAsync(fromUserId)).ReturnsAsync(fromUser);
            _userService.Setup(s => s.GetUserOrThrowAsync(toUserId)).ReturnsAsync(toUser);
            _userRepository.Setup(r => r.UpdateAsync(It.IsAny<UserEntity>())).ReturnsAsync(true);

            // Act
            var result = await _service.UpdateAsync(requestId, dto);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(friendRequest.Status, Is.EqualTo("accepted"));

            _friendRequestRepository.Verify(r => r.UpdateAsync(It.Is<FriendRequestEntity>(f => f.Status == "accepted")), Times.Once);
            _friendRequestRepository.Verify(r => r.DeleteAsync(requestId), Times.Once);
            _userRepository.Verify(r => r.UpdateAsync(It.Is<UserEntity>(u => u.Id == fromUserId)), Times.Once);
            _userRepository.Verify(r => r.UpdateAsync(It.Is<UserEntity>(u => u.Id == toUserId)), Times.Once);
        }

        [Test]
        public void TC017_CreateAsync_ShouldThrow_WhenFromAndToUserAreSame()
        {
            var dto = new FriendRequestCreateDto { FromUserId = "1", ToUserId = "1" };

            var ex = Assert.ThrowsAsync<FailedToCreateException>(() => _service.CreateAsync(dto));
            Assert.That(ex.Message, Is.EqualTo("Failed to create friend request. You can`t be a friend to yourself."));
        }

        [Test]
        public void TC018_AddToFriendsAsync_ShouldThrow_WhenUsersAreAlreadyFriends()
        {
            var fromUser = new UserEntity
            {
                Id = "1",
                Nickname = "Alice",
                Friends = new List<UserFriendInfo> { new UserFriendInfo { UserId = "2" } }
            };
            var toUser = new UserEntity
            {
                Id = "2",
                Nickname = "Bob",
                Friends = new List<UserFriendInfo> { new UserFriendInfo { UserId = "1" } }
            };

            _userService.Setup(x => x.GetUserOrThrowAsync("1")).ReturnsAsync(fromUser);
            _userService.Setup(x => x.GetUserOrThrowAsync("2")).ReturnsAsync(toUser);

            var ex = Assert.ThrowsAsync<AlreadyExistException>(() => _service.AddToFriendsAsync("1", "2"));
            Assert.That(ex.Message, Is.EqualTo("Failed to add friends. These users are already friends"));
        }

        [Test]
        public async Task TC019_GetByIdAsync_ShouldReturnDto_WhenFound()
        {
            var entity = new FriendRequestEntity { Id = "1", FromUserId = "1", ToUserId = "2" };
            _friendRequestRepository.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(entity);

            var result = await _service.GetByIdAsync("1");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.FromUserId, Is.EqualTo("1"));
        }

        [Test]
        public async Task TC020_GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            _friendRequestRepository.Setup(r => r.GetByIdAsync("unknown")).ReturnsAsync((FriendRequestEntity)null);

            var result = await _service.GetByIdAsync("unknown");

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TC021_DeleteAsync_ShouldCallRepositoryAndReturnResult()
        {
            _friendRequestRepository.Setup(r => r.DeleteAsync("1")).ReturnsAsync(true);

            var result = await _service.DeleteAsync("1");

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task TC022_GetIncomingRequestsAsync_ShouldReturnMappedDtosWithSenders()
        {
            var requests = new List<FriendRequestEntity>
    {
        new FriendRequestEntity { Id = "1", FromUserId = "1", ToUserId = "2" },
        new FriendRequestEntity { Id = "2", FromUserId = "3", ToUserId = "2" }
    };

            var senders = new List<UserEntity>
    {
        new UserEntity { Id = "1", Nickname = "Alice" },
        new UserEntity { Id = "3", Nickname = "Charlie" }
    };

            _friendRequestRepository.Setup(r => r.GetByReceiverIdAsync("2")).ReturnsAsync(requests);
            _userRepository.Setup(r => r.GetManyByIdsAsync(It.IsAny<List<string>>())).ReturnsAsync(senders);

            var result = await _service.GetIncomingRequestsAsync("2");

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.Any(r => r.Sender.Nickname == "Alice"), Is.True);
        }

        [Test]
        public async Task TC023_GetAllAsync_ShouldReturnMappedDtos()
        {
            var entities = new List<FriendRequestEntity>
    {
        new FriendRequestEntity { Id = "1", FromUserId = "1", ToUserId = "2" }
    };

            _friendRequestRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);

            var result = await _service.GetAllAsync();

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().FromUserId, Is.EqualTo("1"));
        }

    }
}
