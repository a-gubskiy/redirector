using System.Collections.Immutable;
using Redirector.Models;

namespace Redirector;

public class Settings
{
    public IReadOnlyCollection<Redirect> Redirects { get; set; } = ImmutableArray<Redirect>.Empty;
}