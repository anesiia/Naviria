using NaviriaAPI.DTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Mappings;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IServices;
using NaviriaAPI.IServices.ISecurityService;
using NaviriaAPI.IServices.IUserServices;

namespace NaviriaAPI.Services.User
{
    /// <summary>
    /// Provides advanced user search operations, including search by task category,
    /// nickname among friends, potential friends, and incoming friend requests.
    /// </summary>
    public class UserSearchService : IUserSearchService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IMessageSecurityService _messageSecurityService;

        public UserSearchService(
            ITaskRepository taskRepository,
            IUserRepository userRepository,
            IUserService userService,
            IFriendRequestRepository friendRequestRepository,
            IMessageSecurityService messageSecurityService)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _userService = userService;
            _friendRequestRepository = friendRequestRepository;
            _messageSecurityService = messageSecurityService;
        }

        /// <summary>
        /// Gets all users who have at least one task in the given category.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>List of users (UserDto) who have at least one task in this category.</returns>
        /// <exception cref="NotFoundException">Thrown if no users are found for this category.</exception>
        public async Task<List<UserDto>> GetUsersByTaskCategoryAsync(string categoryId)
        {
            var userIds = await _taskRepository.GetUserIdsByCategoryAsync(categoryId);

            if (userIds == null || !userIds.Any())
                throw new NotFoundException("No users found for this category.");

            var usersByCategory = await _userRepository.GetManyByIdsAsync(userIds);

            var userDtos = usersByCategory.Select(UserMapper.ToDto).ToList();

            return userDtos;
        }

        /// <summary>
        /// Searches potential friends (users who are not yet friends and not the user themself) by nickname.
        /// </summary>
        /// <param name="userId">ID of the user performing the search.</param>
        /// <param name="query">Search string (part of nickname).</param>
        /// <returns>List of UserDto matching the search among potential friends.</returns>
        public async Task<IEnumerable<UserDto>> SearchPotentialFriendsByNicknameAsync(string userId, string query)
        {
            var user = await _userService.GetUserOrThrowAsync(userId);
            var excludedIds = user.Friends.Select(f => f.UserId).Append(userId).ToHashSet();

            _messageSecurityService.Validate(userId, query);

            var allUsers = await _userRepository.GetAllAsync();

            var matched = allUsers
                .Where(u => !excludedIds.Contains(u.Id))
                .Where(u => u.Nickname.Contains(query, StringComparison.OrdinalIgnoreCase))
                .OrderBy(u => u.Nickname)
                .Select(UserMapper.ToDto);

            return matched;
        }

        /// <summary>
        /// Searches friends of the user by nickname.
        /// </summary>
        /// <param name="userId">ID of the user whose friends to search.</param>
        /// <param name="query">Search string (part of nickname).</param>
        /// <returns>List of UserDto matching the search among user's friends.</returns>
        public async Task<IEnumerable<UserDto>> SearchFriendsByNicknameAsync(string userId, string query)
        {
            var user = await _userService.GetUserOrThrowAsync(userId);
            var friendIds = user.Friends.Select(f => f.UserId).ToList();

            _messageSecurityService.Validate(userId, query);

            if (!friendIds.Any())
                return Enumerable.Empty<UserDto>();

            var friends = await _userRepository.GetManyByIdsAsync(friendIds);

            var matched = friends
                .Where(u => u.Nickname.Contains(query, StringComparison.OrdinalIgnoreCase))
                .OrderBy(u => u.Nickname)
                .Select(UserMapper.ToDto);

            return matched;
        }

        /// <summary>
        /// Searches among users who sent a friend request to the specified user, by nickname.
        /// </summary>
        /// <param name="userId">The ID of the user who received the friend requests.</param>
        /// <param name="query">Search string (part of nickname).</param>
        /// <returns>List of UserDto matching the search among users who sent incoming friend requests.</returns>
        public async Task<IEnumerable<UserDto>> SearchIncomingFriendRequestsByNicknameAsync(string userId, string query)
        {
            _messageSecurityService.Validate(userId, query);

            var incomingRequests = await _friendRequestRepository.GetByReceiverIdAsync(userId);
            var requesterIds = incomingRequests.Select(r => r.FromUserId).Distinct().ToList();

            if (!requesterIds.Any())
                return Enumerable.Empty<UserDto>();

            var requesters = await _userRepository.GetManyByIdsAsync(requesterIds);

            var matched = requesters
                .Where(u => u.Nickname.Contains(query, StringComparison.OrdinalIgnoreCase))
                .OrderBy(u => u.Nickname)
                .Select(UserMapper.ToDto);

            return matched;
        }
    }
}
