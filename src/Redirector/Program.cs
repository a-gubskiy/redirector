using System.Text.Json;
using Redirector;

var builder = WebApplication.CreateBuilder(args);


var settings = builder.Configuration.Get<Settings>();
builder.Services.AddSingleton<Settings>(settings!);

Console.WriteLine("Settings:");
Console.WriteLine(JsonSerializer.Serialize(settings));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    
}

app.UseMiddleware<RedirectMiddleware>();

app.Run();