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
        _redirects = redirects;
        _logger = logger;
    }

    public async Task<Uri?> Route(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }
        
        foreach (var redirect in _redirects)
        {
            if (redirect.Match(url))
            {
                _logger.LogInformation($"Found redirect rul from {redirect.Source} to {redirect.Destination}");

                return new Uri(redirect.Destination);
            }
        }

        return null;
    }

    public Task<Uri?> Route(Uri url) => Route(url?.ToString() ?? string.Empty);
    
    public Task<Uri?> Route(HttpRequest request)
    {
        return Route($"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}");
    }
}