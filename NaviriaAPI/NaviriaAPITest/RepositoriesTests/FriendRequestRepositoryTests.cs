using MongoDB.Driver;
using NaviriaAPI.Data;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Repositories;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using MongoDB.Bson;

namespace NaviriaAPITest.RepositoriesTests
{
    [TestFixture]
    public class FriendRequestRepositoryTests
    {
        private IMongoDbContext _dbContext;
        private IFriendRequestRepository _friendRequestRepository;
        private IMongoCollection<FriendRequestEntity> _friendRequestCollection;

        [SetUp]
        public void SetUp()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("MongoDbSettings.json")
                .Build();

            var mongoDbSettings = configuration.Get<MongoDbSettings>();

            var mockOptions = new Mock<IOptions<MongoDbSettings>>();
            mockOptions.Setup(o => o.Value).Returns(mongoDbSettings);

            _dbContext = new MongoDbContext(mockOptions.Object);
            _friendRequestRepository = new FriendRequestRepository(_dbContext);
            _friendRequestCollection = _dbContext.FriendsRequests;
            _friendRequestCollection.DeleteMany(Builders<FriendRequestEntity>.Filter.Empty);
        }

        //[Test]
        //public async Task TC001_CreateAsync_And_GetByIdAsync_ShouldWorkCorrectly()
        //{
        //    var friendRequest = new FriendRequestEntity
        //    {
        //        Id = ObjectId.GenerateNewId().ToString(),  // Generate a valid ObjectId
        //        FromUserId = "user1",  // Keep this as a string
        //        ToUserId = "user2",    // Keep this as a string
        //        Status = "pending"
        //    };

        //    await _friendRequestRepository.CreateAsync(friendRequest);
        //    var result = await _friendRequestRepository.GetByIdAsync(friendRequest.Id);

        //    Assert.That(result, Is.Not.Null);
        //    Assert.That(result!.FromUserId, Is.EqualTo(friendRequest.FromUserId));
        //}



        //[Test]
        //public async Task TC002_GetAllAsync_ShouldReturnAllRequests()
        //{
        //    var friendRequest1 = new FriendRequestEntity
        //    {
        //        Id = "someRandomStringId1",
        //        FromUserId = "user1",
        //        ToUserId = "user2",
        //        Status = "pending"
        //    };

        //    var friendRequest2 = new FriendRequestEntity
        //    {
        //        Id = ObjectId.GenerateNewId().ToString(),
        //        FromUserId = "user3",
        //        ToUserId = "user4",
        //        Status = "accepted"
        //    };

        //    await _friendRequestRepository.CreateAsync(friendRequest1);
        //    await _friendRequestRepository.CreateAsync(friendRequest2);

        //    var all = await _friendRequestRepository.GetAllAsync();

        //    Assert.That(all.Count, Is.GreaterThanOrEqualTo(2));
        //}

        //[Test]
        //public async Task TC003_UpdateAsync_ShouldModifyDocument()
        //{
        //    var friendRequest = new FriendRequestEntity
        //    {
        //        Id = ObjectId.GenerateNewId().ToString(),
        //        FromUserId = "user1",
        //        ToUserId = "user2",
        //        Status = "pending"
        //    };

        //    await _friendRequestRepository.CreateAsync(friendRequest);

        //    friendRequest.Status = "accepted";
        //    var updated = await _friendRequestRepository.UpdateAsync(friendRequest);
        //    var result = await _friendRequestRepository.GetByIdAsync(friendRequest.Id);

        //    Assert.That(updated, Is.True);
        //    Assert.That(result!.Status, Is.EqualTo("accepted"));
        //}

        //[Test]
        //public async Task TC004_DeleteAsync_ShouldRemoveDocument()
        //{
        //    var friendRequest = new FriendRequestEntity
        //    {
        //        Id = ObjectId.GenerateNewId().ToString(),
        //        FromUserId = "userX",
        //        ToUserId = "userY",
        //        Status = "pending"
        //    };

        //    await _friendRequestRepository.CreateAsync(friendRequest);

        //    var deleted = await _friendRequestRepository.DeleteAsync(friendRequest.Id);
        //    var result = await _friendRequestRepository.GetByIdAsync(friendRequest.Id);

        //    Assert.That(deleted, Is.True);
        //    Assert.That(result, Is.Null);
        //}

        //[Test]
        //public async Task TC005_GetByReceiverIdAsync_ShouldReturnCorrectResults()
        //{
        //    var receiverId = "receiver123";
        //    var request1 = new FriendRequestEntity
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        FromUserId = "sender1",
        //        ToUserId = receiverId,
        //        Status = "pending"
        //    };

        //    var request2 = new FriendRequestEntity
        //    {
        //        Id = ObjectId.GenerateNewId().ToString(),
        //        FromUserId = "sender2",
        //        ToUserId = receiverId,
        //        Status = "pending"
        //    };

        //    await _friendRequestRepository.CreateAsync(request1);
        //    await _friendRequestRepository.CreateAsync(request2);

        //    var results = await _friendRequestRepository.GetByReceiverIdAsync(receiverId);

        //    Assert.That(results, Is.Not.Null);
        //    Assert.That(results.Count(), Is.EqualTo(2));
        //}
    }
}