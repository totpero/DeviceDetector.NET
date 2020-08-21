using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using FluentAssertions;
using DeviceDetectorNET.Cache;
using DeviceDetectorNET.Class;
using DeviceDetectorNET.Parser;
using DeviceDetectorNET.Parser.Client;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Device;
using DeviceDetectorNET.Tests.Class;
using DeviceDetectorNET.Yaml;
using Xunit;

namespace DeviceDetectorNET.Tests
{
    [Trait("Category", "DeviceDetector")]
    public class DeviceDetectorTest
    {
        public DeviceDetectorTest()
        {
            // cache results data for 1 year
            DeviceDetector.ExpirationForDeviceDetectorResults = TimeSpan.FromDays(365);
        }

        [Fact]
        public void TestAddClientParserInvalid()
        {
            var dd = new DeviceDetector();
            dd.AddStandardClientsParser();
            dd.Should().NotBeNull();
        }

        /// <summary>
        /// check the regular expression for the vertical line closing the group
        /// </summary>
        private bool CheckRegexVerticalLineClosingGroup(string regex)
        {
            if (!regex.Contains('|')) return true;
            const string pattern = @"#(?<!\\\)(\|\))#is";
            return !Regex.IsMatch(regex, pattern);
        }

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
        }

        [Fact]
        public void TestParseInvalidUa()
        {
            var dd = new DeviceDetector("12345");
            dd.Parse();
            dd.IsParsed().Should().BeTrue();
            var client = dd.GetClient();
            client.Success.Should().BeFalse();
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
        //[InlineData("bots")]
        [InlineData("camera")]
        [InlineData("car_browser")]
        [InlineData("console")]
        [InlineData("desktop")]
        [InlineData("feature_phone")]
        [InlineData("feed_reader")]
        [InlineData("mediaplayer")]
        [InlineData("mobile_apps")]
        [InlineData("phablet")]
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
        [InlineData("tablet")]
        [InlineData("tablet-1")]
        [InlineData("tablet-2")]
        [InlineData("tablet-3")]
        [InlineData("tablet-4")]
        [InlineData("tv")]
        [InlineData("unknown")]
        [InlineData("wearable")]
        public void TestParse(string fileNme)
        {
            //DeviceDetectorSettings.RegexesDirectory = @"D:\WorkSpaces\GitHubVisualStudio\DeviceDetector.Net\src\DeviceDetector.NET\";
            var parser = new YamlParser<List<DeviceDetectorFixture>>();
            var _fixtureData = parser.ParseFile($"{Utils.CurrentDirectory()}\\fixtures\\{fileNme}.yml");

            foreach (var expected in _fixtureData)
            {
                var dd = DeviceDetector.GetInfoFromUserAgent(expected.user_agent);
                dd.Success.Should().BeTrue();
                dd.Match.OsFamily.Should().BeEquivalentTo(expected.os_family);
                dd.Match.BrowserFamily.Should().BeEquivalentTo(expected.browser_family);
                if (expected.os != null)
                {
                    if (expected.os is Dictionary<object, object> dicOs && dicOs.Count > 0)
                    {
                        var osName = dicOs["name"];
                        dd.Match.Os.Name.Should().BeEquivalentTo(osName.ToString());

                    }
                    else
                    {
                        if (expected.os is List<object> listOs && listOs.Count > 0)
                        {
                            throw new Exception();
                            //var osName = dicOs["name"];
                            //dd.Match.Os.Name.Should().BeEquivalentTo(osName.ToString());

                        }
                    }
                }

                if (expected.client != null)
                {
                    dd.Match.Client.Type.Should().BeEquivalentTo(expected.client.type);
                    dd.Match.Client.Name.Should().BeEquivalentTo(expected.client.name);
                }
                if (expected.device != null)
                {
                    dd.Match.DeviceType?.Should().BeEquivalentTo(expected.device.type);
                    dd.Match.DeviceBrand?.Should().BeEquivalentTo((expected.device.brand ?? ""));
                    dd.Match.DeviceModel?.Should().BeEquivalentTo((expected.device.model ?? ""));
                }
            }
        }

        [Fact]
        public void TestInstanceReusage()
        {
            var userAgents = new Dictionary<string, DeviceMatchResult>()
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
                        Name = "MB95"
                    }
                },
                {
                    "Sraf/3.0 (Linux i686 ; U; HbbTV/1.1.1 (+PVR+DL;NEXUS; TV44; sw1.0) CE-HTML/1.0 Config(L:eng,CC:DEU); en/de)",
                    new DeviceMatchResult
                    {
                        Brand = "",
                        Name = ""
                    }
                },
            };

            var deviceDetector = new DeviceDetector();

            foreach (var userAgent in userAgents) {
                deviceDetector.SetUserAgent(userAgent.Key);
                deviceDetector.Parse();
                deviceDetector.GetBrandName().Should().BeEquivalentTo(userAgent.Value.Brand);
                deviceDetector.GetModel().Should().BeEquivalentTo(userAgent.Value.Name);

            }
        }


        [Fact]
        public void TestVersionTruncation()
        {
            var versionTruncationFixtures = new List<Tuple<string, int, string, string>> {
                new Tuple<string, int, string, string>("Mozilla/5.0 (Linux; Android 4.2.2; ARCHOS 101 PLATINUM Build/JDQ39) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.114 Safari/537.36", VersionTruncation.VERSION_TRUNCATION_NONE,  "4.2.2", "34.0.1847.114"),
                new Tuple<string, int, string, string>("Mozilla/5.0 (Linux; Android 4.2.2; ARCHOS 101 PLATINUM Build/JDQ39) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.114 Safari/537.36", VersionTruncation.VERSION_TRUNCATION_BUILD, "4.2.2", "34.0.1847.114"),
                new Tuple<string, int, string, string>("Mozilla/5.0 (Linux; Android 4.2.2; ARCHOS 101 PLATINUM Build/JDQ39) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.114 Safari/537.36", VersionTruncation.VERSION_TRUNCATION_PATCH, "4.2.2", "34.0.1847"),
                new Tuple<string, int, string, string>("Mozilla/5.0 (Linux; Android 4.2.2; ARCHOS 101 PLATINUM Build/JDQ39) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.114 Safari/537.36", VersionTruncation.VERSION_TRUNCATION_MINOR, "4.2", "34.0"),
                new Tuple<string, int, string, string>("Mozilla/5.0 (Linux; Android 4.2.2; ARCHOS 101 PLATINUM Build/JDQ39) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.114 Safari/537.36", VersionTruncation.VERSION_TRUNCATION_MAJOR, "4", "34"),
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
            var _fixtureData = parser.ParseFile(path);
            foreach (var fixture in _fixtureData)
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
                    botData.Match.Producer.Name.Should().BeEquivalentTo(fixture.bot.producer.name, "Producers name should be equal");
                }

                // client and os will always be unknown for bots
                dd.GetOs().Success.Should().BeFalse();
                dd.GetClient().Success.Should().BeFalse();

                dd.GetOs().Matches[0].ShortName.Should().BeEquivalentTo(DeviceDetector.UNKNOWN);
                dd.GetClient().Matches[0].Name.Should().BeEquivalentTo(DeviceDetector.UNKNOWN);
            }
        }

        [Fact]
        public void TestGetInfoFromUABot()
        {
            var expected = new DeviceDetectorResult {
                UserAgent = "Googlebot/2.1 (http://www.googlebot.com/bot.html)",
                Bot = new BotMatchResult
                {
                    Name = "Googlebot",
                    Category = "Search bot",
                    Url = "http://www.google.com/bot.html",
                    Producer = new Producer
                    {
                        Name = "Google Inc.",
                        Url = "http://www.google.com"
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
           var user_agent = "Googlebot/2.1 (http://www.googlebot.com/bot.html)";
            var dd = new DeviceDetector(user_agent);
            dd.DiscardBotInformation();
            dd.Parse();
            dd.IsBot().Should().BeTrue();
        }

        [Fact(Skip = "Not Implemented")]
        public void TestMagicMethods()
        {
            var ua = "Mozilla/5.0 (Linux; Android 4.4.2; Nexus 4 Build/KOT49H) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.136 Mobile Safari/537.36";
            var dd = new DeviceDetector(ua);
            dd.Parse();
            dd.Is(ClientType.Browser);
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
            var userAgents = new List<Tuple<string, bool, bool, bool>> {
                new Tuple<string, bool, bool, bool>("Googlebot/2.1 (http://www.googlebot.com/bot.html)", true, false, false),
                new Tuple<string, bool, bool, bool>("Mozilla/5.0 (Linux; Android 4.4.2; Nexus 4 Build/KOT49H) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.136 Mobile Safari/537.36", false, true, false),
                new Tuple<string, bool, bool, bool>("Mozilla/5.0 (Linux; Android 4.4.3; Build/KTU84L) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2062.117 Mobile Safari/537.36", false, true, false),
                new Tuple<string, bool, bool, bool>("Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)", false, false, true),
                new Tuple<string, bool, bool, bool>("Mozilla/3.01 (compatible;)", false, false, false),
                // Mobile only browsers:
                new Tuple<string, bool, bool, bool>("Opera/9.80 (J2ME/MIDP; Opera Mini/9.5/37.8069; U; en) Presto/2.12.423 Version/12.16", false, true, false),
                new Tuple<string, bool, bool, bool>("Mozilla/5.0 (X11; U; Linux i686; th-TH@calendar=gregorian) AppleWebKit/534.12 (KHTML, like Gecko) Puffin/1.3.2665MS Safari/534.12", false, true, false),
                new Tuple<string, bool, bool, bool>("Mozilla/5.0 (Linux; Android 4.4.4; MX4 Pro Build/KTU84P) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/33.0.0.0 Mobile Safari/537.36; 360 Aphone Browser (6.9.7)", false, true, false),
                new Tuple<string, bool, bool, bool>("Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_5_7; xx) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Safari/530.17 Skyfire/6DE", false, true, false),
                // useragent containing non unicode chars
                new Tuple<string, bool, bool, bool>("Mozilla/5.0 (Linux; U; Android 4.1.2; ru-ru; PMP7380D3G Build/JZO54K) AppleWebKit/534.30 (KHTML, ÃÂºÃÂ°ÃÂº Gecko) Version/4.0 Safari/534.30", false, true, false),
            };

            foreach (var item in userAgents)
            {
                var dd = new DeviceDetector(item.Item1);
                dd.DiscardBotInformation();
                dd.Parse();
                dd.IsBot().Should().Be(item.Item2);
                dd.IsMobile().Should().Be(item.Item3);
                dd.IsDesktop().Should().Be(item.Item4);
            }
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
    }
}