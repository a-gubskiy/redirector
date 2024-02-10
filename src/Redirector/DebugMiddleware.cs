using System.Net;
using System.Text.Json;

namespace Redirector;

public class DebugMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Settings _settings;
    private readonly ILogger _logger;

    public DebugMiddleware(RequestDelegate next, Settings settings, ILogger<RedirectMiddleware> logger)
    {
        _next = next;
        _settings = settings;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/debug")
        {
            var json = JsonSerializer.Serialize(_settings);

            await context.Response.WriteAsync(json);
            return;
        }

        await _next.Invoke(context);
    }
}