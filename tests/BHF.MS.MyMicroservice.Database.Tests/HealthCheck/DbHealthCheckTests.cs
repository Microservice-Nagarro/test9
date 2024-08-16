using BHF.MS.MyMicroservice.Database.Context;
using BHF.MS.MyMicroservice.Database.HealthCheck;
using BHF.MS.MyMicroservice.Database.Models.HealthCheck;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Moq;

namespace BHF.MS.MyMicroservice.Database.Tests.HealthCheck
{
    public sealed class DbHealthCheckTests
    {
        private readonly DbHealthCheck _sut;
        private readonly Mock<CustomDbContext> _contextMock = new();
        private readonly IOptions<HealthCheckSettings> _settings = Options.Create(new HealthCheckSettings
        {
            AttemptsCount = 2
        });

        public DbHealthCheckTests()
        {
            _sut = new DbHealthCheck(_contextMock.Object, _settings);
        }

        [Fact]
        public async Task CheckHealthAsync_WhenSuccess_ReturnsHealthyResult_ShouldCallCanConnectOnce()
        {
            // Arrange
            _contextMock.Setup(x => x.Database.CanConnectAsync(default)).ReturnsAsync(true);

            // Act
            var result = await _sut.CheckHealthAsync(new HealthCheckContext());

            // Assert
            _contextMock.Verify(x => x.Database.CanConnectAsync(default), Times.Once);
            result.Should().NotBeNull();
            result.Status.Should().Be(HealthStatus.Healthy);
        }

        [Fact]
        public async Task CheckHealthAsync_WhenFailure_ReturnsUnhealthyResult_ShouldCallCanConnectTwice()
        {
            // Arrange
            _contextMock.Setup(x => x.Database.CanConnectAsync(default)).ReturnsAsync(false);

            // Act
            var result = await _sut.CheckHealthAsync(new HealthCheckContext());

            // Assert
            _contextMock.Verify(x => x.Database.CanConnectAsync(default), Times.Exactly(2));
            result.Should().NotBeNull();
            result.Status.Should().Be(HealthStatus.Unhealthy);
        }
    }
}
