using NaviriaAPI.DTOs.Folder;

namespace NaviriaAPI.IServices
{
    public interface IFolderService
    {
        Task<IEnumerable<FolderDto>> GetAllByUserIdAsync(string userId);
        Task<FolderDto?> GetByIdAsync(string id);
        Task<FolderDto> CreateAsync(FolderCreateDto dto);
        Task<bool> UpdateAsync(string id, FolderUpdateDto dto);
        Task<bool> DeleteAsync(string id);
    }
}
