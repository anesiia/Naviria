using NaviriaAPI.DTOs.User;

namespace NaviriaAPI.IServices
{
    public interface IUserSearchService
    {
        /// <summary>
        /// Searches all users in the system (excluding the specified user) by optional category and/or nickname or full name.
        /// </summary>
        /// <param name="userId">The ID of the user performing the search (will be excluded from results).</param>
        /// <param name="categoryId">Optional category ID to filter users who have at least one task in this category. If null, no category filter is applied.</param>
        /// <param name="query">Optional search string to match user's nickname or full name. If null, no name filter is applied.</param>
        /// <returns>
        /// A list of <see cref="UserDto"/> matching the specified criteria.
        /// </returns>
        /// <exception cref="NotFoundException">
        /// Thrown if no users are found matching the criteria.
        /// </exception>
        Task<List<UserDto>> SearchAllUsersAsync(string userId, string? categoryId, string? query);

        /// <summary>
        /// Searches among the specified user's friends by optional category and/or nickname or full name.
        /// </summary>
        /// <param name="userId">The ID of the user whose friends will be searched.</param>
        /// <param name="categoryId">Optional category ID to filter friends who have at least one task in this category. If null, no category filter is applied.</param>
        /// <param name="query">Optional search string to match friend's nickname or full name. If null, no name filter is applied.</param>
        /// <returns>
        /// A list of <see cref="UserDto"/> representing friends matching the specified criteria.
        /// </returns>
        /// <exception cref="NotFoundException">
        /// Thrown if no friends are found matching the criteria.
        /// </exception>
        Task<List<UserDto>> SearchFriendsAsync(string userId, string? categoryId, string? query);

        /// <summary>
        /// Searches among users who have sent a friend request to the specified user, by optional category and/or nickname or full name.
        /// </summary>
        /// <param name="userId">The ID of the user who received the friend requests.</param>
        /// <param name="categoryId">Optional category ID to filter users who have at least one task in this category. If null, no category filter is applied.</param>
        /// <param name="query">Optional search string to match requester's nickname or full name. If null, no name filter is applied.</param>
        /// <returns>
        /// A list of <see cref="UserDto"/> representing users who sent friend requests and match the specified criteria.
        /// </returns>
        /// <exception cref="NotFoundException">
        /// Thrown if no incoming friend requests match the criteria.
        /// </exception>
        Task<List<UserDto>> SearchIncomingFriendRequestsAsync(string userId, string? categoryId, string? query);
    }
}
