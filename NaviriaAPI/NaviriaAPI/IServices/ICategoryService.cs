using NaviriaAPI.Entities;
using NaviriaAPI.DTOs.Category;

namespace NaviriaAPI.IServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(string id);
        Task<CategoryDto> CreateAsync(CategoryCreateDto categoryDto);
        Task<bool> UpdateAsync(string id, CategoryUpdateDto categoryDto);
        Task<bool> DeleteAsync(string id);
        Task<CategoryEntity?> GetByNameAsync(string name);
    }
}
