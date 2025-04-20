using NaviriaAPI.Entities;

namespace NaviriaAPI.IRepositories
{
    public interface IFriendRequestRepository
    {
        Task<List<FriendRequestEntity>> GetAllAsync();
        Task<FriendRequestEntity?> GetByIdAsync(string id);
        Task CreateAsync(FriendRequestEntity quote);
        Task<bool> UpdateAsync(FriendRequestEntity quote);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<FriendRequestEntity>> GetByReceiverIdAsync(string toUserId);

    }
}
