using CloudinaryDotNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NaviriaAPI.Options;

namespace NaviriaAPI.Configurations
{
    public static class CloudinaryConfiguration
    {
        public static void ConfigureCloudinary(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CloudinaryOptions>(config.GetSection("Cloudinary"));

            services.AddSingleton(sp =>
            {
                var options = sp.GetRequiredService<IOptions<CloudinaryOptions>>().Value;
                return new Cloudinary(new Account(options.CloudName, options.ApiKey, options.ApiSecret));
            });
        }
    }
}
