using NaviriaAPI.DTOs;
using NaviriaAPI.Exceptions;

namespace NaviriaAPI.IServices
{
    /// <summary>
    /// Provides advanced user search operations, including search by task category,
    /// nickname among friends, potential friends, and incoming friend requests.
    /// </summary>
    public interface IUserSearchService
    {
        /// <summary>
        /// Gets all users who have at least one task in the given category.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>List of users (UserDto) who have at least one task in this category.</returns>
        /// <exception cref="NotFoundException">Thrown if no users are found for this category.</exception>
        Task<List<UserDto>> GetUsersByTaskCategoryAsync(string categoryId);

        /// <summary>
        /// Gets all potential friends for a user who have at least one task in the given category.
        /// Potential friends are users who are not already friends and are not the user themself.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="categoryId">The category ID.</param>
        /// <returns>List of UserDto matching the conditions.</returns>
        /// <exception cref="NotFoundException">Thrown if no potential friends are found for this category.</exception>
        Task<List<UserDto>> GetPotentialFriendsByTaskCategoryAsync(string userId, string categoryId);

        /// <summary>
        /// Searches potential friends (users who are not yet friends and not the user themself) by nickname.
        /// </summary>
        /// <param name="userId">ID of the user performing the search.</param>
        /// <param name="query">Search string (part of nickname).</param>
        /// <returns>List of UserDto matching the search among potential friends.</returns>
        Task<IEnumerable<UserDto>> SearchPotentialFriendsByNicknameAsync(string userId, string query);

        /// <summary>
        /// Searches friends of the user by nickname.
        /// </summary>
        /// <param name="userId">ID of the user whose friends to search.</param>
        /// <param name="query">Search string (part of nickname).</param>
        /// <returns>List of UserDto matching the search among user's friends.</returns>
        Task<IEnumerable<UserDto>> SearchFriendsByNicknameAsync(string userId, string query);

        /// <summary>
        /// Searches among users who sent a friend request to the specified user, by nickname.
        /// </summary>
        /// <param name="userId">The ID of the user who received the friend requests.</param>
        /// <param name="query">Search string (part of nickname).</param>
        /// <returns>List of UserDto matching the search among users who sent incoming friend requests.</returns>
        Task<IEnumerable<UserDto>> SearchIncomingFriendRequestsByNicknameAsync(string userId, string query);
    }
}
