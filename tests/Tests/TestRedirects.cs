using System;
using System.Linq;
using System.Threading.Tasks;
using Redirector.Models;
using Xunit;

namespace Tests;

public class TestRedirects
{
    [Fact]
    public async Task TestRedirect()
    {
        var redirect1 = new Redirect("gymnasium.kiev.ua", "https://andrew.gubskiy.com/blog/item/gymnasium-kiev-ua/");
        var redirect2 = new Redirect("agi.net.ua", "https://andrew.gubskiy.com/agi");
        var redirect3 = new Redirect("http://agi.net.ua/q", "https://andrew.gubskiy.com/agi");
        var redirect4 = new Redirect("torf.tv", "https://torf.bar");

        var result1 = redirect1.Match("https://gymnasium.kiev.ua");
        var result2 = redirect1.Match("http://gymnasium.kiev.ua");
        var result3 = redirect1.Match("http://gymnasium.kiev.ua/x/x/1/2/x/2");
        var result4 = redirect1.Match("gymnasium.kiev.ua/x/x/1/2/x/2");
        var result5 = redirect1.Match("gymnasium.kiev.ua");

        var result6 = redirect2.Match("https://agi.net.ua/quote/1");

        var result7 = redirect3.Match("http://agi.net.ua/q");
        var result8 = redirect3.Match("http://dotnet.city/q/123");

        var result9 = redirect4.Match("https://dotnet.city/");

        Assert.True(result1);
        Assert.True(result2);
        Assert.True(result3);
        Assert.True(result4);
        Assert.True(result5);

        Assert.True(result6);

        Assert.True(result7);
        Assert.False(result8);

        Assert.False(result9);
    }

    private bool Match(Redirect redirect, string url)
    {
        // Normalize the redirect source and input URL by removing schemes
        var normalizedSource = redirect.Source
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