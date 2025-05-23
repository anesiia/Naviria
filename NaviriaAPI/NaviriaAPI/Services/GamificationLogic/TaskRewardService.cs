using NaviriaAPI.Entities.EmbeddedEntities.Subtasks;
using NaviriaAPI.Entities;
using NaviriaAPI.Helpers;
using NaviriaAPI.IServices.IGamificationLogic;
using NaviriaAPI.IServices.IUserServices;
using NaviriaAPI.Mappings;
using NaviriaAPI.DTOs.User;

namespace NaviriaAPI.Services.GamificationLogic
{
    public class TaskRewardService : ITaskRewardService
    {
        private readonly ILevelService _levelService;
        private readonly IUserService _userService;

        public TaskRewardService(ILevelService levelService, IUserService userService)
        {
            _levelService = levelService;
            _userService = userService;
        }

        public async Task<int> GrantTaskCompletionRewardsAsync(
            TaskEntity task,
            UserDto user,
            CurrentTaskStatus prevStatus,
            CurrentTaskStatus newStatus)
        {
            int points = 0;

            if (prevStatus == CurrentTaskStatus.InProgress &&
                (newStatus == CurrentTaskStatus.Completed ||
                 newStatus == CurrentTaskStatus.CompletedInTime ||
                 newStatus == CurrentTaskStatus.CompletedNotInTime))
            {
                points = CalculateTaskPoints(task);

                if (task.IsDeadlineOn)
                {
                    if (task.Deadline.HasValue && task.Deadline.Value > DateTime.UtcNow)
                    {
                        task.Status = CurrentTaskStatus.CompletedInTime;
                        points = (int)(points * 1.5);
                    }
                    else
                    {
                        task.Status = CurrentTaskStatus.CompletedNotInTime;
                        points = 0;
                    }
                }
                else
                {
                    task.Status = CurrentTaskStatus.Completed;
                }

                if (points > 0)
                {
                    var levelProgress = await _levelService.CalculateLevelProgressAsync(user, points);

                    user.LevelInfo = levelProgress;
                    user.Points = user.LevelInfo.TotalXp;

                    var updateDto = UserMapper.ToUpdateDto(user);

                    await _userService.UpdateAsync(user.Id, updateDto);
                }
            }
            else if (prevStatus == CurrentTaskStatus.DeadlineMissed &&
                     (newStatus == CurrentTaskStatus.Completed || newStatus == CurrentTaskStatus.CompletedNotInTime))
            {
                task.Status = CurrentTaskStatus.CompletedNotInTime;
            }
            return points;
        }

        public int CalculateTaskPoints(TaskEntity task)
        {
            int basePoints = 10;
            int priorityBonus = task.Priority * 2;
            int subtaskBonus = task.Subtasks?.Count * 3 ?? 0;
            int tagsBonus = task.Tags?.Select(t => t.TagName).Distinct().Count() ?? 0;

            int checkedInDaysBonus = 0;
            foreach (var subtask in task.Subtasks)
            {
                if (subtask is SubtaskRepeatable repeatable)
                {
                    checkedInDaysBonus += Math.Min(repeatable.CheckedInDays?.Count ?? 0, 10) * 2;
                }
            }

            return basePoints + priorityBonus + subtaskBonus + tagsBonus + checkedInDaysBonus;
        }
    }

}
