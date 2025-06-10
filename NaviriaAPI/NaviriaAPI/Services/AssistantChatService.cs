using System.Text.Json;
using NaviriaAPI.DTOs.Folder;
using NaviriaAPI.DTOs.FeaturesDTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.Mappings;
using NaviriaAPI.Exceptions;
using NaviriaAPI.Helpers;
using NaviriaAPI.Constants;
using OpenAI.Chat;
using NaviriaAPI.IServices.ISecurityService;
using NaviriaAPI.IServices.IUserServices;
using NaviriaAPI.DTOs.Task.Create;
using System.Text.Json.Nodes;

namespace NaviriaAPI.Services
{
    public class AssistantChatService : IAssistantChatService
    {
        private readonly IAssistantChatRepository _chatRepository;
        private readonly IUserService _userService;
        private readonly IFolderService _folderService;
        private readonly IMessageSecurityService _messageSecurityService;
        private readonly ILogger<AssistantChatService> _logger;
        private readonly ITaskService _taskService;
        private readonly ICategoryService _categoryService;
        private readonly string _openAiKey;

        public AssistantChatService(
            IAssistantChatRepository chatRepository,
            IConfiguration config,
            ILogger<AssistantChatService> logger,
            IFolderService folderService,
            IUserService userService,
            ITaskService taskService,
            IMessageSecurityService messageSecurityService,
            ICategoryService categoryService)
        {
            _chatRepository = chatRepository;
            _openAiKey = config["OpenAIKey"];
            _logger = logger;
            _folderService = folderService;
            _userService = userService;
            _taskService = taskService;
            _messageSecurityService = messageSecurityService;
            _categoryService = categoryService;
        }

        public async Task<IEnumerable<AssistantChatMessageDto>> GetUserChatAsync(string userId)
        {
            await EnsureUserExistsAsync(userId);

            var entities = await _chatRepository.GetByUserIdAsync(userId);

            foreach( var entity in entities)
            {
                entity.CreatedAt = entity.CreatedAt.ToLocalTime();
            }

            return entities.Select(AssistantChatMapper.ToDto);
        }

        public async Task<string> SendMessageAsync(AssistantChatMessageDto dto)
        {
            ValidateInputs(dto);
            _messageSecurityService.Validate(dto.UserId, dto.Message);
            await EnsureMessageLimitNotExceededAsync(dto.UserId);

            return dto.IsTaskRequest
                ? await ProcessTaskCreationAsync(dto)
                : await ProcessGeneralReplyAsync(dto);
        }

        private async Task<string> ProcessGeneralReplyAsync(AssistantChatMessageDto dto)
        {
            var userMessage = AssistantChatMapper.ToEntity(dto, "user");
            await _chatRepository.AddMessageAsync(userMessage);

            var history = (await _chatRepository.GetByUserIdAsync(dto.UserId))
                          .TakeLast(10)
                          .Select(m => new AssistantChatMessage(m.Role, m.Content))
                          .ToList();

            var client = new ChatClient(AssistantChatConstants.DefaultModel, _openAiKey);
            var result = await client.CompleteChatAsync(history);

            var reply = result.Value.Content[0].Text;

            var replyEntity = new AssistantChatMessageEntity
            {
                UserId = dto.UserId,
                Role = "assistant",
                CreatedAt = DateTime.UtcNow,
                Content = reply
            };

            await _chatRepository.AddMessageAsync(replyEntity);

            return reply;
        }

        private async Task<string> ProcessTaskCreationAsync(AssistantChatMessageDto dto)
        {
            var prompt = PromptTemplates.WithUserMessage(PromptTemplates.TaskCreationPrompt, dto.Message);
            await _chatRepository.AddMessageAsync(AssistantChatMapper.ToEntity(dto, "user"));

            var client = new ChatClient(AssistantChatConstants.TaskModel, _openAiKey);
            var result = await client.CompleteChatAsync(prompt);

            var rawReply = result.Value.Content[0].Text;
            var extractedJson = JsonHelper.ExtractJsonFromCodeBlock(rawReply);

            await Console.Out.WriteLineAsync(rawReply);

            if (!extractedJson.TrimStart().StartsWith("{"))
            {
                _logger.LogWarning("GPT returned non-JSON response: {RawReply}", rawReply);
                return "GPT did not generated valid JSON. Try formulating a more precise query.";
            }

            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var node = System.Text.Json.Nodes.JsonNode.Parse(extractedJson);
                var type = node?["Type"]?.GetValue<string>()?.ToLower() ?? "standard";

                TaskCreateDto? taskDto = type switch
                {
                    "standard" => node.Deserialize<TaskStandardCreateDto>(options),
                    "repeatable" => node.Deserialize<TaskRepeatableCreateDto>(options),
                    "scale" => node.Deserialize<TaskScaleCreateDto>(options),
                    _ => node.Deserialize<TaskCreateDto>(options)
                };

                if (taskDto == null || string.IsNullOrWhiteSpace(taskDto.Title))
                    return "Failed to read task. GPT did not fill in important fields.";

                taskDto.UserId = dto.UserId;
                taskDto.FolderId = await GetOrCreateAssistantFolderId(dto.UserId);

                var category = await _categoryService.GetByNameAsync(AssistantChatConstants.GeneratedCategoryName);
                if (category == null)
                    return "Category 'AI-generated task' not found. Please create it manually or contact your administrator.";

                taskDto.CategoryId = category.Id;

                await _taskService.CreateAsync(taskDto);

                var subtaskSummary = taskDto.Subtasks?
                    .GroupBy(s => s.Type)
                    .Select(g => $"{g.Count()} {g.Key}")
                    .ToList() ?? new List<string>();

                var confirmation = $"✅ Задача \"{taskDto.Title}\" створена ({string.Join(", ", subtaskSummary)}).";

                await _chatRepository.AddMessageAsync(new AssistantChatMessageEntity
                {
                    UserId = dto.UserId,
                    Role = "assistant",
                    Content = confirmation,
                    CreatedAt = DateTime.UtcNow
                });

                return confirmation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse and save task from GPT response.");
                return "An error occurred while processing the task. GPT may have generated invalid JSON.";
            }
        }

        private async Task<string> GetOrCreateAssistantFolderId(string userId)
        {
            var folders = await _folderService.GetAllByUserIdAsync(userId);
            var existing = folders.FirstOrDefault(f => f.Name == AssistantChatConstants.AssistantFolderName);

            if (existing != null)
                return existing.Id;

            var newFolder = await _folderService.CreateAsync(new FolderCreateDto
            {
                Name = AssistantChatConstants.AssistantFolderName,
                UserId = userId
            });

            return newFolder.Id;
        }

        private async Task EnsureUserExistsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            if (!await _userService.UserExistsAsync(userId))
                throw new NotFoundException($"User with ID {userId} does not exist.");
        }

        private void ValidateInputs(AssistantChatMessageDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId))
                throw new ArgumentException("User ID is required.");
            if (string.IsNullOrWhiteSpace(dto.Message))
                throw new ArgumentException("Message is required.");
        }

        private async Task EnsureMessageLimitNotExceededAsync(string userId)
        {
            var count = await _chatRepository.CountByUserIdAsync(userId);
            if (count >= AssistantChatConstants.MaxMessagesPerUser)
                await _chatRepository.DeleteAllForUserAsync(userId);
        }
    }
}