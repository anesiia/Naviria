using NaviriaAPI.DTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.DTOs.Achievement;
namespace NaviriaAPI.Mappings
{
    public class AchievementMapper
    {
        public static AchievementDto ToDto(AchievementEntity entity) =>
            new AchievementDto { 
                Id = entity.Id, 
                Name = entity.Name, 
                Description = entity.Description, 
                Points = entity.Points, 
                IsRare = entity.IsRare,
            };

        public static AchievementEntity ToEntity(AchievementDto dto) =>
            new AchievementEntity { 
                Id = dto.Id, 
                Name = dto.Name, 
                Description = dto.Description,
                Points = dto.Points,
                IsRare = dto.IsRare,
            };

        public static AchievementEntity ToEntity(AchievementCreateDto dto) =>
            new AchievementEntity
            {
                Name = dto.Name,
                Description = dto.Description,
                Points = dto.Points,
                IsRare = dto.IsRare,
            };

        public static AchievementEntity ToEntity(string id, AchievementUpdateDto dto) =>
            new AchievementEntity
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description,
                Points = dto.Points,
                IsRare = dto.IsRare,
            };
    }
}
