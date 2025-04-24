using NaviriaAPI.DTOs.FeaturesDTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.Mappings;
using NaviriaAPI.Exceptions;
using OpenAI.Chat;

namespace NaviriaAPI.Services
{
    public class AssistantChatService : IAssistantChatService
    {
        private const int MaxMessagesPerUser = 20; // 10 пар

        private readonly IAssistantChatRepository _chatRepository;
        private readonly IUserService _userService;
        private readonly ILogger<AssistantChatService> _logger;
        private readonly string _openAiKey;

        public AssistantChatService(
            IAssistantChatRepository chatRepository,
            IConfiguration config,
            ILogger<AssistantChatService> logger,
            IUserService userService)
        {
            _chatRepository = chatRepository;
            _openAiKey = config["OpenAIKey"];
            _logger = logger;
            _userService = userService;
        }
        public async Task<IEnumerable<AssistantChatMessageDto>> GetUserChatAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("GetUserchatAsync was called with an empty or null userId.");
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
            }

            if (!await _userService.UserExistsAsync(userId))
            {
                _logger.LogWarning("User with ID {UserId} not found.", userId);
                throw new NotFoundException($"User with ID {userId} does not exist.");
            }

            _logger.LogInformation("Retrieving assistant chat messages for user: {UserId}", userId);

            var entities = await _chatRepository.GetByUserIdAsync(userId);
            if (!entities.Any())
            {
                _logger.LogInformation("No assistant chat messages found for user: {UserId}", userId);
            }

            return entities.Select(AssistantChatMapper.ToDto);
        }

        public async Task<string> SendMessageAsync(AssistantChatMessageDto dto)
        {
            ValidateInputs(dto);

            await ClearIfLimitExceeded(dto.UserId);

            var userMessage = AssistantChatMapper.ToEntity(dto, "user");
            await _chatRepository.AddMessageAsync(userMessage);

            var history = (await _chatRepository.GetByUserIdAsync(dto.UserId))
                          .TakeLast(10)
                          .Select(m => new AssistantChatMessage(m.Role, m.Content))
                          .ToList();

            var client = new OpenAI.Chat.ChatClient("gpt-4o-mini", _openAiKey);
            var result = await client.CompleteChatAsync(history);

            var reply = result.Value.Content.First().Text;

            var replyEntity = new AssistantChatMessageEntity
            {
                UserId = dto.UserId,
                Role = "assistant",
                Content = reply
            };

            await _chatRepository.AddMessageAsync(replyEntity);

            return reply;
        }

        private void ValidateInputs(AssistantChatMessageDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId))
                throw new ArgumentException("User ID is required.");
            if (string.IsNullOrWhiteSpace(dto.Message))
                throw new ArgumentException("Message is required.");
        }

        private async Task ClearIfLimitExceeded(string userId)
        {
            var count = await _chatRepository.CountByUserIdAsync(userId);
            if (count >= MaxMessagesPerUser)
                await _chatRepository.DeleteAllForUserAsync(userId);
        }
    }
}
