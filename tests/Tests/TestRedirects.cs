using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Redirector;
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
        var redirect3 = new Redirect("torf.tv", "https://torf.bar");
        
      
        var result1 = Match(redirect1, "https://gymnasium.kiev.ua");
        var result2 = Match(redirect1, "http://gymnasium.kiev.ua");
        var result3 = Match(redirect1, "http://gymnasium.kiev.ua/x/x/1/2/x/2");
        var result4 = Match(redirect1, "gymnasium.kiev.ua/x/x/1/2/x/2");
        var result5 = Match(redirect1, "gymnasium.kiev.ua");
        
        var result6 = Match(redirect2, "https://agi.net.ua/quote/1");
        
        var result7 = Match(redirect3, "https://dotnet.city/");
        
        Assert.True(result1);
        Assert.True(result2);
        Assert.True(result3);
        Assert.True(result4);
        Assert.True(result5);
        
        Assert.True(result6);
        
        Assert.False(result7);
    }

    private bool Match(Redirect redirect, string url)
    {
        // Remove the scheme (http, https) from the URL
        var strippedUrl = url
            .Replace("http://", "")
            .Replace("https://", "");

        // Remove the path, if any, only keeping the domain part
        var domain = strippedUrl.Split('/').FirstOrDefault();

        // Compare the domain of the URL with the Source of the redirect
        var result = redirect.Source.Equals(domain, StringComparison.OrdinalIgnoreCase);
        
        return result;
    }
}