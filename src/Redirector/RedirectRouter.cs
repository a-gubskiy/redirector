using System.Text.Json;
using System.Text.Json.Serialization;
using Redirector.Models;

namespace Redirector;

public interface IRedirectRouter
{
    Task<Uri?> Route(Uri request);
    Task<Uri?> Route(HttpRequest request);
}

public class RedirectRouter : IRedirectRouter
{
    private readonly IReadOnlyCollection<Redirect> _redirects;
    private readonly ILogger _logger;

    public RedirectRouter(IReadOnlyCollection<Redirect> redirects, ILogger<RedirectRouter> logger)
    {
        _logger = logger;
        _redirects = redirects;

        _logger.LogInformation($"{nameof(RedirectRouter)} created. Redirects:");
        _logger.LogInformation($"{JsonSerializer.Serialize(redirects, new JsonSerializerOptions { WriteIndented = true })}");
    }

    public async Task<Uri?> Route(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            _logger.LogWarning("Empty url detected");
            
            return null;
        }
        
        foreach (var redirect in _redirects)
        {
            if (redirect.Match(url))
            {
                _logger.LogInformation($"Found redirect rule from {redirect.Source} to {redirect.Destination} for {url}");

                return new Uri(redirect.Destination);
            }
        }
        
        _logger.LogWarning($"No redirects found for {url}");

        return null;
    }

    public Task<Uri?> Route(Uri url) => Route(url?.ToString() ?? string.Empty);
    
    public Task<Uri?> Route(HttpRequest request)
    {
        return Route($"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}");
    }
}