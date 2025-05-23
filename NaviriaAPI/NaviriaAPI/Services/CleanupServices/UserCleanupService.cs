using CloudinaryDotNet.Actions;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices.ICleanupServices;

namespace NaviriaAPI.Services.CleanupServices
{
    public class UserCleanupService : IUserCleanupService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFolderRepository _folderRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IAssistantChatRepository _assistantChatRepository;
        private readonly ITaskRepository _taskRepository;

        public UserCleanupService(
            IUserRepository userRepository,
            IFolderRepository folderRepository,
            INotificationRepository notificationRepository,
            IFriendRequestRepository friendRequestRepository,
            IAssistantChatRepository assistantChatRepository,
            ITaskRepository taskRepository)
        {
            _userRepository = userRepository;
            _folderRepository = folderRepository;
            _notificationRepository = notificationRepository;
            _friendRequestRepository = friendRequestRepository;
            _assistantChatRepository = assistantChatRepository;
            _taskRepository = taskRepository;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteUserAndRelatedDataAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"User with ID {userId} not found.");
            
            // delete user from other users' friends
            var usersWithFriend = await _userRepository.FindAllHavingFriendAsync(userId);
            foreach (var u in usersWithFriend)
            {
                u.Friends.RemoveAll(f => f.UserId == userId);
                await _userRepository.UpdateAsync(u);
            }

            await _folderRepository.DeleteManyByUserIdAsync(userId);
            await _notificationRepository.DeleteManyByUserIdAsync(userId);
            await _friendRequestRepository.DeleteManyByUserIdAsync(userId);
            await _assistantChatRepository.DeleteManyByUserIdAsync(userId);
            await _taskRepository.DeleteManyByUserIdAsync(userId);

            var deleted = await _userRepository.DeleteAsync(userId);

            return deleted;
        }
    }

}
