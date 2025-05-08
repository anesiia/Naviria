using MongoDB.Driver;
using NaviriaAPI.Data;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Repositories;
using NaviriaAPI.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace NaviriaAPI.Tests.Repositories
{
    [TestFixture]
    public class FolderRepositoryTests
    {
        private IMongoDbContext _dbContext;
        private IFolderRepository _folderRepository;
        private IMongoCollection<FolderEntity> _folderCollection;

        [SetUp]
        public void SetUp()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("MongoDbSettings.json")
                .Build();

            var mongoDbOptions = configuration.GetSection("MongoDbSettings").Get<MongoDbOptions>();
            var options = Microsoft.Extensions.Options.Options.Create(mongoDbOptions);

            _dbContext = new MongoDbContext(options);
            _folderRepository = new FolderRepository(_dbContext);
            _folderCollection = _dbContext.Folders;

            // Очистити колекцію перед кожним тестом
            _folderCollection.DeleteMany(Builders<FolderEntity>.Filter.Empty);
        }

        [TearDown]
        public void TearDown()
        {
            _folderRepository = null;
        }

        [Test]
        public async Task TC001_CreateAsync_ShouldInsertFolder()
        {
            var folder = new FolderEntity
            {
                Name = "Test Folder",
                UserId = ObjectId.GenerateNewId().ToString()
            };

            await _folderRepository.CreateAsync(folder);

            var inserted = await _folderCollection.Find(f => f.Id == folder.Id).FirstOrDefaultAsync();
            Assert.That(inserted, Is.Not.Null);
            Assert.That(inserted.Name, Is.EqualTo("Test Folder"));
        }

        [Test]
        public async Task TC002_GetByIdAsync_ShouldReturnCorrectFolder()
        {
            var folder = new FolderEntity
            {
                Name = "Folder A",
                UserId = ObjectId.GenerateNewId().ToString(),
            };

            await _folderCollection.InsertOneAsync(folder);

            var result = await _folderRepository.GetByIdAsync(folder.Id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Folder A"));
        }

        [Test]
        public async Task TC003_GetByIdAsync_InvalidId_ShouldReturnNull()
        {
            var result = await _folderRepository.GetByIdAsync(ObjectId.GenerateNewId().ToString());
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TC004_GetAllByUserIdAsync_ShouldReturnUserFolders()
        {
            var userId = ObjectId.GenerateNewId().ToString();

            var folders = new[]
            {
                new FolderEntity { Name = "F1", UserId = userId },
                new FolderEntity { Name = "F2", UserId = userId },
                new FolderEntity { Name = "Other", UserId = ObjectId.GenerateNewId().ToString() }
            };

            await _folderCollection.InsertManyAsync(folders);

            var result = await _folderRepository.GetAllByUserIdAsync(userId);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.All(f => f.UserId == userId));
        }

        [Test]
        public async Task TC005_UpdateAsync_ShouldModifyFolder()
        {
            var folder = new FolderEntity
            {
                Name = "Old Name",
                UserId = ObjectId.GenerateNewId().ToString()
            };

            await _folderCollection.InsertOneAsync(folder);

            folder.Name = "Updated Name";
            var updated = await _folderRepository.UpdateAsync(folder);

            Assert.That(updated, Is.True);

            var fetched = await _folderCollection.Find(f => f.Id == folder.Id).FirstOrDefaultAsync();
            Assert.That(fetched.Name, Is.EqualTo("Updated Name"));
        }

        [Test]
        public async Task TC006_UpdateAsync_InvalidId_ShouldReturnFalse()
        {
            var folder = new FolderEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Does Not Exist",
                UserId = ObjectId.GenerateNewId().ToString()
            };

            var result = await _folderRepository.UpdateAsync(folder);
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task TC007_DeleteAsync_ShouldRemoveFolder()
        {
            var folder = new FolderEntity
            {
                Name = "To Delete",
                UserId = ObjectId.GenerateNewId().ToString()
            };

            await _folderCollection.InsertOneAsync(folder);

            var result = await _folderRepository.DeleteAsync(folder.Id);
            Assert.That(result, Is.True);

            var check = await _folderCollection.Find(f => f.Id == folder.Id).FirstOrDefaultAsync();
            Assert.That(check, Is.Null);
        }

        [Test]
        public async Task TC008_DeleteAsync_InvalidId_ShouldReturnFalse()
        {
            var result = await _folderRepository.DeleteAsync(ObjectId.GenerateNewId().ToString());
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task TC009_DeleteManyByUserIdAsync_ShouldRemoveAllUserFolders()
        {
            var userId = ObjectId.GenerateNewId().ToString();

            var folders = new[]
            {
        new FolderEntity { Name = "F1", UserId = userId },
        new FolderEntity { Name = "F2", UserId = userId },
        new FolderEntity { Name = "F3", UserId = ObjectId.GenerateNewId().ToString() } // інший користувач
    };

            await _folderCollection.InsertManyAsync(folders);

            await _folderRepository.DeleteManyByUserIdAsync(userId);

            var remaining = await _folderCollection.Find(f => f.UserId == userId).ToListAsync();
            Assert.That(remaining.Count, Is.EqualTo(0));

            var others = await _folderCollection.Find(f => f.UserId != userId).ToListAsync();
            Assert.That(others.Count, Is.EqualTo(1));
        }

    }
}