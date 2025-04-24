using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NaviriaAPI.Data;
using NaviriaAPI.Options;

namespace NaviriaAPI.Configurations
{
    public static class MongoConfiguration
    {
        public static void ConfigureMongo(this WebApplicationBuilder builder)
        {
            // Configuration
            builder.Services.Configure<MongoDbOptions>(
                builder.Configuration.GetSection("MongoDbSettings"));

            // MongoClient registration
            builder.Services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDbOptions>>().Value;
                return new MongoClient(settings.ConnectionString);
            });

            // Database registration
            builder.Services.AddSingleton(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDbOptions>>().Value;
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(settings.DatabaseName);
            });

            builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();
        }
    }
}

