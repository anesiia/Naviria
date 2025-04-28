using Microsoft.Extensions.Options;
using NaviriaAPI.Options;

public static class TestHelpers
{
    public static IOptions<MongoDbOptions> GetMongoDbOptions()
    {
        var mongoDbOptions = new MongoDbOptions
        {
            ConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING"),
            DatabaseName = Environment.GetEnvironmentVariable("MONGO_DATABASE_NAME")
        };

        if (string.IsNullOrEmpty(mongoDbOptions.ConnectionString))
        {
            throw new ArgumentNullException("MongoDB connection string is not set.");
        }

        return Options.Create(mongoDbOptions);
    }

}
