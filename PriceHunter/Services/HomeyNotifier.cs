using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PriceHunter.Services
{
  public class HomeyNotifier
  {
    private readonly IConfiguration _configuration;
    private readonly ILogger<HomeyNotifier> _logger;
    private readonly HttpClient _httpClient;

    public HomeyNotifier(IConfiguration configuration, ILogger<HomeyNotifier> logger, HttpClient httpClient)
    {
      _configuration = configuration;
      _logger = logger;
      _httpClient = httpClient;
    }

    public async Task SendNotification(string message)
    {
      string homeyApiUrl = _configuration["Homey:ApiUrl"] ?? "";
      string homeyApiKey = _configuration["Homey:ApiKey"] ?? "";

      if (string.IsNullOrEmpty(homeyApiUrl) || string.IsNullOrEmpty(homeyApiKey))
      {
        _logger.LogError("Homey API URL or API Key is not configured.  Please check appsettings.json.");
        return;
      }

      try
      {
        _logger.LogInformation($"Sending notification to Homey: {message}");

        var requestBody = new
        {
          message = message
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {homeyApiKey}");
        var response = await _httpClient.PostAsync(homeyApiUrl, content);

        if (response.IsSuccessStatusCode)
        {
          _logger.LogInformation("Notification sent successfully to Homey.");
        }
        else
        {
          string responseContent = await response.Content.ReadAsStringAsync();
          _logger.LogError($"Failed to send notification to Homey. Status code: {response.StatusCode}, Response: {responseContent}");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error sending notification to Homey.");
      }
    }
  }
}
