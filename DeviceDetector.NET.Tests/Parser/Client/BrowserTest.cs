using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using DeviceDetectorNET.Parser.Client;
using DeviceDetectorNET.Tests.Class.Client;
using DeviceDetectorNET.Yaml;
using System.Linq;

namespace DeviceDetectorNET.Tests.Parser.Client
{
    [Trait("Category", "Browser")]
    public class BrowserTest
    {
        private List<BrowserFixture> _fixtureData;

        public BrowserTest()
        {
            var path = $"{Utils.CurrentDirectory()}\\{@"Parser\Client\fixtures\browser.yml"}";

            var parser = new YamlParser<List<BrowserFixture>>();
            _fixtureData = parser.ParseFile(path);

            //replace null
            _fixtureData = _fixtureData.Select(f =>
            {
                f.client.version = f.client.version ?? "";
                f.client.engine = f.client.engine ?? "";
                f.client.engine_version = f.client.engine_version ?? "";
                return f;
            }).ToList();
        }

        [Fact]
        public void TestGetAvailableBrowserFamilies()
        {
            BrowserParser.GetAvailableBrowserFamilies().Count.Should().BeGreaterThan(5);
        }


        [Fact]
        public void TestAllBrowsersTested()
        {
            var browsers = new BrowserParser();
            BrowserParser.SetVersionTruncation(BrowserParser.VERSION_TRUNCATION_NONE);

            foreach (var fixture in _fixtureData)
            {
                browsers.SetUserAgent(fixture.user_agent);
                var result = browsers.Parse();
              
                result.Success.Should().BeTrue("Match should be with success");
                result.Match.Engine.Should().BeEquivalentTo(fixture.client.engine, "Engine should be equal " + fixture.user_agent);
                result.Match.EngineVersion.Should().BeEquivalentTo(fixture.client.engine_version.ToString(), "EngineVersion should be equal");
                result.Match.Name.Should().BeEquivalentTo(fixture.client.name, "Names should be equal");
                result.Match.ShortName.Should().BeEquivalentTo(fixture.client.short_name, "Short Names should be equal");
                result.Match.Type.Should().BeEquivalentTo(fixture.client.type, "Type should be equal");
                result.Match.Version.Should().BeEquivalentTo(fixture.client.version, "Version should be equal");

            }
        }

        [Fact]
        public void TestGetAvailableClients()
        {
            var available = new BrowserParser().GetAvailableClients();
            BrowserParser.GetAvailableBrowsers().Count.Should().BeGreaterOrEqualTo(available.Count);
        }

    }
}
