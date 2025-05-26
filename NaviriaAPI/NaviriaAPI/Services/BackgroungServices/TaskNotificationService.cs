using NaviriaAPI.DTOs.Notification;
using NaviriaAPI.Helpers;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;

namespace NaviriaAPI.Services.BackgroungServices
{
    public class TaskNotificationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public TaskNotificationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                    var now = DateTime.UtcNow;

                    var tasks = await taskRepo.GetAllAsync();

                    var tasksToNotify = tasks
                        .Where(t =>
                            t.IsNotificationsOn
                            && t.NotificationDate.HasValue
                            && t.NotificationDate.Value <= now
                            && !t.NotificationSent
                            && t.Status != CurrentTaskStatus.Completed
                            && t.Status != CurrentTaskStatus.DeadlineMissed);

                    foreach (var task in tasksToNotify)
                    {
                        await notificationService.CreateAsync(new NotificationCreateDto
                        {
                            UserId = task.UserId,
                            Text = $"Нагадування: настав час виконати задачу \"{task.Title}\"!",
                            RecievedAt = now
                        });

                        task.NotificationSent = true;
                        await taskRepo.UpdateAsync(task);
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
            }
        }
    }

}
