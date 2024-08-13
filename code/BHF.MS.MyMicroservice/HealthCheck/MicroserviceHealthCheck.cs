using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BHF.MS.MyMicroservice.HealthCheck
{
    public class MicroserviceHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(HealthCheckResult.Healthy("Put a description here"));
        }
    }
}
