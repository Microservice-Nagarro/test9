using BHF.MS.MyMicroservice.Models;
using BHF.MS.MyMicroservice.Models.Settings;
using BHF.MS.MyMicroservice.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace BHF.MS.MyMicroservice.HealthCheck
{
    public class ExampleServiceHealthCheck(IExampleService exampleService, ILogger<ExampleServiceHealthCheck> logger, IOptions<HealthCheckSettings> settings) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var model = new ExampleModel
            {
                Email = "test@test.com",
                Title = "title",
                Phone = "123123123"
            };

            for (byte i = 0; i < settings.Value.AttemptsCount; i++)
            {
                var result = await exampleService.DoSomethingAsync(model);
                if (result.IsSuccessStatusCode)
                {
                    return HealthCheckResult.Healthy("Successfully connected to the service");
                }

                logger.LogWarning("Failure status code returned: {StatusCode}", (int)result.StatusCode);
            }

            return HealthCheckResult.Unhealthy("Could not connect to the service");
        }
    }
}
