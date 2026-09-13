using System;
using System.IO;
using Npgsql;
using backend.Data;
using Microsoft.EntityFrameworkCore;

AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
{
    var ex = e.ExceptionObject as Exception;
    Console.WriteLine("===== CRASH =====");
    Console.WriteLine($"Message: {ex?.Message}");
    Console.WriteLine($"Type: {ex?.GetType().FullName}");
    Console.WriteLine($"Stack: {ex?.StackTrace}");
    Console.WriteLine($"Inner: {ex?.InnerException?.Message}");
    Console.WriteLine("=================");
    Console.Out.Flush();
};

TaskScheduler.UnobservedTaskException += (sender, e) =>
{
    Console.WriteLine("===== TASK EXCEPTION =====");
    Console.WriteLine($"Message: {e.Exception?.Message}");
    Console.WriteLine($"Stack: {e.Exception?.StackTrace}");
    Console.WriteLine("==========================");
    Console.Out.Flush();
    e.SetObserved();
};

var builder = WebApplication.CreateBuilder(args);

// ===== PORT (Railway / Render ke liye zaroori) =====
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// ===== DATABASE CONNECTION CHECK (optional, sirf log ke liye) =====
try
{
    var connString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connString))
    {
        Console.WriteLine("===== CONNECTION STRING IS EMPTY! =====");
    }
    else
    {
        // Password ko log mein mat print karo — sirf host/db dikhao
        Console.WriteLine("===== CONNECTION STRING FOUND (hidden for security) =====");
        using var connection = new NpgsqlConnection(connString);
        connection.Open();
        Console.WriteLine("===== DATABASE CONNECTED SUCCESSFULLY =====");
    }
}
catch (Exception ex)
{
    Console.WriteLine("===== DATABASE CONNECTION FAILED =====");
    Console.WriteLine($"Message: {ex.Message}");
    Console.WriteLine($"Type: {ex.GetType().FullName}");
    Console.WriteLine($"Inner: {ex.InnerException?.Message}");
    Console.WriteLine("=====================================");
}
Console.Out.Flush();

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ===== AUTO MIGRATION — database tables khud ban jayenge =====
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    Console.WriteLine("===== APPLYING MIGRATIONS =====");
    db.Database.Migrate();
    Console.WriteLine("===== MIGRATIONS APPLIED SUCCESSFULLY =====");
}
catch (Exception ex)
{
    Console.WriteLine("===== MIGRATION FAILED =====");
    Console.WriteLine($"Message: {ex.Message}");
    Console.WriteLine($"Inner: {ex.InnerException?.Message}");
    Console.WriteLine("===========================");
}
Console.Out.Flush();

try
{
    // Swagger hamesha ON — production mein bhi
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors("AllowFrontend");
    app.UseStaticFiles();
    app.UseAuthorization();
    app.MapControllers();

    Console.WriteLine("===== APP STARTED SUCCESSFULLY =====");
    Console.Out.Flush();
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("===== STARTUP CRASH =====");
    Console.WriteLine($"Message: {ex.Message}");
    Console.WriteLine($"Type: {ex.GetType().FullName}");
    Console.WriteLine($"Stack: {ex.StackTrace}");
    Console.WriteLine($"Inner: {ex.InnerException?.Message}");
    Console.WriteLine("=========================");
    Console.Out.Flush();
    throw;
}