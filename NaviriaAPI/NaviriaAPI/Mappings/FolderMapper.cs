using NaviriaAPI.DTOs.Folder;
using NaviriaAPI.Entities;

namespace NaviriaAPI.Mappings
{
    public static class FolderMapper
    {
        public static FolderDto ToDto(FolderEntity entity) => new()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Name = entity.Name,
            CreatedAt = entity.CreatedAt
        };

        public static FolderEntity ToEntity(string userId, FolderCreateDto dto) => new()
        {
            UserId = userId,
            Name = dto.Name,
            CreatedAt = DateTime.UtcNow
        };

        public static void UpdateEntity(FolderEntity entity, FolderUpdateDto dto)
        {
            entity.Name = dto.Name;
        }
    }
}
