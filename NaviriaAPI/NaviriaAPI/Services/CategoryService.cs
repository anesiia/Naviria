using NaviriaAPI.IRepositories;
using NaviriaAPI.Mappings;
using NaviriaAPI.IServices;
using NaviriaAPI.Entities;
using NaviriaAPI.DTOs.Category;

namespace NaviriaAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<CategoryDto> CreateAsync(CategoryCreateDto categoryDto)
        {
            var entity = CategoryMapper.ToEntity(categoryDto);
            await _categoryRepository.CreateAsync(entity);
            return CategoryMapper.ToDto(entity);
        }
        public async Task<bool> UpdateAsync(string id, CategoryUpdateDto categoryDto)
        {
            var entity = CategoryMapper.ToEntity(id, categoryDto);
            return await _categoryRepository.UpdateAsync(entity);
        }
        public async Task<CategoryDto?> GetByIdAsync(string id)
        {
            var entity = await _categoryRepository.GetByIdAsync(id);
            return entity == null ? null : CategoryMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(string id) =>
            await _categoryRepository.DeleteAsync(id);

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(CategoryMapper.ToDto).ToList();
        }
        public async Task<CategoryEntity?> GetByNameAsync(string name)
        {
            return await _categoryRepository.GetByNameAsync(name);
        }
    }
}
