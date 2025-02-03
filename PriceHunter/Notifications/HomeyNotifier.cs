using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PriceHunter.Configuration;

namespace PriceHunter.Notifications
{
    public class HomeyNotifier
    {
        private readonly NotificationConfig _config;
        private readonly HttpClient _httpClient;

        public HomeyNotifier(NotificationConfig config)
        {
            _config = config;
            _httpClient = new HttpClient();
        }

        public async Task<bool> SendNotificationAsync(string message)
        {
            var payload = new { text = message };
            string jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(_config.HomeyWebhookUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Notification sent successfully: {message}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to send notification. Status code: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while sending notification: {ex.Message}");
                return false;
            }
        }
    }
}
