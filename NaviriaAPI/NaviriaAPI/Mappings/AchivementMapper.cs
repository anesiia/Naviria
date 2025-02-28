using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.DTOs.UdateDTOs;
using SharpCompress.Common;

namespace NaviriaAPI.Mappings
{
    public class AchievementMapper
    {
        // Entity → DTO (для відправки в API)
        public static AchievementDto ToDto(AchievementEntity entity) =>
            new AchievementDto { Id = entity.Id, Name = entity.Name, Description = entity.Description };

        // 🟩 DTO → Entity (для збереження в БД)
        public static AchievementEntity ToEntity(AchievementDto dto) =>
            new AchievementEntity { Id = dto.Id, Name = dto.Name, Description = dto.Description };

        // CreateDto → Entity (для створення)
        public static AchievementEntity ToEntity(AchievementCreateDto dto) =>
            new AchievementEntity { Name = dto.Name, Description = dto.Description };

        // UpdateDto → Entity (для оновлення)
        public static AchievementEntity ToEntity(string id, AchievementUpdateDto dto) =>
            new AchievementEntity { Id = id, Name = dto.Name, Description = dto.Description };
    }
}
