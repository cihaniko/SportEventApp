using Microsoft.Extensions.Diagnostics.HealthChecks;
using Orleans.Runtime;
using SportsEventApp.GrainInterfaces;

namespace SportEventApp.HealthChecks
{
    public class OrleansSelfCheck : IHealthCheck
    {
        private readonly IClusterClient _client;

        public OrleansSelfCheck(IClusterClient client)
        {
            _client = client;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var pingGrain = _client.GetGrain<IPingGrain>(Guid.NewGuid());
                await pingGrain.Ping();

                return HealthCheckResult.Healthy("Orleans PingGrain çalışıyor.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Orleans PingGrain erişilemedi: " + ex.Message);
            }
        }
    }
}
