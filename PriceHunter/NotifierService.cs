using System;
using System.Collections.Generic;
using PriceHunter.Configuration;
using PriceHunter.Notifications;

namespace PriceHunter
{
    public class NotifierService
    {
        private readonly NotificationConfig _config;
        private readonly HomeyNotifier _homeyNotifier;

        public NotifierService(NotificationConfig config)
        {
            _config = config;
            _homeyNotifier = new HomeyNotifier(_config);
        }

        // Checks prices and triggers notification if a price drop is detected
        public void CheckAndNotify(Dictionary<string, decimal> priceData, List<Product> products)
        {
            Console.WriteLine("Checking for price drops...");

            foreach (var product in products)
            {
                if (priceData.TryGetValue(product.ProductId, out decimal currentPrice))
                {
                    decimal thresholdPrice = product.TargetPrice * (1 - _config.PriceDropThreshold / 100);
                    if (currentPrice < thresholdPrice)
                    {
                        string message = $"Price drop for {product.Name}: Current = {currentPrice}, Threshold = {thresholdPrice}";
                        Console.WriteLine(message);
                        // Send notification asynchronously
                        System.Threading.Tasks.Task.Run(async () => await _homeyNotifier.SendNotificationAsync(message));
                    }
                }
            }
        }
    }
}
