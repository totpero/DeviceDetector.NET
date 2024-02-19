using DeviceDetectorNET.Cache;
using DeviceDetectorNET.Class;
using DeviceDetectorNET.Parser;
using DeviceDetectorNET.Parser.Client;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Device;
using DeviceDetectorNET.Tests.Class;
using DeviceDetectorNET.Yaml;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        dd.Should().NotBeNull();
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
        dd.GetCache().Should().BeOfType<DictionaryCache>();
    }

    [Fact]
    public void TestParseEmptyUa()
    {
        var dd = new DeviceDetector();
        dd.Parse();
        dd.IsParsed().Should().BeTrue();
        dd.Parse(); // call second time complete code coverage
        dd.IsParsed().Should().BeTrue();
        var client = dd.GetClient();
        client.Success.Should().BeFalse();
        dd.IsDesktop().Should().BeFalse();
        dd.IsMobile().Should().BeFalse();
    }

    [Fact]
    public void TestParseInvalidUa()
    {
        var dd = new DeviceDetector("12345");
        dd.Parse();
        dd.IsParsed().Should().BeTrue();
        var client = dd.GetClient();
        client.Success.Should().BeFalse();
        dd.IsDesktop().Should().BeFalse();
        dd.IsMobile().Should().BeFalse();
    }

    [Fact]
    public void TestIsParsed()
    {
        var dd = new DeviceDetector("Mozilla/5.0 (Linux; Android 4.2.2; ARCHOS 101 PLATINUM Build/JDQ39) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.114 Safari/537.36");
        dd.IsParsed().Should().BeFalse();
        dd.Parse();
        dd.IsParsed().Should().BeTrue();
    }

    [Theory]
    // [InlineData("bots")]
    [InlineData("camera")]
    [InlineData("car_browser")]
    [InlineData("clienthints")]
    [InlineData("clienthints-app")]
    [InlineData("console")]
    [InlineData("desktop")]
    [InlineData("feature_phone")]
    [InlineData("feed_reader")]
    [InlineData("mediaplayer")]
    [InlineData("mobile_apps")]
    [InlineData("peripheral")]
    [InlineData("phablet")]
    [InlineData("phablet-1")]
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
    [InlineData("tv")]
    [InlineData("tv-1")]
    [InlineData("tv-2")]
    [InlineData("unknown")]
    [InlineData("wearable")]
    public void TestParse(string fileNme)
    {
        //DeviceDetectorSettings.RegexesDirectory = @"D:\WorkSpaces\GitHubVisualStudio\DeviceDetector.Net\src\DeviceDetector.NET\";
        var parser = new YamlParser<List<DeviceDetectorFixture>>();
        var fixtureData = parser.ParseFile($"{Utils.CurrentDirectory()}\\fixtures\\{fileNme}.yml");
        DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);

        Parallel.ForEach(fixtureData, expected =>
            // fixtureData.ForEach(expected =>
        {
            var clientHints = expected.headers.Any() ? ClientHints.Factory(expected.headers) : null;

            var dd = DeviceDetector.GetInfoFromUserAgent(expected.user_agent, clientHints);
            dd.Success.Should().BeTrue();
                
            dd.Match.OsFamily.Should().BeOneOf(expected.os_family, DeviceDetector.UNKNOWN_FULL);
            dd.Match.BrowserFamily.Should().BeOneOf(expected.browser_family, DeviceDetector.UNKNOWN_FULL);

            if (expected.os != null)
            {
                switch (expected.os)
                {
                    case Dictionary<object, object> { Count: > 0 } dicOs:
                    {
                        if (dicOs.TryGetValue("name", out var osName))
                        {
                            dd.Match.Os.Name.Should().BeEquivalentTo(osName.ToString());
                        }
                        if (dicOs.TryGetValue("version", out var osVersion))
                        {
                            dd.Match.Os.Version.Should().BeEquivalentTo(osVersion.ToString());
                        }
                        if (dicOs.TryGetValue("platform", out var osPlatform))
                        {
                            dd.Match.Os.Platform.Should().BeOneOf(osPlatform?.ToString(), null, string.Empty);
                        }
                        if (dicOs.TryGetValue("short_name", out var osShortName))
                        {
                            dd.Match.Os.ShortName.Should().BeEquivalentTo(osShortName.ToString());
                        }
                        if (dicOs.TryGetValue("family", out var osfamily))
                        {
                            dd.Match.Os.Family.Should().BeEquivalentTo(osfamily.ToString());
                        }
                        break;
                    }
                    case List<object> { Count: > 0 }:
                        throw new Exception("ExpectedOS is a list rather than a dictionary.");
                    //var osName = dicOs["name"];
                    //dd.Match.Os.Name.Should().BeEquivalentTo(osName.ToString());
                }
            }

            if (expected.client != null)
            {
                dd.Match.Client.Type.Should().BeEquivalentTo(expected.client.type);
                dd.Match.Client.Name.Should().BeEquivalentTo(expected.client.name);
                dd.Match.Client.Version.Should().BeOneOf(expected.client.version, null, string.Empty);
                //dd.Match.Client.Engine.Should().BeEquivalentTo(expected.client.engine);
                //dd.Match.Client.EngineVersion.Should().BeEquivalentTo(expected.client.engine_version);
            }

            if (expected.device == null) return;

            dd.Match.DeviceType?.Should().BeEquivalentTo(expected.device.type);
            dd.Match.DeviceBrand.Should().BeOneOf(expected.device.brand, null, string.Empty);
            dd.Match.DeviceModel?.Should().BeOneOf(expected.device.model, null, string.Empty);
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
            deviceDetector.IsParsed().Should().BeFalse();
            deviceDetector.GetModel().Should().BeNullOrEmpty();

            deviceDetector.Parse();
            deviceDetector.GetBrandName().Should().BeOneOf(userAgent.Value.Brand, null);
            deviceDetector.GetModel().Should().BeEquivalentTo(userAgent.Value.Name);

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
            os.Success.Should().BeTrue();
            os.Match.Version.Should().BeEquivalentTo(item.Item3);
            var client = dd.GetClient();
            client.Success.Should().BeTrue();
            client.Match.Version.Should().BeEquivalentTo(item.Item4);
            DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);
        }
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
            dd.IsBot().Should().BeTrue();
            var botData = dd.GetBot();
            botData.Success.Should().BeTrue("Match should be with success");
                 
            botData.Match.Name.Should().BeEquivalentTo(fixture.bot.name, "Names should be equal");
            botData.Match.Category.Should().BeEquivalentTo(fixture.bot.category, "Categories should be equal");
            botData.Match.Url.Should().BeEquivalentTo(fixture.bot.url, "URLs should be equal");
            if (botData.Match.Producer != null && fixture.bot.producer != null)
            {
                botData.Match.Producer.Name.Should().BeEquivalentTo(fixture.bot.producer.name ?? string.Empty, "Producers name should be equal");
            }

            // client and os will always be unknown for bots
            dd.GetOs().Success.Should().BeFalse();
            dd.GetClient().Success.Should().BeFalse();

            dd.GetOs().Matches[0].ShortName.Should().BeEquivalentTo(DeviceDetector.UNKNOWN);
            dd.GetClient().Matches[0].Name.Should().BeEquivalentTo(DeviceDetector.UNKNOWN);
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
        dd.Success.Should().BeTrue();
        dd.Match.Bot.Should().BeEquivalentTo(expected.Bot);
    }

    [Fact]
    public void TestParseNoDetails()
    {
        const string userAgent = "Googlebot/2.1 (http://www.googlebot.com/bot.html)";
        var dd = new DeviceDetector(userAgent);
        dd.DiscardBotInformation();
        dd.Parse();
        dd.IsBot().Should().BeTrue();
    }

    [Fact]
    public void TestMagicMethods()
    {
        var ua = "Mozilla/5.0 (Linux; Android 4.4.2; Nexus 4 Build/KOT49H) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.136 Mobile Safari/537.36";
        var dd = new DeviceDetector(ua);
        dd.Parse();
        var result = dd.Is(ClientType.Browser);
        result.Should().BeTrue();
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
            dd.IsBot().Should().Be(item.check.Item1);
            dd.IsMobile().Should().Be(item.check.Item2);
            dd.IsDesktop().Should().Be(item.check.Item3);
            dd.IsTablet().Should().Be(item.check.Item4);
            dd.IsTv().Should().Be(item.check.Item5);
            dd.IsWearable().Should().Be(item.check.Item6);
        }
    }
    [Fact]
    public void TestLruCache()
    {
        var dd = LRUCachedDeviceDetector.GetDeviceDetector("Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
        dd.IsParsed().Should().BeTrue();
        var os = dd.GetOs();
        os.Success.Should().BeTrue();
        os = dd.GetOs();
        os.Match.Name.Should().BeEquivalentTo("Windows");
        os.Match.ShortName.Should().BeEquivalentTo("WIN");
        os.Match.Version.Should().BeEquivalentTo("7");
        os.Match.Platform.Should().BeEquivalentTo(PlatformType.X64);
    }

    [Fact]
    public void TestGetOs()
    {
        var dd = new DeviceDetector("Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
        var os = dd.GetOs();
        os.Success.Should().BeFalse();
        dd.Parse();
        os = dd.GetOs();
        os.Match.Name.Should().BeEquivalentTo("Windows");
        os.Match.ShortName.Should().BeEquivalentTo("WIN");
        os.Match.Version.Should().BeEquivalentTo("7");
        os.Match.Platform.Should().BeEquivalentTo(PlatformType.X64);
    }

    [Fact]
    public void TestGetClient()
    {
        var dd = new DeviceDetector("Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
        var client = dd.GetBrowserClient();
        client.Success.Should().BeFalse();
        dd.Parse();
        client = dd.GetBrowserClient();
            
        client.Match.Type.Should().BeEquivalentTo("browser");
        client.Match.Name.Should().BeEquivalentTo("Internet Explorer");
        client.Match.ShortName.Should().BeEquivalentTo("IE");
        client.Match.Version.Should().BeEquivalentTo("9.0");
        client.Match.Engine.Should().BeEquivalentTo("Trident");
        client.Match.EngineVersion.Should().BeEquivalentTo("5.0");
        //client.Match.Family.Should().BeEquivalentTo("Internet Explorer");
    }

    [Fact]
    public void TestGetBrandName()
    {
        var dd = new DeviceDetector("Mozilla/5.0 (Linux; Android 4.4.2; Nexus 4 Build/KOT49H) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.136 Mobile Safari/537.36");
        dd.Parse();
        var brand = dd.GetBrandName();
        brand.Should().BeEquivalentTo("Google");         
    }

    [Fact]
    public void TestIsTouchEnabled()
    {
        var dd = new DeviceDetector("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; ARM; Trident/6.0; Touch; ARMBJS)");
        dd.Parse();
        dd.IsTouchEnabled().Should().BeTrue();
    }

    [Fact]
    public void TestSkipBotDetection()
    {
        var ua = "Mozilla/5.0 (iPhone; CPU iPhone OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5376e Safari/8536.25 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";

        var dd = new DeviceDetector(ua);
        dd.Parse();
        dd.IsMobile().Should().BeFalse();
        dd.IsBot().Should().BeTrue();

        dd = new DeviceDetector(ua);
        dd.SkipBotDetection();
        dd.Parse();

        dd.IsMobile().Should().BeTrue();
        dd.IsBot().Should().BeFalse();

    }

    [Fact]
    public void TestManySegmentedVersion()
    {
        // https://github.com/totpero/DeviceDetector.NET/issues/31
        const string userAgent = "Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3983.280.0.3987.132 Safari/537.36";

        var detector = new DeviceDetector(userAgent);
        detector.Parse();
        detector.IsDesktop().Should().BeTrue();
    }

    [Fact]
    public void TestIntOverflowVersion()
    {
        // https://github.com/totpero/DeviceDetector.NET/issues/5
        const string userAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2227.1529614902747 Safari/537.36";

        var detector = new DeviceDetector(userAgent);
        detector.Parse();
        detector.IsDesktop().Should().BeTrue();
    }

    [Fact]
    public void TestX()
    {
        //const string userAgent = "Microsoft Office OneNote/16.0.13328.20478 (Windows/10.0; Desktop x64; en-GB; Universal app; Dell Inc./XXX TEST)";
        const string userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36";

        var ch = ClientHints.Factory(new Dictionary<string, string> { { "x-requested-with", "com.appssppa.idesktoppcbrowser" } });
        var dd = DeviceDetector.GetInfoFromUserAgent(userAgent, ch);

        dd.Success.Should().BeTrue();
        dd.Match.BrowserFamily.Should().Be("Chrome");
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
    //    dd.Success.Should().BeTrue();
    //}

    ///// <summary>
    ///// Issue #76
    ///// </summary>
    //[Fact]
    //public void TestIssue76()
    //{
    //    const string userAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/89.0.4389.116 Safari/534.24 XiaoMi/MiuiBrowser/17.0.20 swan-mibrowser";
    //    var dd = DeviceDetector.GetInfoFromUserAgent(userAgent);
    //    dd.Success.Should().BeTrue();
    //}
}