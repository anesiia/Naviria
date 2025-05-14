using NaviriaAPI.Helpers;

namespace NaviriaAPI.Configurations
{
    public static class MongoMappingConfiguration
    {
        public static void ConfigureMongoMappings(this IServiceCollection services)
        {
            MongoSubtaskMapping.RegisterSubtaskMappings();
        }
    }
}