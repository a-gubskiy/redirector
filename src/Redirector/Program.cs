using System.Text.Json;
using Redirector;
using Redirector.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration.Get<Settings>();

builder.Services.AddSingleton<Settings>(settings!);

builder.Services.AddSingleton<IRedirectRouter>(p =>
{
    var logger = p.GetService<ILogger<RedirectRouter>>();

    return new RedirectRouter(settings!.Redirects, logger!);
});

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