﻿using NaviriaAPI.Entities;

namespace NaviriaAPI.IRepositories
{
    public interface ICategoryRepository
    {
        Task<List<CategoryEntity>> GetAllAsync();
        Task<CategoryEntity?> GetByIdAsync(string id);
        Task CreateAsync(CategoryEntity category);
        Task<bool> UpdateAsync(CategoryEntity category);
        Task<bool> DeleteAsync(string id);
        Task<CategoryEntity?> GetByNameAsync(string name);
        Task<List<CategoryEntity>> GetManyByIdsAsync(IEnumerable<string> ids);
    }
}
