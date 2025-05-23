using NaviriaAPI.Data;
using NaviriaAPI.Repositories;
using NaviriaAPI.IRepositories;
using System;
using MongoDB.Driver;
using NaviriaAPI.Entities;
using System.Linq.Expressions;

public class FriendRequestRepository : IFriendRequestRepository
{
    private readonly IMongoCollection<FriendRequestEntity> _friendsRequests;

    public FriendRequestRepository(IMongoDbContext database)
    {
        _friendsRequests = database.FriendsRequests;
    }

    public async Task<List<FriendRequestEntity>> GetAllAsync() =>
        await _friendsRequests.Find(_ => true).ToListAsync();

    public async Task<FriendRequestEntity?> GetByIdAsync(string id) =>
        await _friendsRequests.Find(c => c.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(FriendRequestEntity friendReq) =>
        await _friendsRequests.InsertOneAsync(friendReq);

    public async Task<bool> UpdateAsync(FriendRequestEntity friendReq)
    {
        var result = await _friendsRequests.ReplaceOneAsync(f => f.Id == friendReq.Id, friendReq);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _friendsRequests.DeleteOneAsync(f => f.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<IEnumerable<FriendRequestEntity>> GetByReceiverIdAsync(string toUserId)
    {
        var filter = Builders<FriendRequestEntity>.Filter.Eq(r => r.ToUserId, toUserId);
        return await _friendsRequests.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<FriendRequestEntity>> GetBySenderIdAsync(string fromUserId)
    {
        var filter = Builders<FriendRequestEntity>.Filter.Eq(r => r.FromUserId, fromUserId);
        var results = await _friendsRequests.Find(filter).ToListAsync();
        return results;
    }

    public async Task DeleteManyByUserIdAsync(string userId)
    {
        var filter = Builders<FriendRequestEntity>.Filter.Or(
            Builders<FriendRequestEntity>.Filter.Eq(fr => fr.FromUserId, userId),
            Builders<FriendRequestEntity>.Filter.Eq(fr => fr.ToUserId, userId)
        );

        await _friendsRequests.DeleteManyAsync(filter);
    }

}


