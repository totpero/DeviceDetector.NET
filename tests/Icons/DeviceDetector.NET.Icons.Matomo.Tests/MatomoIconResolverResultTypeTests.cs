using DeviceDetectorNET.Icons.Matomo;
using DeviceDetectorNET.Parser.Device;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Client;
using DeviceDetectorNET.Results.Device;
using Shouldly;
using Xunit;

namespace DeviceDetectorNET.Icons.Matomo.Tests
{
    public class MatomoIconResolverResultTypeTests
    {
        [Fact]
        public void GetBrowserOverloadUsesShortName()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("browsers/FF.png");

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions { PhysicalRootPath = dir.RootPath });
            var browser = new BrowserMatchResult { Name = "Firefox", ShortName = "FF" };

            resolver.GetBrowser(browser).ShouldBe("/assets/images/devicedetector/browsers/FF.png");
        }

        [Fact]
        public void GetOsOverloadUsesShortName()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("os/WIN.png");

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions { PhysicalRootPath = dir.RootPath });
            var os = new OsMatchResult { Name = "Windows 10", ShortName = "WIN" };

            resolver.GetOs(os).ShouldBe("/assets/images/devicedetector/os/WIN.png");
        }

        [Fact]
        public void GetBrandOverloadUsesFullBrandNameNotShortCode()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("brand/Apple.svg");

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions { PhysicalRootPath = dir.RootPath });
            var device = new DeviceMatchResult { Brand = "Apple", Type = DeviceType.DEVICE_TYPE_SMARTPHONE };

            resolver.GetBrand(device).ShouldBe("/assets/images/devicedetector/brand/Apple.svg");
        }

        [Fact]
        public void GetBrandOverloadDoesNotFallBackToShortCodeWhenOnlyFullNameFileExists()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("brand/Apple.svg");

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions { PhysicalRootPath = dir.RootPath });

            // Apple's real DeviceDetector short code is "AP" (see Devices.cs). Proving GetBrand never
            // tries the short code confirms brand lookups are genuinely full-name-only, not accidentally
            // short-code-based (a distinction the plan's design explicitly calls load-bearing).
            resolver.GetBrand("AP").ShouldBe("/assets/images/devicedetector/Matomo.svg");
        }

        [Fact]
        public void GetDeviceTypeOverloadResolvesNameFromIntCodeAndConvertsToSnakeCase()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("devices/car_browser.svg");

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetDeviceType((int?)DeviceType.DEVICE_TYPE_CAR_BROWSER)
                .ShouldBe("/assets/images/devicedetector/devices/car_browser.svg");
        }

        [Fact]
        public void GetBotOverloadAlwaysReturnsFallback()
        {
            using var dir = new TempIconDirectory();

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions { PhysicalRootPath = dir.RootPath });
            var bot = new BotMatchResult { Name = "Googlebot", Category = "Search bot" };

            resolver.GetBot(bot).ShouldBe("/assets/images/devicedetector/Matomo.svg");
        }

        [Fact]
        public void GetIconsUsesBrowserIconWhenClientIsABrowser()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("browsers/FF.png");

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions { PhysicalRootPath = dir.RootPath });
            var result = new DeviceDetectorResult
            {
                Client = new BrowserMatchResult { Name = "Firefox", ShortName = "FF" }
            };

            resolver.GetIcons(result).ClientIcon.ShouldBe("/assets/images/devicedetector/browsers/FF.png");
        }

        [Fact]
        public void GetIconsFallsBackForNonBrowserClientsSinceMatomoIconsHasNoClientTypeFolders()
        {
            using var dir = new TempIconDirectory();

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions { PhysicalRootPath = dir.RootPath });
            var result = new DeviceDetectorResult
            {
                Client = new ClientMatchResult { Name = "Feedly", Type = "feed reader" }
            };

            resolver.GetIcons(result).ClientIcon.ShouldBe("/assets/images/devicedetector/Matomo.svg");
        }

        [Fact]
        public void GetIconsBundlesAllFivePaths()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("os/WIN.png");
            dir.CreateFile("browsers/FF.png");
            dir.CreateFile("brand/Apple.svg");
            dir.CreateFile("devices/smartphone.svg");

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions { PhysicalRootPath = dir.RootPath });
            var result = new DeviceDetectorResult
            {
                Os = new OsMatchResult { Name = "Windows 10", ShortName = "WIN" },
                Client = new BrowserMatchResult { Name = "Firefox", ShortName = "FF" },
                Device = new DeviceMatchResult { Brand = "Apple", Type = DeviceType.DEVICE_TYPE_SMARTPHONE }
            };

            var icons = resolver.GetIcons(result);

            icons.BotIcon.ShouldBeNull();
            icons.OsIcon.ShouldBe("/assets/images/devicedetector/os/WIN.png");
            icons.ClientIcon.ShouldBe("/assets/images/devicedetector/browsers/FF.png");
            icons.BrandIcon.ShouldBe("/assets/images/devicedetector/brand/Apple.svg");
            icons.DeviceTypeIcon.ShouldBe("/assets/images/devicedetector/devices/smartphone.svg");
        }

        [Fact]
        public void GetIconsIncludesBotIconWhenResultIsBot()
        {
            using var dir = new TempIconDirectory();

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions { PhysicalRootPath = dir.RootPath });
            var result = new DeviceDetectorResult
            {
                Bot = new BotMatchResult { Name = "Googlebot", Category = "Search bot" }
            };

            resolver.GetIcons(result).BotIcon.ShouldBe("/assets/images/devicedetector/Matomo.svg");
        }
    }
}
