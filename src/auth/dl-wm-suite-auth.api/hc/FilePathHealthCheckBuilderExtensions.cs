using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace dl.wm.suite.auth.api.hc
{
    public static class FilePathHealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddFilePathWrite(this IHealthChecksBuilder builder, string filePath,
            HealthStatus failureStatus, IEnumerable<string> tags = default)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }


            return builder.Add(new HealthCheckRegistration(
                "File Path Health Check",
                new FilePathWriteHealthCheck(filePath), 
                failureStatus,
                tags));
        }
    }
}