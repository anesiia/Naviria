using NaviriaAPI.DTOs;
using NaviriaAPI.DTOs.FeaturesDTOs;

namespace NaviriaAPI.IServices
{
    /// <summary>
    /// Service for managing user friendships and discovering shared interests.
    /// </summary>
    public interface IFriendService
    {
        /// <summary>
        /// Gets the list of friends for a specific user.
        /// </summary>
        /// <param name="userId">The user's unique identifier.</param>
        /// <returns>A collection of <see cref="UserDto"/> representing the user's friends.</returns>
        Task<IEnumerable<UserDto>> GetUserFriendsAsync(string userId);

        /// <summary>
        /// Deletes a friendship between two users.
        /// </summary>
        /// <param name="fromUserId">The ID of the user who wants to remove a friend.</param>
        /// <param name="friendId">The ID of the friend to be removed.</param>
        /// <returns>True if the friendship was deleted successfully, otherwise false.</returns>
        Task<bool> DeleteFriendAsync(string fromUserId, string friendId);

        /// <summary>
        /// Gets a list of potential friends for a user (users who are not already friends).
        /// </summary>
        /// <param name="userId">The user's unique identifier.</param>
        /// <returns>A collection of <see cref="UserDto"/> representing potential friends.</returns>
        Task<IEnumerable<UserDto>> GetPotentialFriendsAsync(string userId);

        /// <summary>
        /// Searches users by nickname, excluding the current user's friends and themselves.
        /// </summary>
        /// <param name="userId">The user's unique identifier.</param>
        /// <param name="query">The nickname query to search for.</param>
        /// <returns>A collection of <see cref="UserDto"/> matching the search criteria.</returns>
        Task<IEnumerable<UserDto>> SearchUsersByNicknameAsync(string userId, string query);

        /// <summary>
        /// Finds shared interests between two users, such as common task categories and tags.
        /// </summary>
        /// <param name="userId1">The unique identifier of the first user.</param>
        /// <param name="userId2">The unique identifier of the second user.</param>
        /// <returns>
        /// A <see cref="SharedInterestsDto"/> object containing the list of shared category names and shared tag names.
        /// </returns>
        Task<SharedInterestsDto> GetSharedInterestsAsync(string userId1, string userId2);
    }
}
