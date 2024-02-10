using System.Text.Json;
using Redirector;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<Settings>(p =>
{
    var settings = new Settings();

    return settings;
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
}

app.Use(Middleware);

async Task Middleware(HttpContext context, Func<Task> next)
{
    var fullUrl =
        $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";

    context.Response.ContentType = "application/json";

    var json = JsonSerializer.Serialize(new
    {
        Query = fullUrl,
        Status = "OK"
    });

    await context.Response.WriteAsync(json);

    await next();
}

app.Run();