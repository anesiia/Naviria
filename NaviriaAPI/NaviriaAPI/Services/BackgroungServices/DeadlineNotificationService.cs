using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.Helpers;

namespace NaviriaAPI.Services.BackgroungServices
{
    public class DeadlineNotificationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public DeadlineNotificationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var taskRepository = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                    var now = DateTime.UtcNow;
                    var tomorrow = now.Date.AddDays(1);

                    // Notification in one day before deadline
                    var tasksTomorrow = await taskRepository.GetTasksWithDeadlineOnDateAsync(tomorrow);

                    foreach (var task in tasksTomorrow)
                    {
                        if (task.IsNotificationsOn && task.Deadline.HasValue && task.Deadline.Value.Date == tomorrow)
                        {
                            await notificationService.CreateAsync(new NotificationCreateDto
                            {
                                UserId = task.UserId,
                                Text = $"Завдання \"{task.Title}\" має дедлайн {task.Deadline:dd.MM.yyyy}. Залишилась 1 доба до завершення!",
                                RecievedAt = DateTime.UtcNow
                            });
                        }
                    }

                    // Check for missed deadlines
                    var overdueTasks = await taskRepository.GetOverdueTasksAsync(now);

                    foreach (var task in overdueTasks)
                    {
                        if (task.IsNotificationsOn && task.Deadline.HasValue && task.Deadline.Value < now && task.Status == CurrentTaskStatus.InProgress)
                        {
                            await notificationService.CreateAsync(new NotificationCreateDto
                            {
                                UserId = task.UserId,
                                Text = $"Дедлайн завдання \"{task.Title}\" ({task.Deadline:dd.MM.yyyy}) сплинув!",
                                RecievedAt = DateTime.UtcNow
                            });

                            task.Status = CurrentTaskStatus.DeadlineMissed;
                            await taskRepository.UpdateAsync(task);
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
