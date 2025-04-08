using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NaviriaAPI.Data;

namespace NaviriaAPI.Extentions
{
    public static class MongoConfiguration
    {
        public static void ConfigureMongo(this WebApplicationBuilder builder)
        {
            var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
            builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
            builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoDbSettings.ConnectionString));
            builder.Services.AddSingleton<IMongoDatabase>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                var client = new MongoClient(settings.ConnectionString);
                return client.GetDatabase(settings.DatabaseName);
            });
            builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();
        }
    }

}
