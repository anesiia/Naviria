using NaviriaAPI.Exceptions;
using NaviriaAPI.Helpers;
using NaviriaAPI.IServices;
using NaviriaAPI.IServices.IGamificationLogic;
using NaviriaAPI.Services.AchievementStrategies;

namespace NaviriaAPI.Services.GamificationLogic
{
    /// <summary>
    /// Main manager for achievements. Responsible for triggering strategies upon specific events and granting achievements.
    /// </summary>
    public class AchievementManager : IAchievementManager
    {
        private readonly IEnumerable<IAchievementStrategy> _strategies;
        private readonly IAchievementGranter _granter;
        private readonly ILogger<AchievementManager> _logger;

        public AchievementManager(
            IEnumerable<IAchievementStrategy> strategies,
            IAchievementGranter granter,
            ILogger<AchievementManager> logger)
        {
            _strategies = strategies;
            _granter = granter;
            _logger = logger;
        }

        /// <summary>
        /// Checks if the achievement condition is met for the specified trigger and grants the achievement if appropriate.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="trigger">The trigger (event that occurred).</param>
        /// <param name="context">Additional context, such as a task ID.</param>
        public async Task EvaluateAsync(string userId, AchievementTrigger trigger, object? context = null)
        {
            var strategy = _strategies.FirstOrDefault(s => s.Trigger == trigger);
            if (strategy == null) return;

            var achievementIds = await strategy.GetAchievementIdsAsync(userId, context);

            foreach (var id in achievementIds)
            {
                try
                {
                    await _granter.GiveAsync(userId, id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to give achievement with ID {0} to user with ID {1}", id, userId);
                }
            }
        }
    }

}
