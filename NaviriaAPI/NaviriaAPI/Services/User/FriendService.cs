using Microsoft.AspNetCore.Identity;
using NaviriaAPI.DTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.IServices.ICloudStorage;
using NaviriaAPI.IServices.IGamificationLogic;
using NaviriaAPI.IServices.IJwtService;
using NaviriaAPI.IServices.ISecurityService;
using NaviriaAPI.IServices.IUserServices;
using NaviriaAPI.Mappings;
using NaviriaAPI.Services.Validation;

namespace NaviriaAPI.Services.User
{
    public class FriendService : IFriendService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMessageSecurityService _messageSecurityService;

        public FriendService(
            IUserRepository userRepository,
            IUserService userService, 
            IMessageSecurityService messageSecurityService)
        {
            _userRepository = userRepository;
            _userService = userService;
            _messageSecurityService = messageSecurityService;
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
            // get all system users
            var user = await _userService.GetUserOrThrowAsync(userId);

            // remove users who are already friends of user
            var exeptionUserIds = user.Friends.Select(f => f.UserId).ToList();

            // remove user himself
            exeptionUserIds.Add(userId);

            var allUsers = await _userRepository.GetAllAsync();
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

    }
}
