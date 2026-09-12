using backend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// EF Core + PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS - allow frontend (any origin during development, including phone on local network)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Swagger / API docs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Note: migrations already applied to the production database (Neon).
// Auto-migrating on every startup uses extra memory, which can crash
// the app on low-RAM free hosting plans — so we skip it here.
// To apply new migrations in the future, run `dotnet ef database update`
// locally against the production connection string instead.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

// Serve uploaded images from wwwroot/uploads
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();