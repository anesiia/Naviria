using NaviriaAPI.Data;
using NaviriaAPI.Repositories;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Entities;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Microsoft.Extensions.Configuration;
using NaviriaAPI.Options;
using System.IO;

namespace NaviriaAPI.Tests.Repositories
{
    [TestFixture]
    public class FriendRequestRepositoryTests
    {
        private IFriendRequestRepository _friendRequestRepository;
        private IMongoCollection<FriendRequestEntity> _friendRequestCollection;
        private Mock<IOptions<MongoDbOptions>> _mockMongoDbOptions;
        private IMongoDbContext _dbContext;

        [SetUp]
        public void SetUp()
        {
            // Load configuration directly from MongoDbSettings.json file
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("MongoDbSettings.json")
                .Build();

            var mongoDbOptions = configuration.GetSection("MongoDbSettings").Get<MongoDbOptions>();

            var options = Microsoft.Extensions.Options.Options.Create(mongoDbOptions);

            _dbContext = new MongoDbContext(options);

            _friendRequestRepository = new FriendRequestRepository(_dbContext);
            _friendRequestCollection = _dbContext.FriendsRequests;

            // Clean up the collection before each test to ensure isolated tests
            _friendRequestCollection.DeleteMany(FilterDefinition<FriendRequestEntity>.Empty);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up the repository after each test
            _friendRequestRepository = null;
        }

        [Test]
        public async Task TC001_CreateAsync_ShouldAddFriendRequest()
        {
            // Arrange
            var fromUserId = ObjectId.GenerateNewId().ToString();
            var toUserId = ObjectId.GenerateNewId().ToString();

            var friendRequest = new FriendRequestEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                FromUserId = fromUserId,
                ToUserId = toUserId,
                Status = "Pending"
            };

            // Act
            await _friendRequestRepository.CreateAsync(friendRequest);

