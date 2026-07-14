using DeviceDetectorNET.Cache;
using DeviceDetectorNET.Class;
using DeviceDetectorNET.Parser;
using DeviceDetectorNET.Parser.Client;
using DeviceDetectorNET.Parser.Device;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Device;
using DeviceDetectorNET.Tests.Class;
using DeviceDetectorNET.Yaml;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeviceDetectorNET.Results.Client;
using Xunit;

namespace DeviceDetectorNET.Tests;

[Trait("Category", "DeviceDetector")]
public class DeviceDetectorTest
{
    // Uncomment below to test the cache during the xUnit tests
    // public DeviceDetectorTest()
    //{
    //    DeviceDetectorSettings.ParseCacheDBExpiration = TimeSpan.FromDays(365);
    //}

    [Fact]
    public void TestAddClientParserInvalid()
    {
        var dd = new DeviceDetector();
        dd.ShouldNotBeNull();
    }

    ///// <summary>
    ///// check the regular expression for the vertical line closing the group
    ///// </summary>
    //private bool CheckRegexVerticalLineClosingGroup(string regex)
    //{
    //    if (!regex.Contains('|')) return true;
    //    const string pattern = @"#(?<!\\\)(\|\))#is";
    //    return !Regex.IsMatch(regex, pattern);
    //}

    //public function testDevicesYmlFiles()
    //{
    //$fixtureFiles = glob(realpath(dirname(__FILE__)). '/../regexes/device/*.yml');
    //    foreach ($fixtureFiles AS $file) {
    //    $ymlData = \Spyc::YAMLLoad($file);
    //        foreach ($ymlData AS $brand => $regex) {
    //        $this->assertArrayHasKey('regex', $regex);
    //        $this->assertTrue(strpos($regex['regex'], '||') === false, sprintf(
    //            "Detect `||` in regex, file %s, brand %s, common regex %s",
    //            $file,
    //            $brand,
    //            $regex['regex']
    //        ));
    //        $this->assertTrue($this->checkRegexVerticalLineClosingGroup($regex['regex']), sprintf(
    //            "Detect `|)` in regex, file %s, brand %s, common regex %s",
    //            $file,
    //            $brand,
    //            $regex['regex']
    //        ));

    //            if (array_key_exists('models', $regex))
    //            {
    //            $this->assertInternalType('array', $regex['models']);
    //                foreach ($regex['models'] AS $model) {
    //                $this->assertArrayHasKey('regex', $model);
    //                $this->assertArrayHasKey('model', $model, sprintf(
    //                    "Key model not exist, file %s, brand %s, model regex %s",
    //                    $file,
    //                    $brand,
    //                    $model['regex']
    //                ));
    //                $this->assertTrue(strpos($model['regex'], '||') === false, sprintf(
    //                    "Detect `||` in regex, file %s, brand %s, model regex %s",
    //                    $file,
    //                    $brand,
    //                    $model['regex']
    //                ));

    //                $this->assertTrue($this->checkRegexVerticalLineClosingGroup($model['regex']), sprintf(
    //                    "Detect `|)` in regex, file %s, brand %s, model regex %s",
    //                    $file,
    //                    $brand,
    //                    $model['regex']
    //                ));
    //                }
    //            }
    //            else
    //            {
    //            $this->assertArrayHasKey('device', $regex);
    //            $this->assertArrayHasKey('model', $regex);
    //            $this->assertInternalType('string', $regex['model']);
    //            }
    //        }
    //    }
    //}

    [Fact]
    public void TestCacheSetAndGet()
    {
        var dd = new DeviceDetector();
        var cache = new DictionaryCache();
        dd.SetCache(cache);
        dd.GetCache().ShouldBeOfType<DictionaryCache>();
    }

    [Fact]
    public void TestParseEmptyUa()
    {
        var dd = new DeviceDetector();
        dd.Parse();
        dd.IsParsed().ShouldBeTrue();
        dd.Parse(); // call second time complete code coverage
        dd.IsParsed().ShouldBeTrue();
        var client = dd.GetClient();
        client.Success.ShouldBeFalse();
        dd.IsDesktop().ShouldBeFalse();
        dd.IsMobile().ShouldBeFalse();
    }

    [Fact]
    public void TestParseInvalidUa()
    {
        var dd = new DeviceDetector("12345");
        dd.Parse();
        dd.IsParsed().ShouldBeTrue();
        var client = dd.GetClient();
        client.Success.ShouldBeFalse();
        dd.IsDesktop().ShouldBeFalse();
        dd.IsMobile().ShouldBeFalse();
    }

    [Fact]
    public void TestIsParsed()
    {
        var dd = new DeviceDetector("Mozilla/5.0 (Linux; Android 4.2.2; ARCHOS 101 PLATINUM Build/JDQ39) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.114 Safari/537.36");
        dd.IsParsed().ShouldBeFalse();
        dd.Parse();
        dd.IsParsed().ShouldBeTrue();
    }

