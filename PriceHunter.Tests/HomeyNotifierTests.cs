using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using PriceHunter.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PriceHunter.Tests
{
  public class HomeyNotifierTests
  {
    [Fact]
    public async Task SendNotification_Success()
    {
      // Arrange
      var configurationMock = new Mock<IConfiguration>();
      configurationMock.Setup(c => c["Homey:ApiUrl"]).Returns("http://localhost:3000/api/notification");
      configurationMock.Setup(c => c["Homey:ApiKey"]).Returns("testapikey");

      var loggerMock = new Mock<ILogger<HomeyNotifier>>();
      var httpClient = new HttpClient(new MockHttpMessageHandler(System.Net.HttpStatusCode.OK));

      var homeyNotifier = new HomeyNotifier(configurationMock.Object, loggerMock.Object, httpClient);

      // Act
      await homeyNotifier.SendNotification("Test message");

      // Assert
      // Verify that the log information was called
      loggerMock.Verify(
          x => x.Log(
              LogLevel.Information,
              It.IsAny<EventId>(),
              It.Is<It.IsAnyType>((o, t) => string.Equals("Notification sent successfully to Homey.", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
              It.IsAny<Exception>(),
              It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)!),
          Times.Once);
    }

    [Fact]
    public async Task SendNotification_Failure_InvalidApiKey()
    {
      // Arrange
      var configurationMock = new Mock<IConfiguration>();
      configurationMock.Setup(c => c["Homey:ApiUrl"]).Returns("http://localhost:3000/api/notification");
      configurationMock.Setup(c => c["Homey:ApiKey"]).Returns("invalidapikey");

      var loggerMock = new Mock<ILogger<HomeyNotifier>>();
      var httpClient = new HttpClient(new MockHttpMessageHandler(System.Net.HttpStatusCode.Unauthorized)); // Simulate Unauthorized

      var homeyNotifier = new HomeyNotifier(configurationMock.Object, loggerMock.Object, httpClient);

      // Act
      await homeyNotifier.SendNotification("Test message");

      // Assert
      // Verify that the log Error was called
      loggerMock.Verify(
          x => x.Log(
              LogLevel.Error,
              It.IsAny<EventId>(),
              It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Failed to send notification to Homey"), StringComparison.InvariantCultureIgnoreCase),
              It.IsAny<Exception>(),
              It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)!),
          Times.Once);
    }

    // Mock HTTP message handler for testing
    private class MockHttpMessageHandler : HttpMessageHandler
    {
      private readonly System.Net.HttpStatusCode _statusCode;

      public MockHttpMessageHandler(System.Net.HttpStatusCode statusCode)
      {
        _statusCode = statusCode;
      }

      protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
      {
        return await Task.FromResult(new HttpResponseMessage()
        {
          StatusCode = _statusCode,
          Content = new StringContent("Error message", System.Text.Encoding.UTF8)
        });
      }
    }
  }
}
