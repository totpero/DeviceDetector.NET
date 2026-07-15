using System;
using DeviceDetectorNET.Icons.Matomo;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace DeviceDetectorNET.Icons.Matomo.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void RegistersMatomoIconResolverAsSingletonWithConfiguredOptions()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("devices/tv.svg");

            var services = new ServiceCollection();
            services.AddMatomoDeviceDetectorIcons(options => options.PhysicalRootPath = dir.RootPath);

            using var provider = services.BuildServiceProvider();
            var resolver = provider.GetRequiredService<MatomoIconResolver>();

            resolver.GetDeviceType("tv").ShouldBe("/assets/images/devicedetector/devices/tv.svg");
            provider.GetRequiredService<MatomoIconResolver>().ShouldBeSameAs(resolver);
        }

        [Fact]
        public void ThrowsWhenServicesIsNull()
        {
            Should.Throw<ArgumentNullException>(() =>
                ServiceCollectionExtensions.AddMatomoDeviceDetectorIcons(null, options => { }));
        }

        [Fact]
        public void ThrowsWhenConfigureOptionsIsNull()
        {
            var services = new ServiceCollection();

            Should.Throw<ArgumentNullException>(() =>
                services.AddMatomoDeviceDetectorIcons(null));
        }
    }
}
