namespace NaviriaAPI.IServices.ICleanupServices
{
    /// <summary>
    /// Service for cascading deletion of a category and all tasks assigned to that category.
    /// </summary>
    public interface ICategoryCleanupService
    {
        /// <summary>
        /// Deletes the category and all tasks related to this category.
        /// </summary>
        /// <param name="categoryId">The identifier of the category to delete.</param>
        /// <returns>
        /// Returns <c>true</c> if deletion was successful; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> DeleteCategoryAndTasksAsync(string categoryId);
    }
}
