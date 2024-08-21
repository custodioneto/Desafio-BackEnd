using Dev.Challenge.Api;
using Microsoft.Extensions.Caching.Memory;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddTransient<Startup>();

        var startup = builder.Services.BuildServiceProvider().GetRequiredService<Startup>();

        startup.ConfigureServices(builder.Services);

        var app = builder.Build();

        var env = app.Services.GetRequiredService<IWebHostEnvironment>();
        var memoryCache = app.Services.GetRequiredService<IMemoryCache>();

        startup.Configure(app, env, memoryCache);

        app.Run();
    }
}

