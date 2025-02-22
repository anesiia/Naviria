using MongoDB.Driver;
using NaviriaAPI.Entities;

namespace NaviriaAPI.Data
{
    public interface IMongoDbContext
    {
        IMongoDatabase GetDatabase();
        IMongoCollection<CategoryEntity> Categories { get; }
    }

}
