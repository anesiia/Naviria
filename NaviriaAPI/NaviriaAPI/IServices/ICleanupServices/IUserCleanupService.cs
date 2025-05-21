namespace NaviriaAPI.IServices.ICleanupServices
{
    /// <summary>
    /// Service interface for cascading deletion of a user and all related data.
    /// </summary>
    public interface IUserCleanupService
    {
        /// <summary>
        /// Deletes the specified user and all related data, such as folders, notifications,
        /// friend requests, assistant chat messages, and tasks.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>
        /// Returns <c>true</c> if the user and all related data were deleted successfully;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// If the user does not exist, returns <c>false</c>.
        /// If any internal error occurs during deletion, the exception will be thrown.
        /// </remarks>
        Task<bool> DeleteUserAndRelatedDataAsync(string userId);
    }
}