            // Assert
            var retrievedRequest = await _friendRequestRepository.GetByIdAsync(friendRequest.Id);
            Assert.That(retrievedRequest, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(retrievedRequest.FromUserId, Is.EqualTo(friendRequest.FromUserId));
                Assert.That(retrievedRequest.ToUserId, Is.EqualTo(friendRequest.ToUserId));
                Assert.That(retrievedRequest.Status, Is.EqualTo(friendRequest.Status));
            });
        }

        [Test]
        public async Task TC002_GetByIdAsync_ShouldReturnCorrectFriendRequest()
        {
            // Arrange
            var fromUserId = ObjectId.GenerateNewId().ToString();
            var toUserId = ObjectId.GenerateNewId().ToString();

            var friendRequest = new FriendRequestEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                FromUserId = fromUserId,
                ToUserId = toUserId,
                Status = "Pending"
            };

            await _friendRequestRepository.CreateAsync(friendRequest);

            // Act
            var retrievedRequest = await _friendRequestRepository.GetByIdAsync(friendRequest.Id);

            // Assert
            Assert.That(retrievedRequest, Is.Not.Null);
            Assert.That(retrievedRequest.Id, Is.EqualTo(friendRequest.Id));
        }

        [Test]
        public async Task TC003_UpdateAsync_ShouldModifyFriendRequest()
        {
            // Arrange
            var fromUserId = ObjectId.GenerateNewId().ToString();
            var toUserId = ObjectId.GenerateNewId().ToString();

            var friendRequest = new FriendRequestEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                FromUserId = fromUserId,
                ToUserId = toUserId,
                Status = "Pending"
            };

            await _friendRequestRepository.CreateAsync(friendRequest);

            // Modify the friend request
            friendRequest.Status = "Accepted";

            // Act
            var updateResult = await _friendRequestRepository.UpdateAsync(friendRequest);
            var updatedRequest = await _friendRequestRepository.GetByIdAsync(friendRequest.Id);

            // Assert
            Assert.That(updateResult, Is.True);
            Assert.That(updatedRequest.Status, Is.EqualTo("Accepted"));
        }

        [Test]
        public async Task TC004_DeleteAsync_ShouldRemoveFriendRequest()
        {
            // Arrange
            var fromUserId = ObjectId.GenerateNewId().ToString();
            var toUserId = ObjectId.GenerateNewId().ToString();

            var friendRequest = new FriendRequestEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                FromUserId = fromUserId,
                ToUserId = toUserId,
                Status = "Pending"
            };

            await _friendRequestRepository.CreateAsync(friendRequest);

            // Act
            var deleteResult = await _friendRequestRepository.DeleteAsync(friendRequest.Id);
            var deletedRequest = await _friendRequestRepository.GetByIdAsync(friendRequest.Id);

            // Assert
            Assert.That(deleteResult, Is.True);
            Assert.That(deletedRequest, Is.Null);
        }

        [Test]
        public async Task TC005_GetByReceiverIdAsync_ShouldReturnCorrectRequests()
        {
            // Arrange
            var fromUserId1 = ObjectId.GenerateNewId().ToString();
            var toUserId1 = ObjectId.GenerateNewId().ToString();
            var fromUserId2 = ObjectId.GenerateNewId().ToString();
            var toUserId2 = ObjectId.GenerateNewId().ToString();

            var friendRequest1 = new FriendRequestEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                FromUserId = fromUserId1,
                ToUserId = toUserId1,
                Status = "Pending"
            };
            var friendRequest2 = new FriendRequestEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                FromUserId = fromUserId2,
                ToUserId = toUserId2,
                Status = "Approved"
            };

            await _friendRequestRepository.CreateAsync(friendRequest1);
            await _friendRequestRepository.CreateAsync(friendRequest2);

            // Act
            var requests = await _friendRequestRepository.GetByReceiverIdAsync(toUserId1);

            // Assert
            Assert.That(requests, Has.All.Matches<FriendRequestEntity>(r => r.ToUserId == toUserId1));
        }

        [Test]
        public async Task TC006_GetAllAsync_ShouldReturnAllFriendRequests()
        {
            // Arrange
            var request1 = new FriendRequestEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                FromUserId = ObjectId.GenerateNewId().ToString(),
                ToUserId = ObjectId.GenerateNewId().ToString(),
                Status = "Pending"
            };
            var request2 = new FriendRequestEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                FromUserId = ObjectId.GenerateNewId().ToString(),
                ToUserId = ObjectId.GenerateNewId().ToString(),
                Status = "Approved"
            };

            await _friendRequestRepository.CreateAsync(request1);
            await _friendRequestRepository.CreateAsync(request2);

            // Act
            var allRequests = await _friendRequestRepository.GetAllAsync();

            // Assert
            Assert.That(allRequests.Count(), Is.EqualTo(2));
        }


        [Test]
        public async Task TC007_DeleteManyByUserIdAsync_ShouldRemoveAllRelatedRequests()
        {
            // Arrange
            var userId = ObjectId.GenerateNewId().ToString();

            var request1 = new FriendRequestEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                FromUserId = userId,
                ToUserId = ObjectId.GenerateNewId().ToString(),
                Status = "Pending"
            };
            var request2 = new FriendRequestEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                FromUserId = ObjectId.GenerateNewId().ToString(),
                ToUserId = userId,
                Status = "Pending"
            };
            var request3 = new FriendRequestEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                FromUserId = ObjectId.GenerateNewId().ToString(),
                ToUserId = ObjectId.GenerateNewId().ToString(),
                Status = "Pending"
            };

            await _friendRequestRepository.CreateAsync(request1);
            await _friendRequestRepository.CreateAsync(request2);
            await _friendRequestRepository.CreateAsync(request3);

            // Act
            await _friendRequestRepository.DeleteManyByUserIdAsync(userId);
            var allRequests = await _friendRequestRepository.GetAllAsync();

            // Assert
            Assert.That(allRequests, Has.Count.EqualTo(1));
            Assert.That(allRequests.First().Id, Is.EqualTo(request3.Id));
        }

    }
}