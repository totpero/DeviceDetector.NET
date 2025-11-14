using Microsoft.Extensions.DependencyInjection;
using Serilog.Configuration;
using Serilog.Enrichers;
using System;

namespace Serilog
{
    public static class DeviceDetectorConfigurationExtensions
    {
        /// <summary>
        /// Enrich log events with Aspnetcore httpContext properties.
        /// </summary>
        /// <param name="enrichmentConfiguration">Logger enrichment configuration.</param>
        /// <param name="serviceProvider"></param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration WithDeviceDetector(this LoggerEnrichmentConfiguration enrichmentConfiguration,
            IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(enrichmentConfiguration);

            return enrichmentConfiguration.With<DeviceDetectorEnricher>();
        }
    }
}
