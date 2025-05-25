using NaviriaAPI.DTOs.Category;
using NaviriaAPI.Entities;

namespace NaviriaAPI.Mappings
{
    public static class CategoryMapper
    {
        
        // Entity → DTO (для відправки в API)
        public static CategoryDto ToDto(CategoryEntity entity) =>
            new CategoryDto { Id = entity.Id, Name = entity.Name };

        // 🟩 DTO → Entity (для збереження в БД)
        public static CategoryEntity ToEntity(CategoryDto dto) =>
            new CategoryEntity { Id = dto.Id, Name = dto.Name };

        // CreateDto → Entity (для створення)
        public static CategoryEntity ToEntity(CategoryCreateDto dto) =>
            new CategoryEntity { Name = dto.Name };

        // UpdateDto → Entity (для оновлення)
        public static CategoryEntity ToEntity(string id, CategoryUpdateDto dto) =>
            new CategoryEntity { Id = id, Name = dto.Name };

    }

}
