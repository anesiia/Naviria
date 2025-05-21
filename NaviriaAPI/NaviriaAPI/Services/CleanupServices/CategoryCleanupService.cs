using CloudinaryDotNet.Actions;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices.ICleanupServices;

namespace NaviriaAPI.Services.CleanupServices
{
    public class CategoryCleanupService : ICategoryCleanupService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITaskRepository _taskRepository;

        public CategoryCleanupService(
            ICategoryRepository categoryRepository,
            ITaskRepository taskRepository)
        {
            _categoryRepository = categoryRepository;
            _taskRepository = taskRepository;
        }

        public async Task<bool> DeleteCategoryAndTasksAsync(string categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
                throw new NotFoundException($"Category with ID {categoryId} not found.");

            // Delete all tasks with certain category
            await _taskRepository.DeleteManyByCategoryIdAsync(categoryId);

            // Delete the category itself
            var deleted = await _categoryRepository.DeleteAsync(categoryId);

            return deleted;
        }
    }
}
