using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Serilog;
using PriceHunter.Services;
using System.Threading.Tasks;
using System;

namespace PriceHunter
{
  class Program
  {
    static async Task Main(string[] args)
    {
      // Configuration
      var configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("config/appsettings.json", optional: false, reloadOnChange: true)
          .Build();

      // Logging
      Log.Logger = new LoggerConfiguration()
          .ReadFrom.Configuration(configuration)
          .CreateLogger();

      // Dependency Injection
      var serviceProvider = new ServiceCollection()
          .AddSingleton<IConfiguration>(configuration)
          .AddSingleton<ScraperService>()
          .AddSingleton<NotifierService>()
          .AddTransient<HomeyNotifier>()
          .AddHttpClient<HomeyNotifier>() // Register HttpClient
          .AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger))
          .BuildServiceProvider();

      // Resolve services
      var scraperService = serviceProvider.GetService<ScraperService>();
      var notifierService = serviceProvider.GetService<NotifierService>();

      try
      {
        // Run the price hunting process
        await scraperService.HuntPrices();
        await notifierService.SendNotifications();
      }
      catch (Exception ex)
      {
        Log.Error(ex, "An error occurred during the price hunting process.");
      }
      finally
      {
        Log.CloseAndFlush();
      }
    }
  }
}
