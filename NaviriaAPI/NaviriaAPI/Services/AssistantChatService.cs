using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using OpenAI.Chat;

namespace NaviriaAPI.Services
{
    public class AssistantChatService : IAssistantChatService
    {
        private const int MaxMessagesPerUser = 20; // 10 пар

        private readonly IAssistantChatRepository _chatRepository;
        private readonly string _openAiKey;

        public AssistantChatService(IAssistantChatRepository chatRepository, IConfiguration config)
        {
            _chatRepository = chatRepository;
            _openAiKey = config["OpenAIKey"];
        }

        public async Task<string> SendMessageAsync(string userId, string message)
        {
            // Очистити якщо перевищено
            var count = await _chatRepository.CountByUserIdAsync(userId);
            if (count >= MaxMessagesPerUser)
            {
                await _chatRepository.DeleteAllForUserAsync(userId);
            }

            // Зберегти запит
            var userMessage = new AssistantChatMessageEntity
            {
                UserId = userId,
                Role = "user",
                Content = message
            };
            await _chatRepository.AddMessageAsync(userMessage);

            // Отримати історію
            var history = (await _chatRepository.GetByUserIdAsync(userId))
                          .TakeLast(10) // контекст максимум 5 пар
                          .Select(m => new AssistantChatMessage(m.Role, m.Content))
                          .ToList();

            // Виклик OpenAI
            var client = new OpenAI.Chat.ChatClient("gpt-4o-mini", _openAiKey);
            var result = await client.CompleteChatAsync(history);

            var reply = result.Value.Content.First().Text;

            // Зберегти відповідь
            var assistantMessage = new AssistantChatMessageEntity
            {
                UserId = userId,
                Role = "assistant",
                Content = reply
            };
            await _chatRepository.AddMessageAsync(assistantMessage);

            return reply;
        }
    }

}
