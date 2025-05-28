using NaviriaAPI.IRepositories;
using NaviriaAPI.Mappings;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IServices;
using NaviriaAPI.IServices.ISecurityService;
using NaviriaAPI.IServices.IUserServices;
using NaviriaAPI.DTOs.User;
using NaviriaAPI.DTOs.FeaturesDTOs;
using NaviriaAPI.Entities;

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
            return await UniversalSearchAsync(userId, categoryId, query, excludeSelf: true, onlyFriends: false);
        }

        /// <inheritdoc />
        public async Task<List<UserDto>> SearchFriendsAsync(string userId, string? categoryId, string? query)
        {
            return await UniversalSearchAsync(userId, categoryId, query, excludeSelf: true, onlyFriends: true);
        }

        /// <inheritdoc />
        public async Task<List<FriendRequestWithUserDto>> SearchIncomingFriendRequestsAsync(string userId, string? categoryId, string? query)
        {
            ValidateQuery(userId, query);

            // 1. Retrieve all incoming friend requests
            var requests = await _friendRequestRepository.GetByReceiverIdAsync(userId);
            var senderIds = requests.Select(r => r.FromUserId).Distinct();

            // 2. Filter by category if needed
            senderIds = await FilterUserIdsByCategoryAsync(senderIds, categoryId);

            // 3. Load user entities for senders
            var senders = await _userRepository.GetManyByIdsAsync(senderIds);

            // 4. Filter users by query if provided
            senders = FilterUsersByQuery(senders, query);

            // 5. Map users for quick access
            var sendersDict = senders.ToDictionary(u => u.Id);

            // 6. Build result list combining friend request and user info
            var result = requests
                .Where(r => sendersDict.ContainsKey(r.FromUserId))
                .Select(r => new FriendRequestWithUserDto
                {
                    Request = FriendRequestMapper.ToDto(r),
                    Sender = UserMapper.ToDto(sendersDict[r.FromUserId])
                })
                .ToList();

            if (!result.Any())
                throw new NotFoundException("No incoming friend requests found.");

            return result;
        }

        /// <summary>
        /// Universal search logic for finding users or friends based on category, query, and filter options.
        /// </summary>
        private async Task<List<UserDto>> UniversalSearchAsync(
            string userId,
            string? categoryId,
            string? query,
            bool excludeSelf,
            bool onlyFriends)
        {
            ValidateQuery(userId, query);

            // 1. Retrieve relevant user IDs (all or by category)
            IEnumerable<string> userIds;
            if (!string.IsNullOrWhiteSpace(categoryId))
                userIds = await _taskRepository.GetUserIdsByCategoryAsync(categoryId);
            else
                userIds = (await _userRepository.GetAllAsync()).Select(u => u.Id);

            // 2. Exclude current user if required
            if (excludeSelf)
                userIds = userIds.Where(id => id != userId);

            // 3. Filter only friends if requested
            if (onlyFriends)
            {
                var me = await _userService.GetUserOrThrowAsync(userId);
                var friendIds = me.Friends.Select(f => f.UserId).ToHashSet();
                userIds = userIds.Where(id => friendIds.Contains(id));
            }

            // 4. Load user entities by filtered IDs
            var users = await _userRepository.GetManyByIdsAsync(userIds.Distinct());

            // 5. Filter users by query (nickname or full name) if provided
            users = FilterUsersByQuery(users, query);

            var dtos = users.Select(UserMapper.ToDto).ToList();

            if (!dtos.Any())
                throw new NotFoundException("No users found.");

            return dtos;
        }

        /// <summary>
        /// Validates the search query for security.
        /// </summary>
        private void ValidateQuery(string userId, string? query)
        {
            if (!string.IsNullOrWhiteSpace(query))
                _messageSecurityService.Validate(userId, query);
        }

        /// <summary>
        /// Filters user IDs by category, if a category is provided.
        /// </summary>
        private async Task<IEnumerable<string>> FilterUserIdsByCategoryAsync(IEnumerable<string> userIds, string? categoryId)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
                return userIds;

            var idsInCategory = (await _taskRepository.GetUserIdsByCategoryAsync(categoryId)).ToHashSet();
            return userIds.Where(id => idsInCategory.Contains(id));
        }

        /// <summary>
        /// Filters users by the search query (nickname or full name).
        /// </summary>
        private static List<UserEntity> FilterUsersByQuery(IEnumerable<UserEntity> users, string? query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return users.ToList();

            return users.Where(u =>
                (!string.IsNullOrWhiteSpace(u.Nickname) && u.Nickname.Contains(query, StringComparison.OrdinalIgnoreCase))
            // Uncomment below to enable full name search:
            // || (!string.IsNullOrWhiteSpace(u.FullName) && u.FullName.Contains(query, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }
    }
}
