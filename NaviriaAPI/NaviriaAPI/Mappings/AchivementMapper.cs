using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.DTOs.UdateDTOs;
using SharpCompress.Common;

namespace NaviriaAPI.Mappings
{
    public class AchivementMapper
    {
        // Entity → DTO (для відправки в API)
        public static AchivementDto ToDto(AchivementEntity entity) =>
            new AchivementDto { Id = entity.Id, Name = entity.Name, Description = entity.Description };

        // 🟩 DTO → Entity (для збереження в БД)
        public static AchivementEntity ToEntity(AchivementDto dto) =>
            new AchivementEntity { Id = dto.Id, Name = dto.Name, Description = dto.Description };

        // CreateDto → Entity (для створення)
        public static AchivementEntity ToEntity(AchivementCreateDto dto) =>
            new AchivementEntity { Name = dto.Name, Description = dto.Description };

        // UpdateDto → Entity (для оновлення)
        public static AchivementEntity ToEntity(string id, AchivementUpdateDto dto) =>
            new AchivementEntity { Id = id, Name = dto.Name, Description = dto.Description };
    }
}
