using DeviceDetectorNET.Icons;
using DeviceDetectorNET.Parser.Device;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Client;
using DeviceDetectorNET.Results.Device;
using Shouldly;
using Xunit;

namespace DeviceDetectorNET.Icons.Tests
{
    public class IconResolverResultTypeTests
    {
        [Fact]
        public void GetBotOverloadUsesNameThenCategory()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("bot/category/Search bot.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });
            var bot = new BotMatchResult { Name = "UnknownBot", Category = "Search bot" };

            resolver.GetBot(bot).ShouldBe("/assets/images/devicedetector/bot/category/Search bot.svg");
        }

        [Fact]
        public void GetBotOverloadHandlesNull()
        {
            using var dir = new TempIconDirectory();

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetBot((BotMatchResult)null).ShouldBe("/assets/images/devicedetector/Matomo.svg");
        }

        [Fact]
        public void GetBrowserOverloadUsesNameFamilyEngine()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("client/browser/family/Chrome.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });
            var browser = new BrowserMatchResult { Name = "Headless Chrome", Family = "Chrome", Engine = "Blink" };

            resolver.GetBrowser(browser).ShouldBe("/assets/images/devicedetector/client/browser/family/Chrome.svg");
        }

        [Fact]
        public void GetClientOverloadDispatchesToBrowserWhenClientIsBrowserMatchResult()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("client/browser/Vivaldi.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });
            ClientMatchResult client = new BrowserMatchResult { Name = "Vivaldi", Type = "browser" };

            resolver.GetClient(client).ShouldBe("/assets/images/devicedetector/client/browser/Vivaldi.svg");
        }

        [Fact]
        public void GetClientOverloadUsesGenericClientForNonBrowserTypes()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("client/feed reader/Feedly.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });
            var client = new ClientMatchResult { Name = "Feedly", Type = "feed reader" };

            resolver.GetClient(client).ShouldBe("/assets/images/devicedetector/client/feed reader/Feedly.svg");
        }

        [Fact]
        public void GetOsOverloadUsesNameThenFamily()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("client/os/family/Linux.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });
            var os = new OsMatchResult { Name = "Arch Linux", Family = "Linux" };

            resolver.GetOs(os).ShouldBe("/assets/images/devicedetector/client/os/family/Linux.svg");
        }

        [Fact]
        public void GetBrandOverloadResolvesDeviceTypeNameFromIntCode()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("device/type/smartphone.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });
            var device = new DeviceMatchResult { Brand = "SomeUnknownBrand", Type = DeviceType.DEVICE_TYPE_SMARTPHONE };

            resolver.GetBrand(device).ShouldBe("/assets/images/devicedetector/device/type/smartphone.svg");
        }

        [Fact]
        public void GetDeviceTypeOverloadResolvesNameFromIntCode()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("device/type/smartphone.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetDeviceType((int?)DeviceType.DEVICE_TYPE_SMARTPHONE)
                .ShouldBe("/assets/images/devicedetector/device/type/smartphone.svg");
        }

        [Fact]
        public void GetIconsIncludesBotIconWhenResultIsBot()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("bot/category/Search bot.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });
            var result = new DeviceDetectorResult
            {
                Bot = new BotMatchResult { Name = "UnknownBot", Category = "Search bot" }
            };

            var icons = resolver.GetIcons(result);

            icons.BotIcon.ShouldBe("/assets/images/devicedetector/bot/category/Search bot.svg");
        }

        [Fact]
        public void GetIconsBundlesAllFivePaths()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("client/type/browser.svg");
            dir.CreateFile("client/type/os.svg");
            dir.CreateFile("device/type/tv.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });
            var result = new DeviceDetectorResult
            {
                Os = new OsMatchResult { Name = "SomeUnknownOs" },
                Client = new BrowserMatchResult { Name = "SomeUnknownBrowser", Type = "browser" },
                Device = new DeviceMatchResult { Brand = "SomeUnknownBrand", Type = DeviceType.DEVICE_TYPE_TV }
            };

            var icons = resolver.GetIcons(result);

            icons.BotIcon.ShouldBeNull();
            icons.OsIcon.ShouldBe("/assets/images/devicedetector/client/type/os.svg");
            icons.ClientIcon.ShouldBe("/assets/images/devicedetector/client/type/browser.svg");
            icons.BrandIcon.ShouldBe("/assets/images/devicedetector/device/type/tv.svg");
            icons.DeviceTypeIcon.ShouldBe("/assets/images/devicedetector/device/type/tv.svg");
        }
    }
}
