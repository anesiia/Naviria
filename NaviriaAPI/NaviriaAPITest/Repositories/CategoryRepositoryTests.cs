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
using NaviriaAPI.Tests.helper;

namespace NaviriaAPI.Tests.Repositories
{

        [TestFixture]
        public class CategoryRepositoryTests : RepositoryTestBase<CategoryEntity>
        {
            private ICategoryRepository _repository = null!;

            [SetUp]
            public override void SetUp()
            {
                base.SetUp();
                _repository = new CategoryRepository(DbContext);
            }

            protected override IMongoCollection<CategoryEntity> GetCollection(IMongoDbContext dbContext)
            {
                return dbContext.Categories;
            }
            [Test]


        public async Task TC001_CreateAsync_ShouldInsertCategory()
        {
            var category = new CategoryEntity { Name = "Health" };
            await _repository.CreateAsync(category);

            var fromDb = await Collection.Find(c => c.Id == category.Id).FirstOrDefaultAsync();

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

            await Collection.InsertManyAsync(categories);

            var result = await _repository.GetAllAsync();

            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result.Any(c => c.Name == "Work"), Is.True);
        }

        [Test]
        public async Task TC003_GetByIdAsync_ShouldReturnCorrectCategory()
        {
            var category = new CategoryEntity { Name = "Fitness" };
            await Collection.InsertOneAsync(category);

            var result = await _repository.GetByIdAsync(category.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("Fitness"));
        }

        [Test]
        public async Task TC004_UpdateAsync_ShouldUpdateCategory()
        {
            var category = new CategoryEntity { Name = "Travel" };
            await Collection.InsertOneAsync(category);

            category.Name = "Adventure";

            var updated = await _repository.UpdateAsync(category);
            var result = await Collection.Find(c => c.Id == category.Id).FirstOrDefaultAsync();

            Assert.That(updated, Is.True);
            Assert.That(result!.Name, Is.EqualTo("Adventure"));
        }

        [Test]
        public async Task TC005_DeleteAsync_ShouldRemoveCategory()
        {
            var category = new CategoryEntity { Name = "DeleteMe" };
            await Collection.InsertOneAsync(category);

            var deleted = await _repository.DeleteAsync(category.Id);
            var result = await Collection.Find(c => c.Id == category.Id).FirstOrDefaultAsync();

            Assert.That(deleted, Is.True);
            Assert.That(result, Is.Null);
        }
    }
}