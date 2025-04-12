using CloudinaryDotNet;
using NaviriaAPI.Extentions;
using NaviriaAPI.IServices.IAuthServices;
using NaviriaAPI.IServices.IJwtService;
using NaviriaAPI.IServices.ICloudStorage;
using NaviriaAPI.Services.AuthServices;
using NaviriaAPI.Services.CloudStorage;
using NaviriaAPI.Services.JwtTokenService;
using NaviriaAPI.Services.SignalRHub;
using NaviriaAPI.Services.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureJwt();
builder.ConfigureCors();
builder.ConfigureMongo();
builder.ConfigureServices();
builder.ConfigureSwagger();
builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<UserValidationService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();


builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var cloudName = config["Cloudinary:CloudName"];
    var apiKey = config["Cloudinary:ApiKey"];
    var apiSecret = config["Cloudinary:ApiSecret"];

    return new Cloudinary(new Account(cloudName, apiKey, apiSecret));
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles(); //testing static suth file

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapHub<PresenceHub>("/hubs/presence");

app.Run();
