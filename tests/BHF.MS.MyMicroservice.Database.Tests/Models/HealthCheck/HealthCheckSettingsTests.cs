using BHF.MS.MyMicroservice.Database.Models.HealthCheck;
using FluentAssertions;

namespace BHF.MS.MyMicroservice.Database.Tests.Models.HealthCheck
{
    public sealed class HealthCheckSettingsTests
    {
        [Fact]
        public void Constructor_DefaultValuesAreSet()
        {
            // Arrange
            var expectedResult = new HealthCheckSettings { AttemptsCount = 1 };

            // Act
            var sut = new HealthCheckSettings();

            // Assert
            sut.Should().BeEquivalentTo(expectedResult);
        }
    }
}
