using NaviriaAPI.Configurations;
using NaviriaAPI.Extentions;
using NaviriaAPI.Services.SignalRHub;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.ConfigureJsonConverters();

builder.ConfigureJwt();
builder.ConfigureCors();
builder.ConfigureMongo();
builder.ConfigureSwagger();
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapHub<PresenceHub>("/hubs/presence");

app.Run();