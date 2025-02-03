using System;
using System.Collections.Generic;
using PriceHunter.Configuration;

namespace PriceHunter
{
    public class ScraperService
    {
        private readonly AppSettings _settings;

        public ScraperService(AppSettings settings)
        {
            _settings = settings;
        }

        // Simulate web scraping: returns a dictionary with product ID and a random price
        public Dictionary<string, decimal> RunScraping()
        {
            Console.WriteLine("Simulating web scraping for current prices...");
            var random = new Random();
            var prices = new Dictionary<string, decimal>();

            foreach (var product in _settings.Products)
            {
                // Simulate a price fluctuation within ±20% of target price
                double fluctuation = random.NextDouble() * 0.4 - 0.2;
                decimal currentPrice = product.TargetPrice * (1 + (decimal)fluctuation);
                prices[product.ProductId] = Math.Round(currentPrice, 2);
                Console.WriteLine($"Product: {product.Name}, Target: {product.TargetPrice}, Current: {currentPrice}");
            }

            return prices;
        }
    }
}
