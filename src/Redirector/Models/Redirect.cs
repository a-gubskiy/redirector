namespace Redirector.Models;

public record Redirect(string Source, string Destination)
{
    public string Source { get; set; } = Source;

    public string Destination { get; set; } = Destination;

    public bool Match(HostString requestHost, PathString requestPath)
    {
        throw new NotImplementedException();
    }
}