    [Theory]
    // [InlineData("bots")]
    [InlineData("camera")]
    [InlineData("car_browser")]
    [InlineData("clienthints")]
    [InlineData("clienthints-app")]
    [InlineData("console")]
    [InlineData("desktop")]
    [InlineData("desktop-1")]
    [InlineData("feature_phone")]
    [InlineData("feed_reader")]
    [InlineData("mediaplayer")]
    [InlineData("mobile_apps")]
    [InlineData("peripheral")]
    [InlineData("phablet")]
    [InlineData("phablet-1")]
    [InlineData("podcasting")]
    [InlineData("portable_media_player")]
    [InlineData("smart_display")]
    [InlineData("smart_speaker")]
    [InlineData("smartphone")]
    [InlineData("smartphone-1")]
    [InlineData("smartphone-2")]
    [InlineData("smartphone-3")]
    [InlineData("smartphone-4")]
    [InlineData("smartphone-5")]
    [InlineData("smartphone-6")]
    [InlineData("smartphone-7")]
    [InlineData("smartphone-8")]
    [InlineData("smartphone-9")]
    [InlineData("smartphone-10")]
    [InlineData("smartphone-11")]
    [InlineData("smartphone-12")]
    [InlineData("smartphone-13")]
    [InlineData("smartphone-14")]
    [InlineData("smartphone-15")]
    [InlineData("smartphone-16")]
    [InlineData("smartphone-17")]
    [InlineData("smartphone-18")]
    [InlineData("smartphone-19")]
    [InlineData("smartphone-20")]
    [InlineData("smartphone-21")]
    [InlineData("smartphone-22")]
    [InlineData("smartphone-23")]
    [InlineData("smartphone-24")]
    [InlineData("smartphone-25")]
    [InlineData("smartphone-26")]
    [InlineData("smartphone-27")]
    [InlineData("smartphone-28")]
    [InlineData("smartphone-29")]
    [InlineData("smartphone-30")]
    [InlineData("smartphone-31")]
    [InlineData("smartphone-32")]
    [InlineData("smartphone-33")]
    [InlineData("smartphone-34")]
    [InlineData("smartphone-35")]
    [InlineData("smartphone-36")]
    [InlineData("smartphone-37")]
    [InlineData("smartphone-38")]
    [InlineData("smartphone-39")]
    [InlineData("smartphone-40")]
    [InlineData("smartphone-41")]
    [InlineData("smartphone-42")]
    [InlineData("smartphone-43")]
    [InlineData("tablet")]
    [InlineData("tablet-1")]
    [InlineData("tablet-2")]
    [InlineData("tablet-3")]
    [InlineData("tablet-4")]
    [InlineData("tablet-5")]
    [InlineData("tablet-6")]
    [InlineData("tablet-7")]
    [InlineData("tablet-8")]
    [InlineData("tablet-9")]
    [InlineData("tablet-10")]
    [InlineData("tablet-11")]
    [InlineData("tablet-12")]
    [InlineData("tv")]
    [InlineData("tv-1")]
    [InlineData("tv-2")]
    [InlineData("tv-3")]
    [InlineData("tv-4")]
    [InlineData("tv-5")]
    [InlineData("unknown")]
    [InlineData("wearable")]
    public void TestParse(string fileNme)
    {
        //DeviceDetectorSettings.RegexesDirectory = @"D:\WorkSpaces\GitHubVisualStudio\DeviceDetector.Net\src\DeviceDetector.NET\";
        var parser = new YamlParser<List<DeviceDetectorFixture>>();
        var fixtureData = parser.ParseFile($@"{Utils.CurrentDirectory()}\fixtures\{fileNme}.yml");
        DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);

