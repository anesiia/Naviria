using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices.ICleanupServices;

namespace NaviriaAPI.Services.CleanupServices
{
    public class CategoryCleanupService : ICategoryCleanupService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<CategoryCleanupService> _logger;

        public CategoryCleanupService(
            ICategoryRepository categoryRepository,
            ITaskRepository taskRepository,
            ILogger<CategoryCleanupService> logger)
        {
            _categoryRepository = categoryRepository;
            _taskRepository = taskRepository;
            _logger = logger;
        }

        public async Task<bool> DeleteCategoryAndTasksAsync(string categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                _logger.LogWarning("Category with ID {CategoryId} not found", categoryId);
                return false;
            }

            try
            {
                // Видаляємо всі задачі з цією категорією
                await _taskRepository.DeleteManyByCategoryIdAsync(categoryId);

                // Видаляємо категорію
                var deleted = await _categoryRepository.DeleteAsync(categoryId);

                if (!deleted)
                {
                    _logger.LogError("Failed to delete category with ID {CategoryId}", categoryId);
                    return false;
                }

                _logger.LogInformation("Category with ID {CategoryId} and all its tasks were deleted", categoryId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during cascade deletion for category {CategoryId}", categoryId);
                throw;
            }
        }
    }
}
