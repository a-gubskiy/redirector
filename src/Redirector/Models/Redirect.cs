namespace Redirector.Models;

public record Redirect(string Source, string Destination)
{
    public string Source { get; set; } = Source;

    public string Destination { get; set; } = Destination;

    public bool Match(HostString requestHost, PathString requestPath)
    {
        return Match($"{requestHost}{requestPath}");
    }

    public bool Match(string url)
    {
        // Normalize the redirect source and input URL by removing schemes
        var normalizedSource = Source
            .Replace("http://", string.Empty)
            .Replace("https://", string.Empty);

        var normalizedUrl = url
            .Replace("http://", string.Empty)
            .Replace("https://", string.Empty);

        // Check if the redirect source includes a path
        if (normalizedSource.Contains('/'))
        {
            // For matching with path, the start of the URL should match the entire normalized source
            return normalizedUrl.StartsWith(normalizedSource, StringComparison.OrdinalIgnoreCase);
        }
        else
        {
            // If the source does not include a path, compare the domain part only
            var domain = normalizedUrl.Split('/').FirstOrDefault();

            return normalizedSource.Equals(domain, StringComparison.OrdinalIgnoreCase);
        }
    }
}