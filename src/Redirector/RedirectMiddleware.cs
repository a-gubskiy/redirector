using System.Net;
using System.Text.Json;

namespace Redirector;

public class RedirectMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Settings _settings;
    private readonly ILogger _logger;

    public RedirectMiddleware(RequestDelegate next, Settings settings, ILogger<RedirectMiddleware> logger)
    {
        _next = next;
        _settings = settings;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var request = context.Request;
            
            foreach (var redirect in _settings.Redirects)
            {
                if (redirect.Match(request.Host, request.Path))
                {
                    _logger.LogInformation($"Redirecting from {redirect.Source} to {redirect.Destination}");
                    
                    context.Response.StatusCode = (int)HttpStatusCode.MovedPermanently;
                    context.Response.Headers.Location = redirect.Destination;

                    return;
                }
            }
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            
            var json = JsonSerializer.Serialize(new
            {
                RequestUrl = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}",
                Status = "Redirect not found",
            });
            
            await context.Response.WriteAsync(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during process request");
            
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            
            await context.Response.WriteAsync("Error during process request");
        }
    }
}