using NaviriaAPI.IRepositories;
using NaviriaAPI.Mappings;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IServices;
using NaviriaAPI.IServices.ISecurityService;
using NaviriaAPI.IServices.IUserServices;
using NaviriaAPI.DTOs.User;

namespace NaviriaAPI.Services.User
{
    public class UserSearchService : IUserSearchService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IUserService _userService;
        private readonly IMessageSecurityService _messageSecurityService;

        public UserSearchService(
            ITaskRepository taskRepository,
            IUserRepository userRepository,
            IFriendRequestRepository friendRequestRepository,
            IUserService userService,
            IMessageSecurityService messageSecurityService)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _friendRequestRepository = friendRequestRepository;
            _userService = userService;
            _messageSecurityService = messageSecurityService;
        }

        /// <inheritdoc />
        public async Task<List<UserDto>> SearchAllUsersAsync(string userId, string? categoryId, string? query)
        {
            return await UniversalSearchAsync(
                userId,
                categoryId,
                query,
                excludeSelf: true,
                onlyFriends: false,
                onlyIncomingRequests: false);
        }

        /// <inheritdoc />
        public async Task<List<UserDto>> SearchFriendsAsync(string userId, string? categoryId, string? query)
        {
            return await UniversalSearchAsync(
                userId,
                categoryId,
                query,
                excludeSelf: true,
                onlyFriends: true,
                onlyIncomingRequests: false);
        }

        /// <inheritdoc />
        public async Task<List<UserDto>> SearchIncomingFriendRequestsAsync(string userId, string? categoryId, string? query)
        {
            return await UniversalSearchAsync(
                userId,
                categoryId,
                query,
                excludeSelf: true,
                onlyFriends: false,
                onlyIncomingRequests: true);
        }

        /// <summary>
        /// Universal filtering logic for searching users, friends, or incoming requests
        /// based on category, name, and filter options.
        /// </summary>
        private async Task<List<UserDto>> UniversalSearchAsync(
            string userId,
            string? categoryId,
            string? query,
            bool excludeSelf,
            bool onlyFriends,
            bool onlyIncomingRequests)
        {
            // Validate the query to prevent dangerous content
            if (!string.IsNullOrWhiteSpace(query))
            {
                _messageSecurityService.Validate(userId, query);
            }

            // Get the list of relevant user IDs
            IEnumerable<string> userIds;

            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                userIds = await _taskRepository.GetUserIdsByCategoryAsync(categoryId);
            }
            else
            {
                userIds = (await _userRepository.GetAllAsync()).Select(u => u.Id);
            }

            // Exclude the current user if requested
            if (excludeSelf)
                userIds = userIds.Where(id => id != userId);

            // Filter to only friends if requested
            if (onlyFriends)
            {
                var me = await _userService.GetUserOrThrowAsync(userId);
                var friendIds = me.Friends.Select(f => f.UserId).ToHashSet();
                userIds = userIds.Where(id => friendIds.Contains(id));
            }

            // Filter to only users who sent a friend request, if requested
            if (onlyIncomingRequests)
            {
                var incomingRequests = await _friendRequestRepository.GetByReceiverIdAsync(userId);
                var requesterIds = incomingRequests.Select(r => r.FromUserId).ToHashSet();
                userIds = userIds.Where(id => requesterIds.Contains(id));
            }

            // Load user entities by filtered IDs
            var users = await _userRepository.GetManyByIdsAsync(userIds.Distinct());

            // Filter users by query (nickname or full name) if provided
            if (!string.IsNullOrWhiteSpace(query))
            {
                users = users.Where(u =>
                    (!string.IsNullOrWhiteSpace(u.Nickname) && u.Nickname.Contains(query, StringComparison.OrdinalIgnoreCase)) 
                    //uncomment to turn on search by fullname
                    // || (!string.IsNullOrWhiteSpace(u.FullName) && u.FullName.Contains(query, StringComparison.OrdinalIgnoreCase)) 
                ).ToList();
            }

            var dtos = users.Select(UserMapper.ToDto).ToList();

            if (!dtos.Any())
                throw new NotFoundException("No users found.");

            return dtos;
        }
    }
}
