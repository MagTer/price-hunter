using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Globalization;

namespace PriceHunter.Services
{
  public class ScraperService
  {
    private readonly IConfiguration _configuration;
    private readonly ILogger<ScraperService> _logger;
    private readonly List<Product> _products = new List<Product>();

    public ScraperService(IConfiguration configuration, ILogger<ScraperService> logger)
    {
      _configuration = configuration;
      _logger = logger;
      _products = _configuration.GetSection("Products").Get<List<Product>>() ?? new List<Product>();
    }

    public async Task HuntPrices()
    {
      _logger.LogInformation("Starting price hunting...");

      foreach (var product in _products)
      {
        foreach (var store in product.Stores)
        {
          try
          {
            _logger.LogInformation($"Scraping {product.Name} from {store.Name} at {store.Url}");
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(store.Url);

            // Basic scraping logic - adjust based on actual website structure
            var priceNode = doc.DocumentNode.SelectSingleNode(store.PriceXPath);
            if (priceNode != null)
            {
              decimal price = ParsePrice(priceNode.InnerText, store.Culture);
              _logger.LogInformation($"Found price: {price} for {product.Name} at {store.Name}");
              store.CurrentPrice = price;
            }
            else
            {
              _logger.LogWarning($"Could not find price for {product.Name} at {store.Name} using XPath: {store.PriceXPath}");
            }
          }
          catch (Exception ex)
          {
            _logger.LogError(ex, $"Error scraping {product.Name} from {store.Name}");
          }
        }
      }
    }

    private decimal ParsePrice(string priceText, string cultureInfo)
    {
      // Implement more robust parsing logic here, handling different formats
      priceText = priceText.Replace("kr", "").Trim();
      CultureInfo culture = new CultureInfo(cultureInfo);

      if (decimal.TryParse(priceText, NumberStyles.Any, culture, out decimal price))
      {
        return price;
      }
      return 0;
    }

    public List<Product> GetProducts()
    {
      return _products;
    }
  }

  public class Product
  {
    public string Name { get; set; } = "";
    public List<Store> Stores { get; set; } = new List<Store>();
  }

  public class Store
  {
    public string Name { get; set; } = "";
    public string Url { get; set; } = "";
    public string PriceXPath { get; set; } = "";
    public decimal CurrentPrice { get; set; }
    public string Culture { get; set; } = "sv-SE";
  }
}
