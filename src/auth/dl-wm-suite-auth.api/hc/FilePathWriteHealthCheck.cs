using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace dl.wm.suite.auth.api.hc
{
    public class FilePathWriteHealthCheck : IHealthCheck
    {
        private readonly string _filePath;
        private readonly IReadOnlyDictionary<string, object> _healthCheckData;

        public FilePathWriteHealthCheck(string filePath)
        {
            _filePath = filePath;
            _healthCheckData = new Dictionary<string, object>
            {
                { "filePath", _filePath }
            };
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var networkFile = $"{_filePath}\\hc.txt";
                var fs = File.Create(networkFile);
                fs.Close();
                File.Delete(networkFile);

                return Task.FromResult(HealthCheckResult.Healthy());
            }
            catch (Exception e)
            {

                switch (context.Registration.FailureStatus)
                {
                    case HealthStatus.Degraded:
                        return Task.FromResult(HealthCheckResult.Degraded($"Issues writing to file path", 
                            e, _healthCheckData));
                    case HealthStatus.Healthy:
                        return Task.FromResult(HealthCheckResult.Healthy($"Issues writing to file path", _healthCheckData));
                    default:
                        return Task.FromResult(HealthCheckResult.Unhealthy($"Issues writing to file path", 
                            e, _healthCheckData));
                }
            }
        }
    }
}