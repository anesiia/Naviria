using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NaviriaAPI.Extentions;
using NaviriaAPI.IServices.IAuthServices;
using NaviriaAPI.IServices.IJwtService;
using NaviriaAPI.Services.AuthServices;
using NaviriaAPI.Services.JwtTokenService;
using NaviriaAPI.Services.SignalRHub;
using NaviriaAPI.Services.Validation;
using System.Text;

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


var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

//app.UseStaticFiles(); //testing static suth file

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapHub<PresenceHub>("/hubs/presence");

app.Run();
