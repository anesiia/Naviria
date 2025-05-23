using System.Text.Json.Serialization;
using NaviriaAPI.Helpers.TasksPolymorphism;

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
                options.SerializerOptions.AddPolymorphicTaskConverters();
            });

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.AddPolymorphicSubtaskConverters();
                options.JsonSerializerOptions.AddPolymorphicTaskConverters();
            });
        }
    }
}