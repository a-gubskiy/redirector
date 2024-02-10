using Redirector.Models;

namespace Redirector;

public class Settings
{
    public IReadOnlyCollection<Redirect> Redirects = new List<Redirect>();

    public static IReadOnlyCollection<Redirect> LoadRedirects()
    {
        throw new NotImplementedException();
    }
}