using NaviriaAPI.DTOs.FeaturesDTOs;

namespace NaviriaAPI.IServices
{
    public interface IAssistantChatService
    {
        Task<IEnumerable<AssistantChatMessageDto>> GetUserChatAsync(string userId);
        Task<string> SendMessageAsync(AssistantChatMessageDto dto);
    }
}
