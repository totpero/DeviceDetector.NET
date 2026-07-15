using System;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceDetectorNET.Icons.Matomo
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a singleton <see cref="MatomoIconResolver"/> configured by <paramref name="configureOptions"/>.
        /// </summary>
        public static IServiceCollection AddMatomoDeviceDetectorIcons(this IServiceCollection services, Action<MatomoIconResolverOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            var options = new MatomoIconResolverOptions();
            configureOptions(options);

            services.AddSingleton(new MatomoIconResolver(options));
            return services;
        }
    }
}
