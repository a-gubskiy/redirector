using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
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

    [Fact]
    public async Task TestRedirectRouter()
    {
        var redirectRouter = new RedirectRouter(new List<Redirect>
        {
            new Redirect("gymnasium.kiev.ua", "https://andrew.gubskiy.com/blog/item/gymnasium-kiev-ua/"),
            new Redirect("agi.net.ua", "https://andrew.gubskiy.com/agi"),
            new Redirect("http://agi.net.ua/q", "https://andrew.gubskiy.com/agi"),
            new Redirect("torf.tv", "https://torf.bar"),
        }, NullLogger<RedirectRouter>.Instance);


        var result1 = await redirectRouter.Route("https://gymnasium.kiev.ua");
        var result2 = await redirectRouter.Route("http://gymnasium.kiev.ua");
        var result3 = await redirectRouter.Route(new Uri("http://gymnasium.kiev.ua/x/x/1/2/x/2"));
        var result4 = await redirectRouter.Route("gymnasium.kiev.ua/x/x/1/2/x/2");
        var result5 = await redirectRouter.Route("gymnasium.kiev.ua");

        var result6 = await redirectRouter.Route("https://agi.net.ua/quote/1");
        var result7 = await redirectRouter.Route("http://agi.net.ua/q");
        
        var result8 = await redirectRouter.Route("http://dotnet.city/q/123");

        var result9 = await redirectRouter.Route(new Uri("https://dotnet.city/"));
        
        Assert.Equal("https://andrew.gubskiy.com/blog/item/gymnasium-kiev-ua/", result1.ToString());
        Assert.Equal("https://andrew.gubskiy.com/blog/item/gymnasium-kiev-ua/", result2.ToString());
        Assert.Equal("https://andrew.gubskiy.com/blog/item/gymnasium-kiev-ua/", result3.ToString());
        Assert.Equal("https://andrew.gubskiy.com/blog/item/gymnasium-kiev-ua/", result4.ToString());
        Assert.Equal("https://andrew.gubskiy.com/blog/item/gymnasium-kiev-ua/", result5.ToString());
        
        Assert.Equal("https://andrew.gubskiy.com/agi", result6.ToString());
        Assert.Equal("https://andrew.gubskiy.com/agi", result7.ToString());

        Assert.Null(result8);
        
        Assert.Null(result9);
    }
    
    [Fact]
    public async Task TestEmptyRequest()
    {
        var redirectRouter = new RedirectRouter(new List<Redirect>
        {
            new Redirect("gymnasium.kiev.ua", "https://andrew.gubskiy.com/blog/item/gymnasium-kiev-ua/"),
            new Redirect("agi.net.ua", "https://andrew.gubskiy.com/agi"),
            new Redirect("http://agi.net.ua/q", "https://andrew.gubskiy.com/agi"),
            new Redirect("torf.tv", "https://torf.bar"),
        }, NullLogger<RedirectRouter>.Instance);


        var result1 = await redirectRouter.Route(" ");
        var result2 = await redirectRouter.Route("");
        

        Assert.Null(result1);
        
        Assert.Null(result2);
    }
}