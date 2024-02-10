using Redirector;
using Redirector.Models;

var builder = WebApplication.CreateBuilder(args);


var settings = builder.Configuration.Get<Settings>();
builder.Services.AddSingleton<Settings>(settings);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
}

app.UseMiddleware<RedirectMiddleware>();

app.Run();