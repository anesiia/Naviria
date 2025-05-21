namespace NaviriaAPI.IServices.ICleanupServices
{
    /// <summary>
    /// Service interface for cascading deletion of a folder and all tasks in that folder.
    /// </summary>
    public interface IFolderCleanupService
    {
        /// <summary>
        /// Deletes the specified folder and all tasks belonging to this folder.
        /// </summary>
        /// <param name="folderId">The ID of the folder to delete.</param>
        /// <returns>
        /// Returns <c>true</c> if the folder and all related tasks were deleted successfully;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// If the folder does not exist, returns <c>false</c>.
        /// If any internal error occurs during deletion, the exception will be thrown.
        /// </remarks>
        Task<bool> DeleteFolderAndTasksAsync(string folderId);
    }
}
