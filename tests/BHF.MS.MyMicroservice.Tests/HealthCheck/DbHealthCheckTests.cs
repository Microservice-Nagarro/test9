using System.Net;
using BHF.MS.MyMicroservice.HealthCheck;
using BHF.MS.MyMicroservice.Models;
using BHF.MS.MyMicroservice.Models.Settings;
using BHF.MS.MyMicroservice.Services;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace BHF.MS.MyMicroservice.Tests.HealthCheck
{
    public sealed class ExampleServiceHealthCheckTests
    {
        private readonly ExampleServiceHealthCheck _sut;
        private readonly Mock<IExampleService> _exampleServiceMock = new();
        private readonly Mock<ILogger<ExampleServiceHealthCheck>> _loggerMock = new();
        private readonly IOptions<HealthCheckSettings> _settings = Options.Create(new HealthCheckSettings
        {
            AttemptsCount = 2
        });

        public ExampleServiceHealthCheckTests()
        {
            _sut = new ExampleServiceHealthCheck(_exampleServiceMock.Object, _loggerMock.Object, _settings);
        }

        [Fact]
        public async Task CheckHealthAsync_WhenSuccess_ReturnsHealthyResult_ShouldCallDoSomethingAsyncOnce()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            _exampleServiceMock.Setup(x => x.DoSomethingAsync(It.IsAny<ExampleModel>())).ReturnsAsync(responseMessage);

            // Act
            var result = await _sut.CheckHealthAsync(new HealthCheckContext());

            // Assert
            _exampleServiceMock.Verify(x => x.DoSomethingAsync(It.IsAny<ExampleModel>()), Times.Once);
            result.Should().NotBeNull();
            result.Status.Should().Be(HealthStatus.Healthy);
        }

        [Fact]
        public async Task CheckHealthAsync_WhenFailure_LogsFailureStatusCodes_ReturnsUnhealthyResult_ShouldCallCanConnectTwice()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            _exampleServiceMock.Setup(x => x.DoSomethingAsync(It.IsAny<ExampleModel>())).ReturnsAsync(responseMessage);

            // Act
            var result = await _sut.CheckHealthAsync(new HealthCheckContext());

            // Assert
            _exampleServiceMock.Verify(x => x.DoSomethingAsync(It.IsAny<ExampleModel>()), Times.Exactly(2));
            _loggerMock.VerifyLog(x => x.LogWarning("Failure status code returned: {StatusCode}", (int)responseMessage.StatusCode), Times.Exactly(2));
            result.Should().NotBeNull();
            result.Status.Should().Be(HealthStatus.Unhealthy);
        }

        [Fact]
        public async Task CheckHealthAsync_WhenAttemptsCountEquals0_ReturnsUnhealthyResult()
        {
            // Assert
            _settings.Value.AttemptsCount = 0;

            // Act
            var result = await _sut.CheckHealthAsync(new HealthCheckContext());

            // Assert
            result.Should().NotBeNull();
            _exampleServiceMock.Verify(x => x.DoSomethingAsync(It.IsAny<ExampleModel>()), Times.Never);
            result.Status.Should().Be(HealthStatus.Unhealthy);
        }
    }
}