        Parallel.ForEach(fixtureData, expected =>
            // fixtureData.ForEach(expected =>
        {
            var clientHints = expected.headers.Any() ? ClientHints.Factory(expected.headers.ToDictionary()) : null;

            var dd = DeviceDetector.GetInfoFromUserAgent(expected.UserAgent, clientHints);
            dd.Success.ShouldBeTrue();

            // Bot fixtures only contain the bot data (like the PHP getInfoFromUserAgent result)
            if (expected.bot != null)
            {
                dd.Match.Bot.ShouldNotBeNull(string.Format("a bot should be detected (UA: {0})", expected.UserAgent));
                dd.Match.Bot.Name.ShouldBeIgnoringCase(expected.bot.name, string.Format("Bot.Name should be equal (UA: {0})", expected.UserAgent));
                return;
            }

            dd.Match.OsFamily.ShouldBeIgnoringCase(expected.os_family, string.Format("OsFamily should be equal (UA: {0})", expected.UserAgent));
            dd.Match.BrowserFamily.ShouldBeIgnoringCase(expected.browser_family, string.Format("BrowserFamily should be equal (UA: {0})", expected.UserAgent));

            if (expected.os != null)
            {
                switch (expected.os)
                {
                    case Dictionary<object, object> { Count: > 0 } dicOs:
                    {
                        if (dicOs.TryGetValue("name", out var osName))
                        {
                            dd.Match.Os.Name.ShouldBeIgnoringCase(osName.ToString(), string.Format("Os.Name should be equal (UA: {0})", expected.UserAgent));
                        }
                        if (dicOs.TryGetValue("version", out var osVersion))
                        {
                            dd.Match.Os.Version.ShouldBeIgnoringCase(osVersion.ToString(), string.Format("Os.Version should be equal (UA: {0})", expected.UserAgent));
                        }
                        if (dicOs.TryGetValue("platform", out var osPlatform))
                        {
                            (dd.Match.Os.Platform ?? string.Empty).ShouldBeIgnoringCase(osPlatform?.ToString() ?? string.Empty, string.Format("Os.Platform should be equal (UA: {0})", expected.UserAgent));
                        }
                        if (dicOs.TryGetValue("short_name", out var osShortName))
                        {
                            dd.Match.Os.ShortName.ShouldBeIgnoringCase(osShortName.ToString(), string.Format("Os.ShortName should be equal (UA: {0})", expected.UserAgent));
                        }
                        if (dicOs.TryGetValue("family", out var osfamily))
                        {
                            dd.Match.Os.Family.ShouldBeIgnoringCase(osfamily.ToString(), string.Format("Os.Family should be equal (UA: {0})", expected.UserAgent));
                        }
                        break;
                    }
                    case List<object> { Count: > 0 }:
                        throw new Exception("ExpectedOS is a list rather than a dictionary.");
                }
            }

            if (expected.client != null)
            {
                dd.Match.Client.Type.ShouldBeIgnoringCase(expected.client.type, string.Format("Client.Type should be equal (UA: {0})", expected.UserAgent));
                dd.Match.Client.Name.ShouldBeIgnoringCase(expected.client.name, string.Format("Client.Name should be equal (UA: {0})", expected.UserAgent));
                (dd.Match.Client.Version ?? string.Empty).ShouldBeIgnoringCase(expected.client.version ?? string.Empty, string.Format("Client.Version should be equal (UA: {0})", expected.UserAgent));

                if (dd.Match.Client is BrowserMatchResult browserMatch)
                {
                    (browserMatch.Engine ?? string.Empty).ShouldBeIgnoringCase(expected.client.engine ?? string.Empty, string.Format("Client.Engine should be equal (UA: {0})", expected.UserAgent));
                    (browserMatch.EngineVersion ?? string.Empty).ShouldBeIgnoringCase(expected.client.EngineVersion ?? string.Empty, string.Format("Client.EngineVersion should be equal (UA: {0})", expected.UserAgent));
                }
            }

            if (expected.device == null) return;

            (dd.Match.DeviceType ?? string.Empty).ShouldBeIgnoringCase(expected.device.type ?? string.Empty, string.Format("DeviceType should be equal (UA: {0})", expected.UserAgent));
            (dd.Match.DeviceBrand ?? string.Empty).ShouldBeIgnoringCase(expected.device.brand ?? string.Empty, string.Format("DeviceBrand should be equal (UA: {0})", expected.UserAgent));
            (dd.Match.DeviceModel ?? string.Empty).ShouldBeIgnoringCase(expected.device.model ?? string.Empty, string.Format("DeviceModel should be equal (UA: {0})", expected.UserAgent));
        });
    }

    [Theory]
    [InlineData("browser")]
    [InlineData("feed_reader")]
    [InlineData("library")]
    [InlineData("mediaplayer")]
    [InlineData("mobile_app")]
    [InlineData("pim")]
    public void TestParseClient(string fileNme)
    {
        var parser = new YamlParser<List<DeviceDetectorFixture>>();
        var fixtureData = parser.ParseFile($@"{Utils.CurrentDirectory()}\Parser\Client\fixtures\{fileNme}.yml");
        DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);

        Parallel.ForEach(fixtureData, expected =>
        {
            var ua = expected.UserAgent;
            var clientHints = expected.headers.Any() ? ClientHints.Factory(expected.headers.ToDictionary()) : null;

            var uaInfo = DeviceDetector.GetInfoFromUserAgent(ua, clientHints);
            uaInfo.Success.ShouldBeTrue();
            uaInfo.Match.IsBoot.ShouldBeFalse(string.Format("no bot should be detected (UA: {0})", ua));
            uaInfo.Match.Client.Type.ShouldBeIgnoringCase(expected.client.type, string.Format("Client.Type should be equal (UA: {0})", ua));
            uaInfo.Match.Client.Name.ShouldBeIgnoringCase(expected.client.name, string.Format("Client.Name should be equal (UA: {0})", ua));
            (uaInfo.Match.Client.Version ?? string.Empty).ShouldBeIgnoringCase(expected.client.version ?? string.Empty, string.Format("Client.Version should be equal (UA: {0})", ua));

            if (uaInfo.Match.Client is BrowserMatchResult browserMatch)
            {
                (browserMatch.Engine ?? string.Empty).ShouldBeIgnoringCase(expected.client.engine ?? string.Empty, string.Format("Client.Engine should be equal (UA: {0})", ua));
                (browserMatch.EngineVersion ?? string.Empty).ShouldBeIgnoringCase(expected.client.EngineVersion ?? string.Empty, string.Format("Client.EngineVersion should be equal (UA: {0})", ua));
            }
        });
    }

    [Theory]
    [InlineData("camera")]
    [InlineData("car_browser")]
    [InlineData("console")]
    [InlineData("notebook")]
    public void TestParseDevice(string fileNme)
    {
        var parser = new YamlParser<List<DeviceDetectorFixture>>();
        var fixtureData = parser.ParseFile($@"{Utils.CurrentDirectory()}\Parser\Devices\fixtures\{fileNme}.yml");
        DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);

        Parallel.ForEach(fixtureData, expected =>
        {
            var ua = expected.UserAgent;
            var clientHints = expected.headers.Any() ? ClientHints.Factory(expected.headers.ToDictionary()) : null;

            var uaInfo = DeviceDetector.GetInfoFromUserAgent(ua, clientHints);
            uaInfo.Success.ShouldBeTrue();
            uaInfo.Match.IsBoot.ShouldBeFalse(string.Format("no bot should be detected (UA: {0})", ua));
            (uaInfo.Match.DeviceType ?? string.Empty).ShouldBeIgnoringCase(expected.device.type ?? string.Empty, string.Format("DeviceType should be equal (UA: {0})", ua));
            (uaInfo.Match.DeviceBrand ?? string.Empty).ShouldBeIgnoringCase(expected.device.brand ?? string.Empty, string.Format("DeviceBrand should be equal (UA: {0})", ua));
            (uaInfo.Match.DeviceModel ?? string.Empty).ShouldBeIgnoringCase(expected.device.model ?? string.Empty, string.Format("DeviceModel should be equal (UA: {0})", ua));
        });
    }

    [Fact]
    public void TestInstanceReusage()
    {
        var userAgents = new Dictionary<string, DeviceMatchResult>
        {
            {
                "Mozilla/5.0 (Linux; Android 4.2.2; ARCHOS 101 PLATINUM Build/JDQ39) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.114 Safari/537.36",
                new DeviceMatchResult
                {
                    Brand = "Archos",
                    Name = "101 PLATINUM"
                }
            },
            {
                "Opera/9.80 (Linux mips; U; HbbTV/1.1.1 (; Vestel; MB95; 1.0; 1.0; ); en) Presto/2.10.287 Version/12.00",
                new DeviceMatchResult
                {
                    Brand = "Vestel",
                    Name = string.Empty
                }
            },
            {
                "Sraf/3.0 (Linux i686 ; U; HbbTV/1.1.1 (+PVR+DL;NEXUS; TV44; sw1.0) CE-HTML/1.0 Config(L:eng,CC:DEU); en/de)",
                new DeviceMatchResult
                {
                    Brand = string.Empty,
                    Name = string.Empty
                }
            },
        };

        var deviceDetector = new DeviceDetector();

        foreach (var userAgent in userAgents) {
            deviceDetector.SetUserAgent(userAgent.Key);

            // quick sanity check of Reset()
            deviceDetector.IsParsed().ShouldBeFalse();
            deviceDetector.GetModel().ShouldBeNullOrEmpty();

            deviceDetector.Parse();
            deviceDetector.GetBrandName().ShouldBeOneOf(userAgent.Value.Brand, null);
            deviceDetector.GetModel().ShouldBeIgnoringCase(userAgent.Value.Name);

        }
    }


    [Fact]
    public void TestVersionTruncation()
    {
        var versionTruncationFixtures = new List<Tuple<string, int, string, string>> {
            new ("Mozilla/5.0 (Linux; Android 4.2.2; ARCHOS 101 PLATINUM Build/JDQ39) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.114 Safari/537.36", VersionTruncation.VERSION_TRUNCATION_NONE,  "4.2.2", "34.0.1847.114"),
            new ("Mozilla/5.0 (Linux; Android 4.2.2; ARCHOS 101 PLATINUM Build/JDQ39) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.114 Safari/537.36", VersionTruncation.VERSION_TRUNCATION_BUILD, "4.2.2", "34.0.1847.114"),
            new ("Mozilla/5.0 (Linux; Android 4.2.2; ARCHOS 101 PLATINUM Build/JDQ39) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.114 Safari/537.36", VersionTruncation.VERSION_TRUNCATION_PATCH, "4.2.2", "34.0.1847"),
            new ("Mozilla/5.0 (Linux; Android 4.2.2; ARCHOS 101 PLATINUM Build/JDQ39) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.114 Safari/537.36", VersionTruncation.VERSION_TRUNCATION_MINOR, "4.2", "34.0"),
            new ("Mozilla/5.0 (Linux; Android 4.2.2; ARCHOS 101 PLATINUM Build/JDQ39) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.114 Safari/537.36", VersionTruncation.VERSION_TRUNCATION_MAJOR, "4", "34"),
        };

        foreach (var item in versionTruncationFixtures)
        {
            DeviceDetector.SetVersionTruncation(item.Item2);
            var dd = new DeviceDetector(item.Item1);
            dd.Parse();
            var os = dd.GetOs();
            os.Success.ShouldBeTrue();
            os.Match.Version.ShouldBeIgnoringCase(item.Item3);
            var client = dd.GetClient();
            client.Success.ShouldBeTrue();
            client.Match.Version.ShouldBeIgnoringCase(item.Item4);
            DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);
        }
    }
    
    [Fact]
    public void TestVersionTruncationForClientHints()
    {
        DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_MINOR);
        var dd = new DeviceDetector();
        dd.SetClientHints(new ClientHints(
                "Galaxy 4",
                "Android",
                "8.0.5",
                "98.0.14335.105",
                new Dictionary<string, string>
                {
                    { " Not A;Brand", "99.0.0.0" },
                    { "Chromium", "98.0.14335.105" },
                    { "Chrome", "98.0.14335.105" }
                },
                true,
                "",
                "",
                "",
                new List<string>()
            )
        );
        dd.Parse();

        dd.GetOs().Success.ShouldBeTrue();
        dd.GetClient().Success.ShouldBeTrue();
        dd.GetOs().Match.Version.ShouldBeIgnoringCase("8.0");
        dd.GetClient().Match.Version.ShouldBeIgnoringCase("98.0");

        DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);
    }

    [Fact]

    public void TestParseBots()
    {
        var path = $"{Utils.CurrentDirectory()}\\{@"fixtures\bots.yml"}";

        var parser = new YamlParser<List<BotFixture>>();
        var fixtureData = parser.ParseFile(path);
        foreach (var fixture in fixtureData)
        {
            var dd = new DeviceDetector(fixture.user_agent);
            dd.Parse();
            dd.IsBot().ShouldBeTrue();
            var botData = dd.GetBot();
            botData.Success.ShouldBeTrue("Match should be with success");
                 
            botData.Match.Name.ShouldBeIgnoringCase(fixture.bot.name, "Names should be equal");
            botData.Match.Category.ShouldBeIgnoringCase(fixture.bot.category, "Categories should be equal");
            botData.Match.Url.ShouldBeIgnoringCase(fixture.bot.url, "URLs should be equal");
            if (botData.Match.Producer != null && fixture.bot.producer != null)
            {
                botData.Match.Producer.Name.ShouldBeIgnoringCase(fixture.bot.producer.name ?? string.Empty, "Producers name should be equal");
            }

            // client and os will always be unknown for bots
            dd.GetOs().Success.ShouldBeFalse();
            dd.GetClient().Success.ShouldBeFalse();

            dd.GetOs().Matches[0].ShortName.ShouldBeIgnoringCase(DeviceDetector.UNKNOWN);
            dd.GetClient().Matches[0].Name.ShouldBeIgnoringCase(DeviceDetector.UNKNOWN);
            if (!string.IsNullOrEmpty(botData.Match.Category))
            {
                var categories = new []
                    {
                        "Benchmark",
                        "Crawler",
                        "Feed Fetcher",
                        "Feed Parser",
                        "Feed Reader",
                        "Network Monitor",
                        "Read-it-later Service",
                        "Search bot",
                        "Search tools",
                        "Security Checker",
                        "Security search bot",
                        "Service Agent",
                        "Service bot",
                        "Site Monitor",
                        "Social Media Agent",
                        "Validator",
                        "AI Agent",
                        "AI Assistant",
                        "AI Data Scraper",
                        "AI Search Crawler",
                    };
                categories.ShouldContain(botData.Match.Category, "Unknown category");
            }
        }
    }

    [Fact]
    public void TestGetInfoFromUaBot()
    {
        var expected = new DeviceDetectorResult {
            UserAgent = "Googlebot/2.1 (http://www.googlebot.com/bot.html)",
            Bot = new BotMatchResult
            {
                Name = "Googlebot",
                Category = "Search bot",
                Url = "https://developers.google.com/search/docs/crawling-indexing/overview-google-crawlers",
                Producer = new Producer
                {
                    Name = "Google Inc.",
                    Url = "https://www.google.com/"
                }
            }
        };

        var dd = DeviceDetector.GetInfoFromUserAgent(expected.UserAgent);
        dd.Success.ShouldBeTrue();
        dd.Match.Bot.Name.ShouldBe(expected.Bot.Name);
        dd.Match.Bot.Category.ShouldBe(expected.Bot.Category);
        dd.Match.Bot.Url.ShouldBe(expected.Bot.Url);
        dd.Match.Bot.Producer.Name.ShouldBe(expected.Bot.Producer.Name);
        dd.Match.Bot.Producer.Url.ShouldBe(expected.Bot.Producer.Url);
    }

    [Fact]
    public void TestParseNoDetails()
    {
        const string userAgent = "Googlebot/2.1 (http://www.googlebot.com/bot.html)";
        var dd = new DeviceDetector(userAgent);
        dd.DiscardBotInformation();
        dd.Parse();
        dd.IsBot().ShouldBeTrue();
    }

    [Fact]
    public void TestMagicMethods()
    {
        var ua = "Mozilla/5.0 (Linux; Android 4.4.2; Nexus 4 Build/KOT49H) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.136 Mobile Safari/537.36";
        var dd = new DeviceDetector(ua);
        dd.Parse();
        var result = dd.Is(ClientType.Browser);
        result.ShouldBeTrue();
    }

    [Fact]
    public void TestInvalidMagicMethod()
    {
        var dd = new DeviceDetector("Mozilla/5.0");
        dd.Parse();
        //dd.InValidMethod();
    }

    [Fact]
    public void TestTypeMethods()
    {
        var path = $"{Utils.CurrentDirectory()}\\{@"Parser\fixtures\type-methods.yml"}";
        var parser = new YamlParser<List<TypeMethodFixture>>();
        var fixtureData = parser.ParseFile(path);

        foreach (var item in fixtureData)
        {
            var dd = new DeviceDetector(item.user_agent);
            dd.DiscardBotInformation();
            dd.Parse();
            dd.IsBot().ShouldBe(item.check[0]);
            dd.IsMobile().ShouldBe(item.check[1]);
            dd.IsDesktop().ShouldBe(item.check[2]);
            dd.IsTablet().ShouldBe(item.check[3]);
            dd.IsTv().ShouldBe(item.check[4]);
            dd.IsWearable().ShouldBe(item.check[5]);
        }
    }

    /// <summary>
    /// Device type check methods, equivalent to the PHP magic methods
    /// (isSmartphone(), isFeaturePhone(), isPhablet(), ...)
    /// </summary>
    [Theory]
    [InlineData("Mozilla/5.0 (Linux; Android 4.4.2; Nexus 4 Build/KOT49H) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.136 Mobile Safari/537.36", DeviceType.DEVICE_TYPE_SMARTPHONE)]
    [InlineData("AIRNESS-AIR99/REV 2.2.1/Teleca Q03B1", DeviceType.DEVICE_TYPE_FEATURE_PHONE)]
    [InlineData("Mozilla/5.0 (Linux; Android 6.0; GI-626 Build/MRA58K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.124 Mobile Safari/537.36", DeviceType.DEVICE_TYPE_PHABLET)]
    [InlineData("Mozilla/5.0 (Linux; U; Android 2.3.3; ja-jp; COOLPIX S800c Build/CP01_WW) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1", DeviceType.DEVICE_TYPE_CAMERA)]
    [InlineData("Mozilla/5.0 (iPod; U; CPU iPhone OS 4_2_1 like Mac OS X; ja-jp) AppleWebKit/533.17.9 (KHTML, like Gecko) Mobile/8C148", DeviceType.DEVICE_TYPE_PORTABLE_MEDIA_PAYER)]
    [InlineData("Mozilla/5.0 (Linux; U; Android 4.0.4; fr-be; DA220HQL Build/IMM76D) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Safari/534.30", DeviceType.DEVICE_TYPE_SMART_DISPLAY)]
    [InlineData("AppleCoreMedia/1.0.0.15F80 (HomePod; U; CPU OS 11_4 like Mac OS X; fr_fr)", DeviceType.DEVICE_TYPE_SMART_SPEAKER)]
    [InlineData("Mozilla/5.0 (Linux; Android 7.0; SHTRIH-SMARTPOS-F2 Build/NRD90M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/51.0.2704.91 Mobile Safari/537.36", DeviceType.DEVICE_TYPE_PERIPHERAL)]
    [InlineData("Mozilla/5.0 (Linux; Android 4.4.2; CarPad-II-P Build/KOT49H) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/30.0.0.0 Safari/537.36", DeviceType.DEVICE_TYPE_CAR_BROWSER)]
    public void TestDeviceTypeCheckMethods(string userAgent, int expectedDeviceType)
    {
        var dd = new DeviceDetector(userAgent);
        dd.Parse();

        dd.GetDevice().ShouldBe(expectedDeviceType);
        dd.IsSmartphone().ShouldBe(DeviceType.DEVICE_TYPE_SMARTPHONE == expectedDeviceType);
        dd.IsFeaturePhone().ShouldBe(DeviceType.DEVICE_TYPE_FEATURE_PHONE == expectedDeviceType);
        dd.IsTablet().ShouldBe(DeviceType.DEVICE_TYPE_TABLET == expectedDeviceType);
        dd.IsPhablet().ShouldBe(DeviceType.DEVICE_TYPE_PHABLET == expectedDeviceType);
        dd.IsConsole().ShouldBe(DeviceType.DEVICE_TYPE_CONSOLE == expectedDeviceType);
        dd.IsTv().ShouldBe(DeviceType.DEVICE_TYPE_TV == expectedDeviceType);
        dd.IsCarBrowser().ShouldBe(DeviceType.DEVICE_TYPE_CAR_BROWSER == expectedDeviceType);
        dd.IsSmartDisplay().ShouldBe(DeviceType.DEVICE_TYPE_SMART_DISPLAY == expectedDeviceType);
        dd.IsCamera().ShouldBe(DeviceType.DEVICE_TYPE_CAMERA == expectedDeviceType);
        dd.IsPortableMediaPlayer().ShouldBe(DeviceType.DEVICE_TYPE_PORTABLE_MEDIA_PAYER == expectedDeviceType);
        dd.IsSmartSpeaker().ShouldBe(DeviceType.DEVICE_TYPE_SMART_SPEAKER == expectedDeviceType);
        dd.IsWearable().ShouldBe(DeviceType.DEVICE_TYPE_WEARABLE == expectedDeviceType);
        dd.IsPeripheral().ShouldBe(DeviceType.DEVICE_TYPE_PERIPHERAL == expectedDeviceType);
    }

    /// <summary>
    /// Client type check methods, equivalent to the PHP magic methods
    /// (isBrowser(), isFeedReader(), isMobileApp(), isPIM(), isLibrary(), isMediaPlayer())
    /// </summary>
    [Theory]
    [InlineData("Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)", "browser")]
    [InlineData("Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.21 (KHTML, like Gecko) akregator/4.11.5 Safari/537.21", "feed reader")]
    [InlineData("Pulse/4.0.5 (iPhone; iOS 7.0.6; Scale/2.00)", "mobile app")]
    [InlineData("Outlook-Express/7.0 (MSIE 7.0; Windows NT 6.1; WOW64; Trident/6.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; AskTbORJ/5.15.9.29495; .NET4.0E; TmstmpExt)", "pim")]
    [InlineData("Wget/1.10+devel", "library")]
    [InlineData("Audacious/3.6.2 neon/0.30.1", "mediaplayer")]
    public void TestClientTypeCheckMethods(string userAgent, string expectedClientType)
    {
        var dd = new DeviceDetector(userAgent);
        dd.Parse();

        dd.IsBrowser().ShouldBe("browser" == expectedClientType);
        dd.IsFeedReader().ShouldBe("feed reader" == expectedClientType);
        dd.IsMobileApp().ShouldBe("mobile app" == expectedClientType);
        dd.IsPIM().ShouldBe("pim" == expectedClientType);
        dd.IsLibrary().ShouldBe("library" == expectedClientType);
        dd.IsMediaPlayer().ShouldBe("mediaplayer" == expectedClientType);
    }

    [Fact]
    public void TestGetUserAgentAndClientHints()
    {
        const string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36";
        var clientHints = ClientHints.Factory(new Dictionary<string, string>
        {
            ["sec-ch-ua-platform"] = "Windows",
        });
        var dd = new DeviceDetector(userAgent, clientHints);

        dd.GetUserAgent().ShouldBe(userAgent);
        dd.GetClientHints().ShouldBeSameAs(clientHints);
    }

    [Fact]
    public void TestLruCache()
    {
        var dd = LRUCachedDeviceDetector.GetDeviceDetector("Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
        dd.IsParsed().ShouldBeTrue();
        var os = dd.GetOs();
        os.Success.ShouldBeTrue();
        os = dd.GetOs();
        os.Match.Name.ShouldBeIgnoringCase("Windows");
        os.Match.ShortName.ShouldBeIgnoringCase("WIN");
        os.Match.Version.ShouldBeIgnoringCase("7");
        os.Match.Platform.ShouldBeIgnoringCase(PlatformType.X64);
    }

    [Fact]
    public void TestGetOs()
    {
        var dd = new DeviceDetector("Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
        var os = dd.GetOs();
        os.Success.ShouldBeFalse();
        dd.Parse();
        os = dd.GetOs();
        os.Match.Name.ShouldBeIgnoringCase("Windows");
        os.Match.ShortName.ShouldBeIgnoringCase("WIN");
        os.Match.Version.ShouldBeIgnoringCase("7");
        os.Match.Platform.ShouldBeIgnoringCase(PlatformType.X64);
    }

    [Fact]
    public void TestGetClient()
    {
        var dd = new DeviceDetector("Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
        var client = dd.GetBrowserClient();
        client.Success.ShouldBeFalse();
        dd.Parse();
        client = dd.GetBrowserClient();
            
        client.Match.Type.ShouldBeIgnoringCase("browser");
        client.Match.Name.ShouldBeIgnoringCase("Internet Explorer");
        client.Match.ShortName.ShouldBeIgnoringCase("IE");
        client.Match.Version.ShouldBeIgnoringCase("9.0");
        client.Match.Engine.ShouldBeIgnoringCase("Trident");
        client.Match.EngineVersion.ShouldBeIgnoringCase("5.0");
        //client.Match.Family.ShouldBeIgnoringCase("Internet Explorer");
    }

    [Fact]
    public void TestGetBrandName()
    {
        var dd = new DeviceDetector("Mozilla/5.0 (Linux; Android 4.4.2; Nexus 4 Build/KOT49H) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.136 Mobile Safari/537.36");
        dd.Parse();
        var brand = dd.GetBrandName();
        brand.ShouldBeIgnoringCase("Google");         
    }

    [Fact]
    public void TestIsTouchEnabled()
    {
        var dd = new DeviceDetector("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; ARM; Trident/6.0; Touch; ARMBJS)");
        dd.Parse();
        dd.IsTouchEnabled().ShouldBeTrue();
    }

    [Fact]
    public void TestSkipBotDetection()
    {
        var ua = "Mozilla/5.0 (iPhone; CPU iPhone OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5376e Safari/8536.25 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";

        var dd = new DeviceDetector(ua);
        dd.Parse();
        dd.IsMobile().ShouldBeFalse();
        dd.IsBot().ShouldBeTrue();

        dd = new DeviceDetector(ua);
        dd.SkipBotDetection();
        dd.Parse();

        dd.IsMobile().ShouldBeTrue();
        dd.IsBot().ShouldBeFalse();

    }

    [Fact]
    public void TestManySegmentedVersion()
    {
        // https://github.com/totpero/DeviceDetector.NET/issues/31
        const string userAgent = "Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3983.280.0.3987.132 Safari/537.36";

        var detector = new DeviceDetector(userAgent);
        detector.Parse();
        detector.IsDesktop().ShouldBeTrue();
    }

    [Fact]
    public void TestIntOverflowVersion()
    {
        // https://github.com/totpero/DeviceDetector.NET/issues/5
        const string userAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2227.1529614902747 Safari/537.36";

        var detector = new DeviceDetector(userAgent);
        detector.Parse();
        detector.IsDesktop().ShouldBeTrue();
    }

    [Fact]
    public void TestX()
    {
        //const string userAgent = "Microsoft Office OneNote/16.0.13328.20478 (Windows/10.0; Desktop x64; en-GB; Universal app; Dell Inc./XXX TEST)";
        const string userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36";

        var ch = ClientHints.Factory(new Dictionary<string, string> { { "x-requested-with", "com.appssppa.idesktoppcbrowser" } });
        var dd = DeviceDetector.GetInfoFromUserAgent(userAgent, ch);

        dd.Success.ShouldBeTrue();
        dd.Match.BrowserFamily.ShouldBe("Chrome");
    }

    ///// <summary>
    ///// Issue #70
    ///// </summary>
    //[Fact]
    //public void TestIssue70()
    //{
    //    //const string userAgent = "Mozilla/5.0+(iPhone;+CPU+iPhone+OS+13_4+like+Mac+OS+X)+AppleWebKit/605.1.15+(KHTML,+like+Gecko)+CriOS/77.0.3865.103+Mobile/15E148+Safari/605.1";
    //    const string userAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_4 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/77.0.3865.103 Mobile/15E148 Safari/605.1";
    //    var dd = DeviceDetector.GetInfoFromUserAgent(userAgent);
    //    dd.Success.ShouldBeTrue();
    //}

    ///// <summary>
    ///// Issue #76
    ///// </summary>
    //[Fact]
    //public void TestIssue76()
    //{
    //    const string userAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/89.0.4389.116 Safari/534.24 XiaoMi/MiuiBrowser/17.0.20 swan-mibrowser";
    //    var dd = DeviceDetector.GetInfoFromUserAgent(userAgent);
    //    dd.Success.ShouldBeTrue();
    //}

    /// <summary>
    /// Issue #79
    /// </summary>
    [Fact]
    public void TestIssue79()
    {
        const string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.60 Safari/537.36";
        var clientHints = ClientHints.Factory(new Dictionary<string, string>
        {
            { "sec-ch-ua", "\"(Not(A:Brand\";v=\"8\", \"Chromium\";v=\"2022\"" },
            { "sec-ch-ua-full-version", "2022.04" },
            { "sec-ch-ua-mobile", "?0" },
            { "sec-ch-ua-model", "" },
            { "sec-ch-ua-platform", "Windows" },
            { "sec-ch-ua-platform-version", "15.0.0" },
            { "sec-fetch-dest", string.Empty },
            { "sec-fetch-mode", "cors" },
            { "sec-fetch-site", "same-origin" }
        });

        var dd = DeviceDetector.GetInfoFromUserAgent(userAgent, clientHints);
        dd.Success.ShouldBeTrue();
        // Upstream device-detector maps Chromium client hints with a YYYY(.MM) version to Iridium
        // (see upstream Tests/Parser/Client/fixtures/browser.yml fixtures with "Chromium";v="2022").
        dd.Match.Client.Name.ShouldBe("Iridium");
    }

    /// <summary>
    /// Issue #79 part 2
    /// </summary>
    [Fact]
    public void TestIssue79_Test2()
    {
        const string userAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36";
        var clientHints = ClientHints.Factory(new Dictionary<string, string>
        {
            ["Sec-Ch-Ua-Full-Version-List"] = "\"Chromium\";v=\"122.0.6261.69\", \"Not(A:Brand\";v=\"24.0.0.0\", \"Google Chrome\";v=\"122.0.6261.69\"",
        });

        var dd = DeviceDetector.GetInfoFromUserAgent(userAgent, clientHints);
        dd.Success.ShouldBeTrue();
        dd.Match.Client.Name.ShouldNotBe("Iridium");
    }

    /// <summary>
    /// Issue #22
    /// </summary>
    [Fact]
    public void TestIssue22_Test1()
    {
        const string userAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 16_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/16.1 Mobile/15E148 Safari/604.1";
        var dd = new DeviceDetector(userAgent);
        dd.Parse();
        dd.IsMobile().ShouldBeTrue();
    }
    /// <summary>
    /// Issue #22
    /// </summary>
    [Fact]
    public void TestIssue22_Test2()
    {
        const string userAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.0.1 Safari/605.1.15";
        var dd = new DeviceDetector(userAgent);
        dd.Parse();
        dd.IsMobile().ShouldBeFalse();
    }    
    
    /// <summary>
    /// Issue #22
    /// </summary>
    [Fact]
    public void TestIssue22_Test3()
    {
        const string userAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.0.1 Safari/605.1.15";
        var dd = DeviceDetector.GetInfoFromUserAgent(userAgent);
        dd.Success.ShouldBeTrue();
        dd.Match.Client.Type.ShouldBe("browser");
        dd.Match.OsFamily.ShouldBe("Mac");
        dd.Match.DeviceType.ShouldBe("desktop");
    }

    /// <summary>
    /// Issue #88
    /// </summary>
    [Fact]
    public void TestIssue88_Test1()
    {
        const string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36";
        var dd = DeviceDetector.GetInfoFromUserAgent(userAgent);
        dd.Success.ShouldBeTrue();
        var browserMatch = dd.Match.Client as BrowserMatchResult;
        browserMatch.Name.ShouldBe("Chrome");
        browserMatch.Version.ShouldBe("131.0.0.0");
        browserMatch.Engine.ShouldBe("Blink");
        browserMatch.EngineVersion.ShouldBe("131.0.0.0");
    }

    /// <summary>
    /// Issue #88
    /// </summary>
    [Fact]
    public void TestIssue88_Test2()
    {
        var clientHints = ClientHints.Factory(new Dictionary<string, string>
        {
            ["sec-ch-ua"] = "\"Google Chrome\";v=\"131\", \"Chromium\";v=\"131\", \"Not_A Brand\";v=\"24\"",
        });
        const string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36";
        var dd = DeviceDetector.GetInfoFromUserAgent(userAgent, clientHints);
        dd.Success.ShouldBeTrue();
        var browserMatch = dd.Match.Client as BrowserMatchResult;
        browserMatch.Name.ShouldBe("Chrome");
        browserMatch.Version.ShouldBe("131.0.0.0");
        browserMatch.Engine.ShouldBe("Blink");
        browserMatch.EngineVersion.ShouldBe("131.0.0.0");
    }

    /// <summary>
    /// Issue #97
    /// The same result like: https://devicedetector.lw1.at/azure-logic-apps%2F1.0%20(workflow%2003e6c330959f42e6b2d98b9e3859a40b;%20version%2008584645446027273492)%20microsoft-flow%2F1.0
    /// </summary>
    [Fact]
    public void TestIssue97()
    {
        const string userAgent = "azure-logic-apps/1.0 (workflow 03e6c330959f42e6b2d98b9e3859a40b; version 08584645446027273492) microsoft-flow/1.0";
        var dd = DeviceDetector.GetInfoFromUserAgent(userAgent);
        dd.Success.ShouldBeTrue();
        dd.Match.IsBoot.ShouldBeTrue();
        var bot = dd.Match.Bot;
        bot.Name.ShouldBe("Microsoft Power Automate");
        bot.Category.ShouldBe("Service Agent");
        bot.Url.ShouldBe("https://www.microsoft.com/en-us/power-platform/products/power-automate");
        bot.Producer.Name.ShouldBe("Microsoft Corporation");
        bot.Producer.Url.ShouldBe("https://www.microsoft.com/");
    }
}