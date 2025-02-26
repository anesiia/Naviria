using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.Repositories;
using NaviriaAPI.Services;
using NaviriaAPI.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Додаємо User Secrets (тільки в режимі розробки)
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}
// Отримуємо налаштування MongoDB
var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// Додаємо MongoDB в DI-контейнер
builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoDbSettings.ConnectionString));

// Додаємо налаштування для MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Реєструємо `IMongoDatabase`
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var client = new MongoClient(settings.ConnectionString);
    return client.GetDatabase(settings.DatabaseName);
});

// Реєструємо `IMongoDbContext`
builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();

// Додаємо контролери
builder.Services.AddControllers();

// Реєструємо репозиторії та сервіси
builder.Services.AddScoped<IAchivementRepository, AchivementRepository>();
builder.Services.AddScoped<IAchivementService, AchivementService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
builder.Services.AddScoped<IFriendRequestService, FriendRequestService>();
builder.Services.AddScoped<IQuoteRepository, QuoteRepository>();
builder.Services.AddScoped<IQuoteService, QuoteService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Додаємо Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers(); // Реєструє контролери

// Включаємо Swagger у режимі розробки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Використовуємо HTTPS
app.UseHttpsRedirection();


app.Run();
