using System.Threading.Tasks;
using PriceHunter.Configuration;
using PriceHunter.Notifications;
using Xunit;

namespace PriceHunter.Tests
{
    public class HomeyNotifierTests
    {
        [Fact]
        public async Task SendNotificationAsync_InvalidUrl_ReturnsFalse()
        {
            // Ange en ogiltig URL för att simulera ett misslyckat anrop
            var config = new NotificationConfig
            {
                Method = "Homey",
                HomeyWebhookUrl = "https://invalid-url",
                PriceDropThreshold = 10
            };

            var notifier = new HomeyNotifier(config);
            bool result = await notifier.SendNotificationAsync("Test message");
            Assert.False(result);
        }
    }
}
