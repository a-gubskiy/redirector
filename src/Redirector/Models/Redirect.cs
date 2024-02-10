namespace Redirector.Models;

public class Redirect
{
    public string Source { get; set; }
    
    public string Destination { get; set; }
    

    public Redirect()
        : this(string.Empty, string.Empty)
    {
    }

    public Redirect(string source, string destination)
    {
        Source = source;
        Destination = destination;
    }

    public bool Match(Uri uri) => Match(uri.ToString());

    public bool Match(HostString host, PathString path) => Match($"{host}{path}");
    
    public bool Match(string url)
    {
        if (this.Source.Contains(url))
        {
            return true;
        }

        return false;
    }
}