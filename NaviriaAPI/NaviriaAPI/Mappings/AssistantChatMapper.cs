using NaviriaAPI.DTOs.FeaturesDTOs;
using NaviriaAPI.Entities;

namespace NaviriaAPI.Mappings
{
    public static class AssistantChatMapper
    {
        public static AssistantChatMessageEntity ToEntity(AssistantChatMessageDto dto, string role)
        {
            return new AssistantChatMessageEntity
            {
                UserId = dto.UserId,
                Role = role,
                Content = dto.Message
            };
        }

        public static AssistantChatMessageDto ToDto(AssistantChatMessageEntity entity)
        {
            return new AssistantChatMessageDto
            {
                UserId = entity.UserId,
                Message = entity.Content
            };
        }
    }
}
