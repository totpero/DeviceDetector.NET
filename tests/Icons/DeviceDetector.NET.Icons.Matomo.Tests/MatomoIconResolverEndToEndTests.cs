using DeviceDetectorNET.Icons.Matomo;
using Shouldly;
using Xunit;

namespace DeviceDetectorNET.Icons.Matomo.Tests
{
    public class MatomoIconResolverEndToEndTests
    {
        [Fact]
        public void ResolvesIconsForARealParsedUserAgent()
        {
            const string userAgent =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36";

            var parseResult = DeviceDetectorNET.DeviceDetector.GetInfoFromUserAgent(userAgent);
            parseResult.Success.ShouldBeTrue();

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions { FileExists = _ => true });

            var icons = resolver.GetIcons(parseResult.Match);

            icons.BotIcon.ShouldBeNull();
            icons.ClientIcon.ShouldContain("/browsers/");
            icons.OsIcon.ShouldContain("/os/");
        }

        [Fact]
        public void ResolvesBotIconAsFallbackForARealParsedBotUserAgent()
        {
            const string userAgent = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";

            var parseResult = DeviceDetectorNET.DeviceDetector.GetInfoFromUserAgent(userAgent);
            parseResult.Success.ShouldBeTrue();
            parseResult.Match.IsBoot.ShouldBeTrue();

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions { FileExists = _ => true });

            var icons = resolver.GetIcons(parseResult.Match);

            // FileExists always returns true here, so GetBot's forced fallback (not a lookup miss) is what's under test:
            icons.BotIcon.ShouldBe("/assets/images/devicedetector/Matomo.svg");
        }
    }
}
