using System;
using System.IO;
using System.Text.Json;
using PriceHunter.Configuration;
using System.Collections.Generic;

namespace PriceHunter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Ladda konfigurationen från config/appsettings.json
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

            // Simulera webb-scraping för att hämta aktuella priser
            var scraper = new ScraperService(settings);
            Dictionary<string, decimal> priceData = scraper.RunScraping();

            // Kontrollera priser och notifiera vid prisnedgång
            var notifier = new NotifierService(settings.Notifications);
            notifier.CheckAndNotify(priceData, settings.Products);

            Console.WriteLine("Process complete. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
