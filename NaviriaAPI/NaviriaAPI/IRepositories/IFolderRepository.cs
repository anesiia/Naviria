using NaviriaAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaviriaAPI.IRepositories
{
    public interface IFolderRepository
    {
        Task<FolderEntity?> GetByIdAsync(string id);
        Task<IEnumerable<FolderEntity>> GetAllByUserIdAsync(string userId);
        Task CreateAsync(FolderEntity folder);
        Task<bool> UpdateAsync(FolderEntity folder);
        Task<bool> DeleteAsync(string id);
        Task DeleteManyByUserIdAsync(string userId);
    }
}