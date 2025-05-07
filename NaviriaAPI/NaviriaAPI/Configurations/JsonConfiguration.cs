using System.Text.Json.Serialization;
using NaviriaAPI.Helpers;

namespace NaviriaAPI.Configurations
{
    public static class JsonConfiguration
    {
        public static void ConfigureJsonConverters(this IServiceCollection services)
        {
            services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.SerializerOptions.AddPolymorphicSubtaskConverters();
            });

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.AddPolymorphicSubtaskConverters();
            });
        }
    }
}