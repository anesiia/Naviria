using NaviriaAPI.IRepositories;
using Microsoft.Extensions.Logging;
using NaviriaAPI.IServices.ICleanupServices;
using NaviriaAPI.Exceptions;
using NaviriaAPI.Helpers;

namespace NaviriaAPI.Services.CleanupServices
{
    /// <summary>
    /// Service for cascading deletion of a folder and all tasks in that folder.
    /// </summary>
    public class FolderCleanupService : IFolderCleanupService
    {
        private readonly IFolderRepository _folderRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<FolderCleanupService> _logger;

        public FolderCleanupService(
            IFolderRepository folderRepository,
            ITaskRepository taskRepository,
            ILogger<FolderCleanupService> logger)
        {
            _folderRepository = folderRepository;
            _taskRepository = taskRepository;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteFolderAndTasksAsync(string folderId)
        {
            var folder = await _folderRepository.GetByIdAsync(folderId);
            if (folder == null)
                throw new NotFoundException($" Folder with ID {folderId} not found.");

            // Delete all tasks in the folder
            await _taskRepository.DeleteManyByFolderIdAsync(folderId);

            // Delete the folder itself
            var deleted = await _folderRepository.DeleteAsync(folderId);

            return deleted;
        }
    }
}
