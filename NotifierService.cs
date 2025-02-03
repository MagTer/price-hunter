using System;
using System.Collections.Generic;
using PriceHunter.Configuration;

namespace PriceHunter
{
    public class NotifierService
    {
        private readonly NotificationConfig _config;

        public NotifierService(NotificationConfig config)
        {
            _config = config;
        }

        // Kontrollerar priserna och loggar en notifiering om ett pris har sjunkit mer än angiven tröskel
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
                        Console.WriteLine($"Price drop detected for {product.Name}: Current Price = {currentPrice}, Threshold = {thresholdPrice}");
                        // Här skulle du anropa en metod för att skicka en notifiering via t.ex. en HTTP POST till _config.HomeyWebhookUrl
                        // Exempel: SendNotification(product, currentPrice);
                    }
                }
            }
        }
    }
}
