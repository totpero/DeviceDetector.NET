using DeviceDetectorNET.Icons.Matomo;
using Shouldly;
using Xunit;

namespace DeviceDetectorNET.Icons.Matomo.Tests
{
    public class MatomoIconResolverLookupTests
    {
        [Fact]
        public void GetBrowserResolvesByShortCode()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("browsers/FF.png");

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetBrowser("FF").ShouldBe("/assets/images/devicedetector/browsers/FF.png");
        }

        [Fact]
        public void GetBrowserFallsStraightToGlobalFallbackWhenShortCodeMissing()
        {
            using var dir = new TempIconDirectory();

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetBrowser("ZZ").ShouldBe("/assets/images/devicedetector/Matomo.svg");
        }

        [Fact]
        public void GetOsResolvesByShortCode()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("os/WIN.png");

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetOs("WIN").ShouldBe("/assets/images/devicedetector/os/WIN.png");
        }

        [Fact]
        public void GetOsFallsStraightToGlobalFallbackWhenShortCodeMissing()
        {
            using var dir = new TempIconDirectory();

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetOs("ZZZ").ShouldBe("/assets/images/devicedetector/Matomo.svg");
        }

        [Fact]
        public void GetBrandResolvesByFullNameNotShortCode()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("brand/Apple.svg");

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetBrand("Apple").ShouldBe("/assets/images/devicedetector/brand/Apple.svg");
        }

        [Fact]
        public void GetBrandFallsStraightToGlobalFallbackWhenBrandMissing()
        {
            using var dir = new TempIconDirectory();

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetBrand("SomeUnknownBrand").ShouldBe("/assets/images/devicedetector/Matomo.svg");
        }

        [Fact]
        public void GetDeviceTypeResolvesSingleWordTypeDirectly()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("devices/smartphone.svg");

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetDeviceType("smartphone").ShouldBe("/assets/images/devicedetector/devices/smartphone.svg");
        }

        [Fact]
        public void GetDeviceTypeConvertsMultiWordTypeToSnakeCase()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("devices/car_browser.svg");

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetDeviceType("car browser").ShouldBe("/assets/images/devicedetector/devices/car_browser.svg");
        }

        [Fact]
        public void GetBotAlwaysReturnsFallbackSinceMatomoIconsHasNoBotCategory()
        {
            using var dir = new TempIconDirectory();

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetBot("Googlebot", "Search bot").ShouldBe("/assets/images/devicedetector/Matomo.svg");
        }
    }
}
