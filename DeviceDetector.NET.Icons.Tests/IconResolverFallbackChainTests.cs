using DeviceDetectorNET.Icons;
using Shouldly;
using Xunit;

namespace DeviceDetectorNET.Icons.Tests
{
    public class IconResolverFallbackChainTests
    {
        [Fact]
        public void GetBotPrefersBotNameOverCategory()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("bot/Googlebot.svg");
            dir.CreateFile("bot/category/Search bot.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetBot("Googlebot", "Search bot").ShouldBe("/assets/images/devicedetector/bot/Googlebot.svg");
        }

        [Fact]
        public void GetBotFallsBackToCategoryWhenBotIconMissing()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("bot/category/Search bot.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetBot("UnknownBot", "Search bot").ShouldBe("/assets/images/devicedetector/bot/category/Search bot.svg");
        }

        [Fact]
        public void GetBotFallsBackToFallbackIconWhenNothingMatches()
        {
            using var dir = new TempIconDirectory();

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetBot("UnknownBot", "UnknownCategory").ShouldBe("/assets/images/devicedetector/Matomo.svg");
        }

        [Fact]
        public void GetBrowserFallsThroughFamilyThenEngineThenGenericBrowserIcon()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("client/browser/engine/Blink.svg");
            dir.CreateFile("client/type/browser.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetBrowser("SomeNewBrowser", "SomeNewFamily", "Blink")
                .ShouldBe("/assets/images/devicedetector/client/browser/engine/Blink.svg");
        }

        [Fact]
        public void GetBrowserFallsBackToGenericBrowserIconWhenNothingElseMatches()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("client/type/browser.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetBrowser("SomeNewBrowser", "SomeNewFamily", "SomeNewEngine")
                .ShouldBe("/assets/images/devicedetector/client/type/browser.svg");
        }

        [Fact]
        public void GetOsFallsThroughFamilyThenGenericOsIcon()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("client/type/os.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetOs("SomeNewOs", "SomeNewFamily").ShouldBe("/assets/images/devicedetector/client/type/os.svg");
        }

        [Fact]
        public void GetClientUsesTypeSubfolderThenGenericTypeIcon()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("client/feed reader/Feedly.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetClient("Feedly", "feed reader")
                .ShouldBe("/assets/images/devicedetector/client/feed reader/Feedly.svg");
        }

        [Fact]
        public void GetClientFallsBackToGenericTypeIconWhenClientIconMissing()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("client/type/feed reader.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetClient("SomeUnknownReader", "feed reader")
                .ShouldBe("/assets/images/devicedetector/client/type/feed reader.svg");
        }

        [Fact]
        public void GetBrandPrefersBrandIconOverDeviceType()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("device/brand/Apple.svg");
            dir.CreateFile("device/type/smartphone.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetBrand("Apple", "smartphone").ShouldBe("/assets/images/devicedetector/device/brand/Apple.svg");
        }

        [Fact]
        public void GetBrandFallsBackToDeviceTypeIcon()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("device/type/smartphone.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetBrand("SomeUnknownBrand", "smartphone")
                .ShouldBe("/assets/images/devicedetector/device/type/smartphone.svg");
        }
    }
}
