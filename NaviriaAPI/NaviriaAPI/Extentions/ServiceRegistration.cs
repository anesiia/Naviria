using Microsoft.AspNetCore.Identity;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.Repositories;
using NaviriaAPI.Services;

namespace NaviriaAPI.Extentions
{
    public static class ServiceRegistration
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAchievementRepository, AchievementRepository>();
            builder.Services.AddScoped<IAchievementService, AchievementService>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
            builder.Services.AddScoped<IFriendRequestService, FriendRequestService>();
            builder.Services.AddScoped<IQuoteRepository, QuoteRepository>();
            builder.Services.AddScoped<IQuoteService, QuoteService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();
        }
    }

}
