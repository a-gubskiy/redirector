using System.Collections.Generic;
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
        
        var settings = new Settings
        {
            Redirects = new List<Redirect>
            {
                redirect1,
                redirect2,
                redirect3
            }
        };

        var result1 = redirect1.Match("https://gymnasium.kiev.ua");
        var result2 = redirect1.Match("https://agi.net.ua/quote/1");
        var result3 = redirect1.Match("https://dotnet.city/");
        
        Assert.True(result1);
        Assert.True(result2);
        Assert.False(result3);
    }
}