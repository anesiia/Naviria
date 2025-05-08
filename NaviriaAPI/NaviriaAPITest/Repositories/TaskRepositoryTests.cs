using MongoDB.Driver;
using NaviriaAPI.Data;
using NaviriaAPI.Entities;
using NaviriaAPI.Options;
using NaviriaAPI.Repositories;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NaviriaAPI.Tests.Repositories
{
    [TestFixture]
    public class TaskRepositoryTests
    {
        private IMongoDbContext _dbContext;
        private TaskRepository _taskRepository;
        private IMongoCollection<TaskEntity> _taskCollection;
        private string _testUserId;

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
            _taskRepository = new TaskRepository(_dbContext);
            _taskCollection = _dbContext.GetDatabase().GetCollection<TaskEntity>("tasks");

            _testUserId = ObjectId.GenerateNewId().ToString();

            // Clean test data
            _taskCollection.DeleteMany(t => t.UserId == _testUserId);
        }

        [TearDown]
        public void TearDown()
        {
            _taskCollection.DeleteMany(t => t.UserId == _testUserId);
        }

        [Test]
        public async Task TC001_CreateAsync_ShouldInsertTask()
        {
            var task = GetSampleTask(_testUserId);

            await _taskRepository.CreateAsync(task);

            var inserted = await _taskCollection.Find(t => t.Id == task.Id).FirstOrDefaultAsync();

            Assert.That(inserted, Is.Not.Null);
            Assert.That(inserted.Title, Is.EqualTo(task.Title));
        }

        [Test]
        public async Task TC002_GetByIdAsync_ShouldReturnCorrectTask()
        {
            var task = GetSampleTask(_testUserId);
            await _taskCollection.InsertOneAsync(task);

            var result = await _taskRepository.GetByIdAsync(task.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(task.Id));
        }

        [Test]
        public async Task TC003_GetAllByUserAsync_ShouldReturnOnlyUserTasks()
        {
            var task1 = GetSampleTask(_testUserId);
            var task2 = GetSampleTask(_testUserId);

            await _taskCollection.InsertManyAsync(new[] { task1, task2 });

            var result = await _taskRepository.GetAllByUserAsync(_testUserId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Exactly(2).Items);
        }

        [Test]
        public async Task TC004_UpdateAsync_ShouldUpdateExistingTask()
        {
            var task = GetSampleTask(_testUserId);
            await _taskCollection.InsertOneAsync(task);

            task.Title = "Updated Title";
            var updated = await _taskRepository.UpdateAsync(task);

            var updatedFromDb = await _taskCollection.Find(t => t.Id == task.Id).FirstOrDefaultAsync();

            Assert.That(updated, Is.True);
            Assert.That(updatedFromDb.Title, Is.EqualTo("Updated Title"));
        }

        [Test]
        public async Task TC005_DeleteAsync_ShouldRemoveTask()
        {
            var task = GetSampleTask(_testUserId);
            await _taskCollection.InsertOneAsync(task);

            var deleted = await _taskRepository.DeleteAsync(task.Id);
            var fromDb = await _taskCollection.Find(t => t.Id == task.Id).FirstOrDefaultAsync();

            Assert.That(deleted, Is.True);
            Assert.That(fromDb, Is.Null);
        }

        [Test]
        public async Task TC006_DeleteManyByUserIdAsync_ShouldDeleteAllUserTasks()
        {
            var task1 = GetSampleTask(_testUserId);
            var task2 = GetSampleTask(_testUserId);
            var otherTask = GetSampleTask(ObjectId.GenerateNewId().ToString());

            await _taskCollection.InsertManyAsync(new[] { task1, task2, otherTask });

            await _taskRepository.DeleteManyByUserIdAsync(_testUserId);

            var remaining = await _taskCollection.Find(t => t.UserId == _testUserId).ToListAsync();

            Assert.That(remaining, Is.Empty);
        }

        private TaskEntity GetSampleTask(string userId)
        {
            return new TaskEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UserId = userId,
                FolderId = ObjectId.GenerateNewId().ToString(),
                Title = "Sample Task",
                Description = "Sample Description",
                CategoryId = ObjectId.GenerateNewId().ToString(),
                Priority = 5,
                IsDeadlineOn = false,
                IsShownProgressOnPage = false,
                IsNotificationsOn = false,
                Tags = new List<Entities.EmbeddedEntities.Tags>(),
                Subtasks = new List<Entities.EmbeddedEntities.Subtasks.SubtaskBase>()
            };
        }
    }
}