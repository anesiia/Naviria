using NaviriaAPI.DTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.Helpers;

namespace NaviriaAPI.IServices.IGamificationLogic
{
    /// <summary>
    /// Service for calculating and granting rewards for task completion.
    /// </summary>
    public interface ITaskRewardService
    {
        /// <summary>
        /// Grants completion rewards (experience points, level update) to a user for completing a task.
        /// Updates the task status, calculates reward points based on complexity, priority, and subtasks,
        /// updates the user's LevelInfo and Points, and saves the changes to the database.
        /// </summary>
        /// <param name="task">The task being completed by the user.</param>
        /// <param name="user">The user DTO receiving the reward.</param>
        /// <param name="prevStatus">The previous status of the task.</param>
        /// <param name="newStatus">The new status of the task.</param>
        /// <returns>The amount of experience points (XP) granted for completing the task.</returns>
        Task<int> GrantTaskCompletionRewardsAsync(
            TaskEntity task,
            UserDto user,
            CurrentTaskStatus prevStatus,
            CurrentTaskStatus newStatus);

        /// <summary>
        /// Calculates the amount of experience points (XP) a user can receive for completing a specific task.
        /// Takes into account task complexity, priority, number of subtasks, tags, and additional bonuses.
        /// </summary>
        /// <param name="task">The task for which to calculate the reward.</param>
        /// <returns>The number of experience points (XP).</returns>
        int CalculateTaskPoints(TaskEntity task);
    }
}
