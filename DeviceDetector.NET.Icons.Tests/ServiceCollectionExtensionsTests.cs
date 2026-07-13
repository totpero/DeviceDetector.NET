using System;
using DeviceDetectorNET.Icons;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace DeviceDetectorNET.Icons.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void RegistersIconResolverAsSingletonWithConfiguredOptions()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("device/type/tv.svg");

            var services = new ServiceCollection();
            services.AddDeviceDetectorIcons(options => options.PhysicalRootPath = dir.RootPath);

            using var provider = services.BuildServiceProvider();
            var resolver = provider.GetRequiredService<IconResolver>();

            resolver.GetDeviceType("tv").ShouldBe("/assets/images/devicedetector/device/type/tv.svg");
            provider.GetRequiredService<IconResolver>().ShouldBeSameAs(resolver);
        }

        [Fact]
        public void ThrowsWhenServicesIsNull()
        {
            Should.Throw<ArgumentNullException>(() =>
                ServiceCollectionExtensions.AddDeviceDetectorIcons(null, options => { }));
        }

        [Fact]
        public void ThrowsWhenConfigureOptionsIsNull()
        {
            var services = new ServiceCollection();

            Should.Throw<ArgumentNullException>(() =>
                services.AddDeviceDetectorIcons(null));
        }
    }
}
