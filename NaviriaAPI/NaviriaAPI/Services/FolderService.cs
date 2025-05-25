using NaviriaAPI.DTOs.Folder;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.Mappings;

namespace NaviriaAPI.Services
{
    public class FolderService : IFolderService
    {
        private readonly IFolderRepository _folderRepository;

        public FolderService(IFolderRepository folderRepository)
        {
            _folderRepository = folderRepository;
        }

        public async Task<IEnumerable<FolderDto>> GetAllByUserIdAsync(string userId)
        {
            var folders = await _folderRepository.GetAllByUserIdAsync(userId);
            return folders.Select(FolderMapper.ToDto);
        }

        public async Task<FolderDto?> GetByIdAsync(string id)
        {
            var entity = await _folderRepository.GetByIdAsync(id);
            return entity is null ? null : FolderMapper.ToDto(entity);
        }

        public async Task<FolderDto> CreateAsync(FolderCreateDto dto)
        {
            var entity = FolderMapper.ToEntity(dto.UserId, dto);
            await _folderRepository.CreateAsync(entity);
            return FolderMapper.ToDto(entity);
        }

        public async Task<bool> UpdateAsync(string id, FolderUpdateDto dto)
        {
            var entity = await _folderRepository.GetByIdAsync(id);
            if (entity is null) return false;

            FolderMapper.UpdateEntity(entity, dto);
            return await _folderRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _folderRepository.DeleteAsync(id);
        }
    }
}