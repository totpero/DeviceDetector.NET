using DeviceDetectorNET.Icons;
using Shouldly;
using Xunit;

namespace DeviceDetectorNET.Icons.Tests
{
    public class IconResolverSingleLevelTests
    {
        [Fact]
        public void PrefersHigherPriorityExtensionWhenMultipleExist()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("client/type/browser.png");
            dir.CreateFile("client/type/browser.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetClientType("browser").ShouldBe("/assets/images/devicedetector/client/type/browser.svg");
        }

        [Fact]
        public void FallsBackToConfiguredFallbackIconWhenNothingMatches()
        {
            using var dir = new TempIconDirectory();

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetClientType("nonexistent-type").ShouldBe("/assets/images/devicedetector/Matomo.svg");
        }

        [Fact]
        public void SanitizesNameBeforeLookingUpFile()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("client/os/OS2.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetOs("OS/2").ShouldBe("/assets/images/devicedetector/client/os/OS2.svg");
        }

        [Fact]
        public void UsesCustomUrlBasePath()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("device/type/tv.svg");

            var resolver = new IconResolver(new IconResolverOptions
            {
                PhysicalRootPath = dir.RootPath,
                UrlBasePath = "/cdn/icons"
            });

            resolver.GetDeviceType("tv").ShouldBe("/cdn/icons/device/type/tv.svg");
        }

        [Fact]
        public void GetBotCategoryResolvesDirectly()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("bot/category/Crawler.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetBotCategory("Crawler").ShouldBe("/assets/images/devicedetector/bot/category/Crawler.svg");
        }

        [Fact]
        public void GetBrowserFamilyResolvesDirectly()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("client/browser/family/Chrome.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetBrowserFamily("Chrome").ShouldBe("/assets/images/devicedetector/client/browser/family/Chrome.svg");
        }

        [Fact]
        public void GetBrowserEngineResolvesDirectly()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("client/browser/engine/Blink.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetBrowserEngine("Blink").ShouldBe("/assets/images/devicedetector/client/browser/engine/Blink.svg");
        }

        [Fact]
        public void GetOsFamilyResolvesDirectly()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("client/os/family/Linux.svg");

            var resolver = new IconResolver(new IconResolverOptions { PhysicalRootPath = dir.RootPath });

            resolver.GetOsFamily("Linux").ShouldBe("/assets/images/devicedetector/client/os/family/Linux.svg");
        }
    }
}
