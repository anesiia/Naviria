using Microsoft.AspNetCore.Identity;
using NaviriaAPI.DTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.IServices.ICloudStorage;
using NaviriaAPI.IServices.IGamificationLogic;
using NaviriaAPI.IServices.IJwtService;
using NaviriaAPI.Mappings;
using NaviriaAPI.Services.Validation;

namespace NaviriaAPI.Services.User
{
    public class FriendService : IFriendService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public FriendService(IUserRepository userRepository, IUserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
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
            var exeptionUserIds = user.Friends.Select(f => f.UserId).ToList();

            exeptionUserIds.Add(userId);

            var allUsers = await _userRepository.GetAllAsync();
            var potentialFriends = allUsers
                .Where(u => !exeptionUserIds.Contains(u.Id))
                .Select(UserMapper.ToDto);

            return potentialFriends;
        }
    }
}
