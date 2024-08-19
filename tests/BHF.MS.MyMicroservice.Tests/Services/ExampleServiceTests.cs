using BHF.MS.MyMicroservice.Models;
using BHF.MS.MyMicroservice.Models.Settings;
using BHF.MS.MyMicroservice.Services;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using FluentAssertions;

namespace BHF.MS.MyMicroservice.Tests.Services
{
    public class ExampleServiceTests
    {
        private readonly HttpClient _httpClient;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        private readonly IOptions<ServiceSettings> _settings = Options.Create(new ServiceSettings
        {
            Endpoint1Uri = "/abc"
        });
        private readonly ExampleService _sut;

        public ExampleServiceTests()
        {
            _httpClient = _httpMessageHandlerMock.CreateClient();
            _httpClient.BaseAddress = new Uri("https://www.test.com");

            _sut = new ExampleService(_httpClient, _settings);
        }

        [Fact]
        public async Task DoSomethingAsync_ReturnsResponse()
        {
            // Arrange
            var model = new ExampleModel { Title = "title", Email = "abc@abc.com", Phone = "123123123" };
            _httpMessageHandlerMock
                .SetupRequest(HttpMethod.Post,
                    new Uri(_httpClient.BaseAddress!, _settings.Value.Endpoint1Uri))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            // Act
            var result = await _sut.DoSomethingAsync(model);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
