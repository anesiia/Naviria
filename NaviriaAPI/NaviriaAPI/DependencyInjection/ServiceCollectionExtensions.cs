using CloudinaryDotNet;
using NaviriaAPI.IServices;
using NaviriaAPI.IServices.IAuthServices;
using NaviriaAPI.IServices.ICloudStorage;
using NaviriaAPI.IServices.IGamificationLogic;
using NaviriaAPI.IServices.IJwtService;
using NaviriaAPI.Options;
using NaviriaAPI.Services;
using NaviriaAPI.Services.AuthServices;
using NaviriaAPI.Services.CloudStorage;
using NaviriaAPI.Services.JwtTokenService;
using NaviriaAPI.Services.User;
using NaviriaAPI.Services.Validation;
using NaviriaAPI.Configurations;
using NaviriaAPI.DependencyInjection;
using NaviriaAPI.IServices.ISecurityService;
using NaviriaAPI.Services.SecurityServices;
using NaviriaAPI.Services.BackupService;
using NaviriaAPI.Services.BackgroungServices;

namespace NaviriaAPI.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration config)
        {
            // Business services
            services.AddApplicationServices();
            services.AddAuthorization();
            services.AddSignalR();
            services.ConfigureMongoMappings();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<IMessageSecurityService, MessageSecurityService>();
            services.AddScoped<ILevelService, LevelService>();
            services.AddScoped<IFriendService, FriendService>();
            services.AddScoped<UserValidationService>();
            services.AddSingleton<BackupManager>();
            services.AddHostedService<WeeklyBackupService>();
            services.AddHostedService<DeadlineNotificationService>();


            services.Configure<JwtOptions>(config.GetSection("Jwt"));
            services.ConfigureCloudinary(config);
        }
    }
}
