using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs;
using NaviriaAPI.DTOs.UdateDTOs;

namespace NaviriaAPI.IServices
{
    public interface IAchivementService
    {
        Task<IEnumerable<AchivementDto>> GetAllAsync();
        Task<AchivementDto?> GetByIdAsync(string id);
        Task<AchivementDto> CreateAsync(AchivementCreateDto achivementDto);
        Task<bool> UpdateAsync(string id, AchivementUpdateDto  achivementDto);
        Task<bool> DeleteAsync(string id);
    }
}
