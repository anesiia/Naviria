using MongoDB.Driver;
using NaviriaAPI.Data;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;

namespace NaviriaAPI.Repositories
{
    public class AssistantChatRepository : IAssistantChatRepository
    {
        private readonly IMongoCollection<AssistantChatMessageEntity> _ai_chat_messages;

        public AssistantChatRepository(IMongoDbContext database)
        {
            _ai_chat_messages = database.AssistantChatMessages;
        }

        public async Task<IEnumerable<AssistantChatMessageEntity>> GetByUserIdAsync(string userId) =>
            await _ai_chat_messages.Find(m => m.UserId == userId).SortBy(m => m.CreatedAt).ToListAsync();

        public async Task AddMessageAsync(AssistantChatMessageEntity message) =>
            await _ai_chat_messages.InsertOneAsync(message);

        public async Task DeleteAllForUserAsync(string userId) =>
            await _ai_chat_messages.DeleteManyAsync(m => m.UserId == userId);

        public async Task<int> CountByUserIdAsync(string userId) =>
            (int)await _ai_chat_messages.CountDocumentsAsync(m => m.UserId == userId);

        public async Task DeleteManyByUserIdAsync(string userId)
        {
            var filter = Builders<AssistantChatMessageEntity>.Filter.Eq(m => m.UserId, userId);
            await _ai_chat_messages.DeleteManyAsync(filter);
        }
    }

}
