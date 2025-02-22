using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NaviriaAPI.Entities;

namespace NaviriaAPI.Data
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoDatabase GetDatabase() => _database;
        public IMongoCollection<CategoryEntity> Categories => _database.GetCollection<CategoryEntity>("categories");
    }

}
