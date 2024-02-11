using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Extensions;

namespace Redirector.Middlewares;

public class RedirectMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IRedirectRouter _redirectRouter;

    private readonly ILogger _logger;

    public RedirectMiddleware(RequestDelegate next, IRedirectRouter redirectRouter, ILogger<RedirectMiddleware> logger)
    {
        _next = next;
        _redirectRouter = redirectRouter;

        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var requestUrl = context.Request.GetEncodedUrl();
            
            var redirect = await _redirectRouter.Route(context.Request);

            if (redirect != null)
            {
                _logger.LogInformation($"Redirecting from {requestUrl} to {redirect}");

                context.Response.Redirect(redirect.ToString(), true);

                return;
            }

            _logger.LogWarning($"No redirects found for {requestUrl}");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            var json = JsonSerializer.Serialize(new
            {
                RequestUrl = requestUrl,
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