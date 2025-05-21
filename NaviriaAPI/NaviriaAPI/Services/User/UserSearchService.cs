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

        /// <inheritdoc />
        public async Task<List<UserDto>> GetUsersByTaskCategoryAsync(string categoryId)
        {
            var userIds = await _taskRepository.GetUserIdsByCategoryAsync(categoryId);
            return await GetUserDtosOrThrowAsync(userIds, "No users found for this category.");
        }

        /// <inheritdoc />
        public async Task<List<UserDto>> GetPotentialFriendsByTaskCategoryAsync(string userId, string categoryId)
        {
            var user = await _userService.GetUserOrThrowAsync(userId);
            var excludeIds = user.Friends.Select(f => f.UserId).Append(userId).ToHashSet();

            var userIds = await _taskRepository.GetUserIdsByCategoryAsync(categoryId);
            var filtered = userIds?.Where(id => !excludeIds.Contains(id)) ?? Enumerable.Empty<string>();

            return await GetUserDtosOrThrowAsync(filtered, "No potential friends found for this category.");
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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
        private async Task<List<UserDto>> GetUserDtosOrThrowAsync(IEnumerable<string> userIds, string notFoundMessage)
        {
            var ids = userIds?.Distinct().ToList() ?? new List<string>();
            if (!ids.Any())
                throw new NotFoundException(notFoundMessage);

            var users = await _userRepository.GetManyByIdsAsync(ids);
            var dtos = users.Select(UserMapper.ToDto).ToList();

            if (!dtos.Any())
                throw new NotFoundException(notFoundMessage);

            return dtos;
        }
    }
}
