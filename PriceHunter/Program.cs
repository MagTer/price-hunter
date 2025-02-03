using System;
using System.IO;
using System.Text.Json;
using PriceHunter.Configuration;

namespace PriceHunter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Load configuration from config/appsettings.json
            string configPath = Path.Combine("config", "appsettings.json");
            if (!File.Exists(configPath))
            {
                Console.WriteLine("Configuration file not found: " + configPath);
                return;
            }

            string json = File.ReadAllText(configPath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            AppSettings settings = JsonSerializer.Deserialize<AppSettings>(json, options);

            Console.WriteLine("Loaded Configuration:");
            Console.WriteLine($"Stores: {settings.Stores.Count}");
            Console.WriteLine($"Products: {settings.Products.Count}");
            Console.WriteLine($"Notification Method: {settings.Notifications.Method}");

            // Run web scraping simulation
            var scraper = new ScraperService(settings);
            var priceData = scraper.RunScraping();

            // Check prices and send notifications
            var notifier = new NotifierService(settings.Notifications);
            notifier.CheckAndNotify(priceData, settings.Products);

            Console.WriteLine("Process complete. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
