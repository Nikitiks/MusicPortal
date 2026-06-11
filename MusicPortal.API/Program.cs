using Microsoft.EntityFrameworkCore;
using MusicPortal.DAL.Data;
using MusicPortal.DAL.Interfaces;
using MusicPortal.DAL.Repositories;
using MusicPortal.BLL.Interfaces;
using MusicPortal.BLL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MusicPortalContext>(options =>
    options.UseSqlServer(connectionString));

// Register repositories and services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<ISongDtoService, SongDtoService>();

// CORS Configuration - CRITICAL для кросс-доменних запитів
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAdminClient", policy =>
    {
        policy.WithOrigins(
            "http://localhost:8080",     // SPA клієнт
            "http://127.0.0.1:8080",
            "http://localhost:5500",     // Live Server (VS Code)
            "http://127.0.0.1:5500"
        )
        .AllowAnyMethod()                // GET, POST, PUT, DELETE
        .AllowAnyHeader()                // Всі заголовки
        .AllowCredentials();             // Cookies, Authorization headers
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// IMPORTANT: CORS має бути перед Authorization
app.UseCors("AllowAdminClient");

app.UseAuthorization();

app.MapControllers();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MusicPortalContext>();
    context.Database.EnsureCreated();
}

app.Run();
