using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PriceHunter.Services
{
  public class NotifierService
  {
    private readonly IConfiguration _configuration;
    private readonly ILogger<NotifierService> _logger;
    private readonly ScraperService _scraperService;
    private readonly HomeyNotifier _homeyNotifier;

    public NotifierService(IConfiguration configuration, ILogger<NotifierService> logger, ScraperService scraperService, HomeyNotifier homeyNotifier)
    {
      _configuration = configuration;
      _logger = logger;
      _scraperService = scraperService;
      _homeyNotifier = homeyNotifier;
    }

    public async Task SendNotifications()
    {
      _logger.LogInformation("Starting notification process...");

      var products = _scraperService.GetProducts();

      foreach (var product in products)
      {
        foreach (var store in product.Stores)
        {
          if (store.CurrentPrice > 0)
          {
            // Example: Send notification if price is below a threshold
            decimal threshold = _configuration.GetValue<decimal>($"NotificationThresholds:{product.Name}");
            if (store.CurrentPrice < threshold)
            {
              _logger.LogInformation($"Price of {product.Name} at {store.Name} is below threshold ({store.CurrentPrice} < {threshold}). Sending notification.");
              await _homeyNotifier.SendNotification($"Price of {product.Name} at {store.Name} is now {store.CurrentPrice} kr!");
            }
            else
            {
              _logger.LogInformation($"Price of {product.Name} at {store.Name} is above threshold ({store.CurrentPrice} >= {threshold}). No notification sent.");
            }
          }
          else
          {
            _logger.LogWarning($"Could not determine price for {product.Name} at {store.Name}. No notification sent.");
          }
        }
      }
    }
  }
}
