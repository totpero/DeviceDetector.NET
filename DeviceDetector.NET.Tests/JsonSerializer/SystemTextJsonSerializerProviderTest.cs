using DeviceDetectorNET.JsonSerializer;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Client;
using Shouldly;
using Xunit;

namespace DeviceDetectorNET.Tests.JsonSerializer
{
    // Regression tests for https://github.com/totpero/DeviceDetector.NET/issues/95
    // "Using System.Text.Json results in incomplete cache conversion"
    [Trait("Category", "ParseCacheSerialization")]
    public class SystemTextJsonSerializerProviderTest
    {
        private static DeviceDetectorCachedData BuildBrowserCachedData()
        {
            var browserMatch = new BrowserMatchResult
            {
                Type = "browser",
                Name = "Chrome",
                Version = "125.0",
                ShortName = "CH",
                Engine = "Blink",
                EngineVersion = "125.0",
                Family = "Chrome"
            };

            return new DeviceDetectorCachedData
            {
                Parsed = true,
                Brand = "AP",
                Model = "iPhone",
                Device = 0,
                Bot = new ParseResult<BotMatchResult>(),
                Os = new ParseResult<OsMatchResult>(new OsMatchResult
                {
                    Name = "iOS",
                    ShortName = "IOS",
                    Version = "17",
                    Platform = "ARM",
                    Family = "iOS"
                }),
                Client = new ParseResult<ClientMatchResult>(browserMatch)
            };
        }

        // This is the exact bug reported in the issue: the first (uncached) parse succeeds,
        // but after a round trip through the cache's JSON layer, ParseResult.Success reverted
        // to false (its default) because System.Text.Json does not deserialize into
        // privately-set properties, which made ParseResult.Match return null.
        [Fact]
        public void CachedClientResult_PreservesSuccessFlag_AfterRoundTrip()
        {
            var provider = new SystemTextJsonSerializerProvider();
            var data = BuildBrowserCachedData();
            data.Client.Success.ShouldBeTrue();

            var json = provider.Serialize(data);
            var restored = provider.Deserialize<DeviceDetectorCachedData>(json);

            restored.Client.Success.ShouldBeTrue();
            restored.Client.Match.ShouldNotBeNull();
            restored.Client.Match.Name.ShouldBe("Chrome");
        }

        [Fact]
        public void CachedOsResult_PreservesSuccessFlag_AfterRoundTrip()
        {
            var provider = new SystemTextJsonSerializerProvider();
            var data = BuildBrowserCachedData();

            var json = provider.Serialize(data);
            var restored = provider.Deserialize<DeviceDetectorCachedData>(json);

            restored.Os.Success.ShouldBeTrue();
            restored.Os.Match.ShouldNotBeNull();
            restored.Os.Match.Name.ShouldBe("iOS");
        }

        [Fact]
        public void CachedBotResult_UnsuccessfulResult_StaysUnsuccessful_AfterRoundTrip()
        {
            var provider = new SystemTextJsonSerializerProvider();
            var data = BuildBrowserCachedData();

            var json = provider.Serialize(data);
            var restored = provider.Deserialize<DeviceDetectorCachedData>(json);

            restored.Bot.Success.ShouldBeFalse();
            restored.Bot.Match.ShouldBeNull();
        }

        // Second facet of the same issue: DeviceDetector stores browser matches as
        // BrowserMatchResult instances inside a ParseResult<ClientMatchResult>. Without
        // polymorphic type information, System.Text.Json serializes list elements using the
        // declared (base) type, silently dropping ShortName/Engine/EngineVersion/Family -
        // which in turn broke DeviceDetector.GetBrowserClient() (it requires
        // `client.Match is BrowserMatchResult`) for every cache hit.
        [Fact]
        public void CachedClientResult_PreservesBrowserSpecificFields_AfterRoundTrip()
        {
            var provider = new SystemTextJsonSerializerProvider();
            var data = BuildBrowserCachedData();

            var json = provider.Serialize(data);
            var restored = provider.Deserialize<DeviceDetectorCachedData>(json);

            restored.Client.Match.ShouldBeOfType<BrowserMatchResult>();
            var browser = (BrowserMatchResult)restored.Client.Match;
            browser.ShortName.ShouldBe("CH");
            browser.Engine.ShouldBe("Blink");
            browser.EngineVersion.ShouldBe("125.0");
            browser.Family.ShouldBe("Chrome");
        }
    }
}
