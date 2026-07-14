using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Configuration;
using Serilog.Enrichers;
using System;

namespace Serilog
{
    public static class DeviceDetectorConfigurationExtensions
    {
        /// <summary>
        /// Enrich log events with device, client, OS, brand and model information parsed from the
        /// current ASP.NET Core request's User-Agent header.
        /// </summary>
        /// <param name="enrichmentConfiguration">Logger enrichment configuration.</param>
        /// <param name="serviceProvider">The application's service provider, used to resolve the registered <see cref="IHttpContextAccessor" />.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration WithDeviceDetector(this LoggerEnrichmentConfiguration enrichmentConfiguration,
            IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(enrichmentConfiguration);
            ArgumentNullException.ThrowIfNull(serviceProvider);

            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            return enrichmentConfiguration.With(new DeviceDetectorEnricher(httpContextAccessor));
        }
    }
}
