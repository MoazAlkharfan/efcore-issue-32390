// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using ConsoleApp1;
using Microsoft.AspNetCore.Mvc;

WebApplicationBuilder builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
});

builder.Services.AddDbContextFactory<Db>(
    options => options
    .UseSqlite($"Data Source=test.db")
    .UseLazyLoadingProxies());

builder.Services.AddHttpClient();

// Order important
builder.Services.AddHostedService<DataSeeder>();

builder.Services
    .AddHostedService<TagsCacheService>()
    .AddSingleton<ITagsService>(x => x.GetServices<IHostedService>().OfType<TagsCacheService>().First());

// Does http requests to the mapped route after 10 seconds of application start "/"
builder.Services.AddHostedService<ServerRequest>();

var app = builder.Build();

app.Map(
    "/",
    ([FromServices] ITagsService service)
    => service.Tags.Select(x => x.Settings?.FirstOrDefault(x => x.Name == "Test")?.Value ?? x.Label).ToList());

app.Run();
