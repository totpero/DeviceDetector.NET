using System;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceDetectorNET.Icons
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a singleton <see cref="IconResolver"/> configured by <paramref name="configureOptions"/>.
        /// </summary>
        public static IServiceCollection AddDeviceDetectorIcons(this IServiceCollection services, Action<IconResolverOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            var options = new IconResolverOptions();
            configureOptions(options);

            services.AddSingleton(new IconResolver(options));
            return services;
        }
    }
}
