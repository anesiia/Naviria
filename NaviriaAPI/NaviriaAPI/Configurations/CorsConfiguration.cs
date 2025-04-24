namespace NaviriaAPI.Configurations
{
    public static class CorsConfiguration
    {
        public static void ConfigureCors(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                {
                    policy.WithOrigins("https://localhost:5173")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .SetIsOriginAllowed(_ => true)
                          .AllowCredentials();
                });
            });
        }
    }

}
