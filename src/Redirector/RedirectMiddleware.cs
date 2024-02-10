using System.Net;
using System.Text.Json;

namespace Redirector;

public class RedirectMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Settings _settings;

    public RedirectMiddleware(RequestDelegate next, Settings settings)
    {
        _next = next;
        _settings = settings; // Now you have access to your settings
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var fullUrl = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";

        context.Response.ContentType = "application/json";

        foreach (var redirect in _settings.Redirects)
        {
            if (redirect.Match(context.Request.Host, context.Request.Path))
            {
                context.Response.StatusCode = (int)HttpStatusCode.MovedPermanently;
                context.Response.Headers.Location = redirect.Destination;
                
                return;
            }
        }

        var json = JsonSerializer.Serialize(new
        {
            context.Request.Host,
            context.Request.Path,
            Status = "Redirect not found",
        });

        context.Response.StatusCode = (int)HttpStatusCode.NotFound;

        await context.Response.WriteAsync(json);
    }
}