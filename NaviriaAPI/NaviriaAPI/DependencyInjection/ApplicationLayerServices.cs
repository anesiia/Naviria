﻿using Microsoft.AspNetCore.Identity;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.IServices.IEmbeddedServices;
using NaviriaAPI.IServices.IGamificationLogic;
using NaviriaAPI.Repositories;
using NaviriaAPI.Services;
using NaviriaAPI.Services.GamificationLogic;
using NaviriaAPI.Services.User;
using NaviriaAPI.Services.AchievementStrategies;
using NaviriaAPI.Services.EmbeddedServices;
using NaviriaAPI.IServices.IUserServices;
using NaviriaAPI.Services.CleanupServices;
using NaviriaAPI.IServices.ICleanupServices;
using NaviriaAPI.Services.StatisticServices;
using NaviriaAPI.IServices.IStatisticServices;

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
            services.AddScoped<IAchievementGranter, AchievementGranter>();

            services.AddScoped<IAchievementStrategy, RegistrationAchievementStrategy>();
            services.AddScoped<IAchievementStrategy, FiveTasksInWeekAchievementStrategy>();
            services.AddScoped<IAchievementStrategy, FirstTaskCompletedAchievementStrategy>();
            services.AddScoped<IAchievementStrategy, LongTaskCompletedAchievementStrategy>();
            services.AddScoped<IAchievementStrategy, FiveFriendsAddedAchievementStrategy>();
            services.AddScoped<IAchievementStrategy, PhotoUploadedAchievementStrategy>();

            services.AddScoped<IAchievementCleanupService, AchievementCleanupService>();
            services.AddScoped<IUserCleanupService, UserCleanupService>();
            services.AddScoped<ICategoryCleanupService, CategoryCleanupService>();
            services.AddScoped<IFolderCleanupService, FolderCleanupService>();

            services.AddScoped<ISupportService, SupportService>();
            services.AddScoped<ISubtaskService, SubtaskService>();
            services.AddScoped<IUserSearchService, UserSearchService>();
            services.AddScoped<ITaskRewardService, TaskRewardService>();

            services.AddScoped<IGeneralStatisticService, GeneralStatisticsService>();
            services.AddScoped<IStatisticRepository, StatisticRepository>();

            services.AddScoped<ITaskStatisticService, RepeatableTaskStatisticService>();
            services.AddScoped<IStatisticsByCategoryService, StatisticsByCategoryService>();
            services.AddScoped<ITaskStatisticByDateService, TaskStatisticByDateService>();
            services.AddScoped<ILeaderboardService, LeaderboardService>();

        }
    }
}

