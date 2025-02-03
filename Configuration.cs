using System.Collections.Generic;

namespace PriceHunter.Configuration
{
    public class AppSettings
    {
        public List<Store> Stores { get; set; }
        public List<Product> Products { get; set; }
        public NotificationConfig Notifications { get; set; }
    }

    public class Store
    {
        public string StoreId { get; set; }
        public string Name { get; set; }
        public string BaseUrl { get; set; }
        public ScraperConfig ScraperConfig { get; set; }
    }

    public class ScraperConfig
    {
        public string PriceSelector { get; set; }
    }

    public class Product
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public List<string> StoreIds { get; set; }
        public decimal TargetPrice { get; set; }
        public string Identifier { get; set; }
    }

    public class NotificationConfig
    {
        public string Method { get; set; }
        public string HomeyWebhookUrl { get; set; }
        public decimal PriceDropThreshold { get; set; }
    }
}
