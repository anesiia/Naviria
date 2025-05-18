using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NaviriaAPI.Data;
using NaviriaAPI.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace NaviriaAPI.Tests.helper
{
    public abstract class RepositoryTestBase<TEntity>
        where TEntity : class
    {
        protected IMongoDbContext DbContext { get; private set; } = null!;
        protected IMongoCollection<TEntity> Collection { get; private set; } = null!;

        [SetUp]
        public virtual void SetUp()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("MongoDbSettings.json")
                .Build();

            var mongoDbOptions = configuration.GetSection("MongoDbSettings").Get<MongoDbOptions>();
            var options = Microsoft.Extensions.Options.Options.Create(mongoDbOptions);

            DbContext = new MongoDbContext(options);
            Collection = GetCollection(DbContext);

            // Очищуємо колекцію до запуску тесту
            Collection.DeleteMany(Builders<TEntity>.Filter.Empty);
        }

        [TearDown]
        public virtual void TearDown()
        {
            // Очищення колекції після тесту (можна, щоб уникнути побічних ефектів)
            Collection.DeleteMany(Builders<TEntity>.Filter.Empty);
        }

        // Метод для отримання колекції, реалізується у конкретних тестах
        protected abstract IMongoCollection<TEntity> GetCollection(IMongoDbContext dbContext);
    }

}