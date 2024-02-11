using System.Text.Json;
using Redirector;
using Redirector.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var configFilePath = Environment.GetEnvironmentVariable("CONFIG_FILE_PATH");

if (File.Exists(configFilePath))
{
    builder.Configuration.AddJsonFile("", true);
}

var settings = builder.Configuration.Get<Settings>();

builder.Services.AddSingleton(settings!);
builder.Services.AddSingleton(settings!.Redirects);
builder.Services.AddSingleton<IRedirectRouter, RedirectRouter>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<DebugMiddleware>();
}

app.UseMiddleware<RedirectMiddleware>();

var logger = app.Services.GetService<ILogger<Settings>>()!;

logger.LogInformation("Settings: ");
logger.LogInformation((JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true })));

app.Run();