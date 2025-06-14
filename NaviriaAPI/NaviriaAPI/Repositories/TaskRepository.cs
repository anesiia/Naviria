﻿using MongoDB.Bson;
using MongoDB.Driver;
using NaviriaAPI.Data;
using NaviriaAPI.Entities;
using NaviriaAPI.Entities.EmbeddedEntities.Subtasks;
using NaviriaAPI.Helpers;
using NaviriaAPI.IRepositories;

namespace NaviriaAPI.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IMongoCollection<TaskEntity> _tasks;

        public TaskRepository(IMongoDbContext dbContext)
        {
            _tasks = dbContext.Tasks;
        }

        public async Task<TaskEntity?> GetByIdAsync(string id)
        {
            return await _tasks.Find(t => t.Id == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<TaskEntity>> GetByUserIdsAsync(IEnumerable<string> userIds)
        {
            return await _tasks.Find(t => userIds.Contains(t.UserId)).ToListAsync();
        }

        public async Task<IEnumerable<TaskEntity>> GetAllAsync()
        {
            return await _tasks.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<TaskEntity>> GetAllByUserAsync(string userId)
        {
            return await _tasks
                .Find(t => t.UserId == userId)
                .SortByDescending(t => t.Priority)
                .ToListAsync();
        }


        public async Task CreateAsync(TaskEntity entity)
        {
            await _tasks.InsertOneAsync(entity);
        }

        public async Task<bool> UpdateAsync(TaskEntity entity)
        {
            var result = await _tasks.ReplaceOneAsync(t => t.Id == entity.Id, entity);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _tasks.DeleteOneAsync(t => t.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task DeleteManyByUserIdAsync(string userId)
        {
            var filter = Builders<TaskEntity>.Filter.Eq(t => t.UserId, userId);
            await _tasks.DeleteManyAsync(filter);
        }

        public async Task DeleteManyByCategoryIdAsync(string categoryId)
        {
            var filter = Builders<TaskEntity>.Filter.Eq(t => t.CategoryId, categoryId);
            await _tasks.DeleteManyAsync(filter);
        }

        public async Task DeleteManyByFolderIdAsync(string folderId)
        {
            var filter = Builders<TaskEntity>.Filter.Eq(t => t.FolderId, folderId);
            await _tasks.DeleteManyAsync(filter);
        }

        public async Task<IEnumerable<TaskEntity>> GetTasksWithDeadlineOnDateAsync(DateTime deadlineDate)
        {
            var from = deadlineDate.Date;
            var to = from.AddDays(1);

            var filter = Builders<TaskEntity>.Filter.And(
                Builders<TaskEntity>.Filter.Eq(t => t.IsDeadlineOn, true),
                Builders<TaskEntity>.Filter.Eq(t => t.IsNotificationsOn, true),
                Builders<TaskEntity>.Filter.Gte(t => t.Deadline, from),
                Builders<TaskEntity>.Filter.Lt(t => t.Deadline, to)
            );

            return await _tasks.Find(filter).ToListAsync();
        }
        public async Task<List<string>> GetUserIdsByCategoryAsync(string categoryId)
        {
            var filter = Builders<TaskEntity>.Filter.Eq(t => t.CategoryId, categoryId);
            var cursor = await _tasks.DistinctAsync<ObjectId>("user_id", filter);
            var objectIds = await cursor.ToListAsync();
            return objectIds.Select(x => x.ToString()).ToList();
        }

        public async Task<List<TaskEntity>> GetOverdueTasksAsync(DateTime now)
        {
            var filter = Builders<TaskEntity>.Filter.And(
                Builders<TaskEntity>.Filter.Lt(t => t.Deadline, now),
                Builders<TaskEntity>.Filter.Eq(t => t.Status, CurrentTaskStatus.InProgress)
            );

            return await _tasks.Find(filter).ToListAsync();
        }

        public async Task<List<SubtaskRepeatable>> GetAllRepeatableSubtasksByUserAsync(string userId)
        {
            var filter = Builders<TaskEntity>.Filter.Eq(t => t.UserId, userId);
            var projection = Builders<TaskEntity>.Projection.Include(t => t.Subtasks);

            var tasks = await _tasks.Find(filter).Project<TaskEntity>(projection).ToListAsync();
            var repeatable = new List<SubtaskRepeatable>();

            foreach (var task in tasks)
            {
                repeatable.AddRange(task.Subtasks.OfType<SubtaskRepeatable>());
            }
            return repeatable;
        }

        public async Task<IEnumerable<TaskEntity>> GetCompletedTasksInLastNDaysAsync(string userId, int days)
        {
            var dateThreshold = DateTime.UtcNow.AddDays(-days);

            var filter = Builders<TaskEntity>.Filter.And(
                Builders<TaskEntity>.Filter.Eq(t => t.UserId, userId),
                Builders<TaskEntity>.Filter.Eq(t => t.Status, CurrentTaskStatus.Completed),
                Builders<TaskEntity>.Filter.Gte(t => t.CompletedAt, dateThreshold)
            );

            return await _tasks.Find(filter).ToListAsync();
        }

        public async Task<int> GetCompletedTasksCountAsync(string userId)
        {
            var filter = Builders<TaskEntity>.Filter.And(
                Builders<TaskEntity>.Filter.Eq(t => t.UserId, userId),
                Builders<TaskEntity>.Filter.Eq(t => t.Status, CurrentTaskStatus.Completed)
            );

            return (int)await _tasks.CountDocumentsAsync(filter);
        }

    }
}