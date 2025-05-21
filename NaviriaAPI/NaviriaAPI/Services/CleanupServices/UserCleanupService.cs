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
        private readonly ILogger<UserCleanupService> _logger;

        public UserCleanupService(
            IUserRepository userRepository,
            IFolderRepository folderRepository,
            INotificationRepository notificationRepository,
            IFriendRequestRepository friendRequestRepository,
            IAssistantChatRepository assistantChatRepository,
            ILogger<UserCleanupService> logger,
            ITaskRepository taskRepository)
        {
            _userRepository = userRepository;
            _folderRepository = folderRepository;
            _notificationRepository = notificationRepository;
            _friendRequestRepository = friendRequestRepository;
            _assistantChatRepository = assistantChatRepository;
            _logger = logger;
            _taskRepository = taskRepository;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteUserAndRelatedDataAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", userId);
                return false;
            }

            try
            {
                await _folderRepository.DeleteManyByUserIdAsync(userId);
                await _notificationRepository.DeleteManyByUserIdAsync(userId);
                await _friendRequestRepository.DeleteManyByUserIdAsync(userId);
                await _assistantChatRepository.DeleteManyByUserIdAsync(userId);
                await _taskRepository.DeleteManyByUserIdAsync(userId);

                var deleted = await _userRepository.DeleteAsync(userId);

                if (!deleted)
                {
                    _logger.LogError("Failed to delete user with ID {UserId}", userId);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during cascade deletion for user {UserId}", userId);
                throw;
            }
        }
    }

}
