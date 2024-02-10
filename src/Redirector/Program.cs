using Redirector;
using Redirector.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<Settings>(p =>
{
    var settings = new Settings();

    settings.Redirects = new List<Redirect>
    {
        new Redirect("gymnasium.kiev.ua", "https://andrew.gubskiy.com/blog/item/gymnasium-kiev-ua/"),
        new Redirect("agi.net.ua", "https://andrew.gubskiy.com/agi")
    };

    return settings;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
}

app.UseMiddleware<RedirectMiddleware>();

app.Run();