using BHF.MS.MyMicroservice.Database.Context;
using BHF.MS.MyMicroservice.Database.Models.HealthCheck;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace BHF.MS.MyMicroservice.Database.HealthCheck
{
    public class DbHealthCheck(CustomDbContext dbContext, IOptions<HealthCheckSettings> settings) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            for (byte i = 0; i < settings.Value.AttemptsCount; i++)
            {
                if (await dbContext.Database.CanConnectAsync(cancellationToken))
                {
                    return HealthCheckResult.Healthy("Successfully connected to the database");
                }
            }

            return HealthCheckResult.Unhealthy("Could not connect to the database");
        }
    }
}
