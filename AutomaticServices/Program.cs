using AutomaticServices.Services.Surf;
using AutomaticServices.Services.Youtube;
using Business.AutomaticServices.Wix;
using DAO.DBConnection;
using Microsoft.Extensions.Options;
using Useful.Service;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{(EnvironmentService.Get() == EnvironmentService.Dev ? $"{Environments.Development}." : "")}json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var appSettings = new XDataDatabaseSettings();
new ConfigureFromConfigurationOptions<XDataDatabaseSettings>(config.GetSection("XDataDatabaseSettings")).Configure(appSettings);

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "XApi Automatic Services";
    })
    .ConfigureServices(services =>
    {
        services.Configure<XDataDatabaseSettings>(config.GetSection(nameof(XDataDatabaseSettings)));
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<XDataDatabaseSettings>>().Value);
        services.AddHostedService(serviceProvider => new SurfAutomaticRecurrenceService(serviceProvider.GetService<ILogger<SurfAutomaticRecurrenceService>>(), appSettings));
        services.AddHostedService(serviceProvider => new WixAutomaticRegisterPostService(serviceProvider.GetService<ILogger<WixAutomaticRegisterPostService>>(), appSettings));
        services.AddHostedService(serviceProvider => new YoutubeAutomaticRegisterPlaylistService(serviceProvider.GetService<ILogger<YoutubeAutomaticRegisterPlaylistService>>(), appSettings));
    })
    .Build();

await host.RunAsync();
