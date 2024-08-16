using BHF.MS.MyMicroservice.Database.Context;
using BHF.MS.MyMicroservice.Database.HealthCheck;
using BHF.MS.MyMicroservice.Database.Models.HealthCheck;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Moq;

namespace BHF.MS.MyMicroservice.Database.Tests.HealthCheck
{
    public sealed class DbHealthCheckTests
    {
        private readonly DbHealthCheck _sut;
        private readonly Mock<CustomDbContext> _contextMock = new();
        private readonly Mock<DatabaseFacade> _dbFacadeMock;
        private readonly IOptions<HealthCheckSettings> _settings = Options.Create(new HealthCheckSettings
        {
            AttemptsCount = 2
        });

        public DbHealthCheckTests()
        {
            _dbFacadeMock = new Mock<DatabaseFacade>(_contextMock.Object);
            _contextMock.SetupGet(x => x.Database).Returns(_dbFacadeMock.Object);
            _sut = new DbHealthCheck(_contextMock.Object, _settings);
        }

        [Fact]
        public async Task CheckHealthAsync_WhenSuccess_ReturnsHealthyResult_ShouldCallCanConnectOnce()
        {
            // Arrange
            _dbFacadeMock.Setup(x => x.CanConnectAsync(default)).ReturnsAsync(true, TimeSpan.FromMilliseconds(100));

            // Act
            var result = await _sut.CheckHealthAsync(new HealthCheckContext());

            // Assert
            _dbFacadeMock.Verify(x => x.CanConnectAsync(default), Times.Once);
            result.Should().NotBeNull();
            result.Status.Should().Be(HealthStatus.Healthy);
        }

        [Fact]
        public async Task CheckHealthAsync_WhenFailure_ReturnsUnhealthyResult_ShouldCallCanConnectTwice()
        {
            // Arrange
            _dbFacadeMock.Setup(x => x.CanConnectAsync(default)).ReturnsAsync(false, TimeSpan.FromMilliseconds(100));

            // Act
            var result = await _sut.CheckHealthAsync(new HealthCheckContext());

            // Assert
            _dbFacadeMock.Verify(x => x.CanConnectAsync(default), Times.Exactly(2));
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
            result.Status.Should().Be(HealthStatus.Unhealthy);
        }
    }
}
