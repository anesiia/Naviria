using NaviriaAPI.Extentions;
using NaviriaAPI.Services.SignalRHub;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureJwt();
builder.ConfigureCors();
builder.ConfigureMongo();
builder.ConfigureServices();
builder.ConfigureSwagger();
builder.Services.AddControllers();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapHub<PresenceHub>("/hubs/presence");

app.Run();
