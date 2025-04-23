using NaviriaAPI.Entities;

namespace NaviriaAPI.IRepositories
{
    public interface IAssistantChatRepository
    {
        Task<IEnumerable<AssistantChatMessageEntity>> GetByUserIdAsync(string userId);
        Task AddMessageAsync(AssistantChatMessageEntity message);
        Task DeleteAllForUserAsync(string userId);
        Task<int> CountByUserIdAsync(string userId);
    }
}
