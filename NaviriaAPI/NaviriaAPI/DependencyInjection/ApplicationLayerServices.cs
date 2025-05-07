using Microsoft.AspNetCore.Identity;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.IServices.IGamificationLogic;
using NaviriaAPI.Repositories;
using NaviriaAPI.Services;
using NaviriaAPI.Services.GamificationLogic;
using NaviriaAPI.Services.User;
using NaviriaAPI.Services.AchievementStrategies;

namespace NaviriaAPI.DependencyInjection
{
    public static class ApplicationLayerServices
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAchievementRepository, AchievementRepository>();
            services.AddScoped<IAchievementService, AchievementService>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IAssistantChatService, AssistantChatService>();
            services.AddScoped<IAssistantChatRepository, AssistantChatRepository>();

            services.AddScoped<IFolderRepository, FolderRepository>();
            services.AddScoped<IFolderService, FolderService>();

            services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
            services.AddScoped<IFriendRequestService, FriendRequestService>();

            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            services.AddScoped<IQuoteRepository, QuoteRepository>();
            services.AddScoped<IQuoteService, QuoteService>();

            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITaskService, TaskService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();

            services.AddScoped<IAchievementManager, AchievementManager>();
            services.AddScoped<IAchievementStrategy, RegistrationAchievementStrategy>();
            services.AddScoped<IAchievementGranter, AchievementGranter>();


        }
    }
}

