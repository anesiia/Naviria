using MongoDB.Bson;
using MongoDB.Driver;
using NaviriaAPI.Data;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Repositories;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using NaviriaAPI.Options;

namespace NaviriaAPI.Tests.Repositories
{
    [TestFixture]
    public class CategoryRepositoryTests
    {
        private IMongoDbContext _dbContext = null!;
        private ICategoryRepository _repository = null!;
        private IMongoCollection<CategoryEntity> _categoriesCollection = null!;
        private string _testId = string.Empty;

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("MongoDbSettings.json")
                .Build();

            var mongoOptions = config.GetSection("MongoDbSettings").Get<MongoDbOptions>();
            var options = Microsoft.Extensions.Options.Options.Create(mongoOptions);

            _dbContext = new MongoDbContext(options);
            _repository = new CategoryRepository(_dbContext);
            _categoriesCollection = _dbContext.Categories;

            _categoriesCollection.DeleteMany(Builders<CategoryEntity>.Filter.Empty);
        }

        [TearDown]
        public void TearDown()
        {
            _categoriesCollection.DeleteMany(Builders<CategoryEntity>.Filter.Empty);
        }

        [Test]
        public async Task TC001_CreateAsync_ShouldInsertCategory()
        {
            var category = new CategoryEntity { Name = "Health" };
            await _repository.CreateAsync(category);

            var fromDb = await _categoriesCollection.Find(c => c.Id == category.Id).FirstOrDefaultAsync();

            Assert.That(fromDb, Is.Not.Null);
            Assert.That(fromDb.Name, Is.EqualTo("Health"));
        }

        [Test]
        public async Task TC002_GetAllAsync_ShouldReturnAllInsertedCategories()
        {
            var categories = new List<CategoryEntity>
            {
                new() { Name = "Work" },
                new() { Name = "Study" }
            };

            await _categoriesCollection.InsertManyAsync(categories);

            var result = await _repository.GetAllAsync();

            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result.Any(c => c.Name == "Work"), Is.True);
        }

        [Test]
        public async Task TC003_GetByIdAsync_ShouldReturnCorrectCategory()
        {
            var category = new CategoryEntity { Name = "Fitness" };
            await _categoriesCollection.InsertOneAsync(category);

            var result = await _repository.GetByIdAsync(category.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("Fitness"));
        }

        [Test]
        public async Task TC004_UpdateAsync_ShouldUpdateCategory()
        {
            var category = new CategoryEntity { Name = "Travel" };
            await _categoriesCollection.InsertOneAsync(category);

            category.Name = "Adventure";

            var updated = await _repository.UpdateAsync(category);
            var result = await _categoriesCollection.Find(c => c.Id == category.Id).FirstOrDefaultAsync();

            Assert.That(updated, Is.True);
            Assert.That(result!.Name, Is.EqualTo("Adventure"));
        }

        [Test]
        public async Task TC005_DeleteAsync_ShouldRemoveCategory()
        {
            var category = new CategoryEntity { Name = "DeleteMe" };
            await _categoriesCollection.InsertOneAsync(category);

            var deleted = await _repository.DeleteAsync(category.Id);
            var result = await _categoriesCollection.Find(c => c.Id == category.Id).FirstOrDefaultAsync();

            Assert.That(deleted, Is.True);
            Assert.That(result, Is.Null);
        }
    }
}