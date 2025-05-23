using NaviriaAPI.DTOs;
using NaviriaAPI.DTOs.FeaturesDTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.IServices.ISecurityService;
using NaviriaAPI.IServices.IUserServices;
using NaviriaAPI.Mappings;

namespace NaviriaAPI.Services.User
{
    public class FriendService : IFriendService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly ITaskRepository _taskRepository;
        private readonly IMessageSecurityService _messageSecurityService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFriendRequestRepository _friendRequestRepository;

        public FriendService(
            IUserRepository userRepository,
            IUserService userService, 
            IMessageSecurityService messageSecurityService,
            ITaskRepository taskRepository,
            ICategoryRepository categoryRepository,
            IFriendRequestRepository friendRequestRepository)
        {
            _userRepository = userRepository;
            _userService = userService;
            _messageSecurityService = messageSecurityService;
            _taskRepository = taskRepository;
            _categoryRepository = categoryRepository;
            _friendRequestRepository = friendRequestRepository;
        }

        public async Task<IEnumerable<UserDto>> GetUserFriendsAsync(string userId)
        {
            var user = await _userService.GetUserOrThrowAsync(userId);
            var friendIds = user.Friends.Select(f => f.UserId).ToList();
            var friends = await _userRepository.GetManyByIdsAsync(friendIds);

            return friends.Select(UserMapper.ToDto);
        }

        public async Task<bool> DeleteFriendAsync(string fromUserId, string friendId)
        {
            var fromUser = await _userService.GetUserOrThrowAsync(fromUserId);
            var friend = await _userService.GetUserOrThrowAsync(friendId);

            // delete friendship from first user
            fromUser.Friends.RemoveAll(f => f.UserId == friendId);

            // delete friendship from second user
            friend.Friends.RemoveAll(f => f.UserId == fromUserId);

            var updatedFrom = await _userRepository.UpdateAsync(fromUser);
            var updatedFriend = await _userRepository.UpdateAsync(friend);

            return updatedFrom && updatedFriend;
        }

        public async Task<IEnumerable<UserDto>> GetPotentialFriendsAsync(string userId)
        {
            var user = await _userService.GetUserOrThrowAsync(userId);

            // Exclude already friends and yourself
            var exeptionUserIds = user.Friends.Select(f => f.UserId).ToHashSet();
            exeptionUserIds.Add(userId);

            var allUsers = await _userRepository.GetAllAsync();

            // Exclude users with already sent requests
            var outgoingRequests = await _friendRequestRepository.GetBySenderIdAsync(userId);
            var requestedUserIds = outgoingRequests.Select(r => r.ToUserId).ToHashSet();

            exeptionUserIds.UnionWith(requestedUserIds);

            var potentialFriends = allUsers
                .Where(u => !exeptionUserIds.Contains(u.Id))
                .Select(UserMapper.ToDto);

            return potentialFriends;
        }

        public async Task<IEnumerable<UserDto>> SearchUsersByNicknameAsync(string userId, string query)
        {
            var user = await _userService.GetUserOrThrowAsync(userId);
            var excludedIds = user.Friends.Select(f => f.UserId).Append(userId).ToHashSet();

            var allUsers = await _userRepository.GetAllAsync();

            _messageSecurityService.Validate(userId, query);

            var matched = allUsers
                .Where(u => !excludedIds.Contains(u.Id))
                .Where(u => u.Nickname.Contains(query, StringComparison.OrdinalIgnoreCase))
                .OrderBy(u => u.Nickname)
                .Select(UserMapper.ToDto);

            return matched;
        }

        public async Task<SharedInterestsDto> GetSharedInterestsAsync(string userId1, string userId2)
        {
            var user1Tasks = await GetUserTasksAsync(userId1);
            var user2Tasks = await GetUserTasksAsync(userId2);

            var sharedCategoryNames = await GetSharedCategoryNamesAsync(user1Tasks, user2Tasks);
            var sharedTagNames = GetSharedTagNames(user1Tasks, user2Tasks);

            return new SharedInterestsDto
            {
                SharedCategoryNames = sharedCategoryNames,
                SharedTagNames = sharedTagNames
            };
        }

        private async Task<List<TaskEntity>> GetUserTasksAsync(string userId)
        {
            return (await _taskRepository.GetAllByUserAsync(userId)).ToList();
        }

        private async Task<List<string>> GetSharedCategoryNamesAsync(
            List<TaskEntity> user1Tasks, List<TaskEntity> user2Tasks)
        {
            var user1CategoryIds = user1Tasks.Select(t => t.CategoryId).Where(x => !string.IsNullOrEmpty(x)).ToHashSet();
            var user2CategoryIds = user2Tasks.Select(t => t.CategoryId).Where(x => !string.IsNullOrEmpty(x)).ToHashSet();
            var sharedCategoryIds = user1CategoryIds.Intersect(user2CategoryIds).ToList();

            if (sharedCategoryIds.Count == 0)
                return new List<string>();

            var categories = await _categoryRepository.GetManyByIdsAsync(sharedCategoryIds);
            return categories.Select(c => c.Name).ToList();
        }

        private List<string> GetSharedTagNames(List<TaskEntity> user1Tasks, List<TaskEntity> user2Tasks)
        {
            var user1Tags = user1Tasks.SelectMany(t => t.Tags).Select(t => t.TagName).ToHashSet();
            var user2Tags = user2Tasks.SelectMany(t => t.Tags).Select(t => t.TagName).ToHashSet();
            return user1Tags.Intersect(user2Tags).ToList();
        }

    }
}
