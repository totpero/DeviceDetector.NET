using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using DeviceDetectorNET.Parser.Client;
using DeviceDetectorNET.Tests.Class.Client;
using DeviceDetectorNET.Yaml;

namespace DeviceDetectorNET.Tests.Parser.Client
{
    [Trait("Category", "Pim")]
    public class PimTest
    {
        private readonly List<ClientFixture> _fixtureData;

        public PimTest()
        {
            var path = $"{Utils.CurrentDirectory()}\\{@"Parser\Client\fixtures\pim.yml"}";

            var parser = new YamlParser<List<ClientFixture>>();
            _fixtureData = parser.ParseFile(path);

            //replace null
            _fixtureData = _fixtureData.Select(f =>
            {
                f.client.version = f.client.version ?? "";
                return f;
            }).ToList();
        }

        [Fact]
        public void MobileAppTestParse()
        {
            var pimParser = new PimParser();
            foreach (var fixture in _fixtureData)
            {
                pimParser.SetUserAgent(fixture.user_agent);
                var result = pimParser.Parse();
                result.Success.Should().BeTrue("Match should be with success");

                result.Match.Name.Should().BeEquivalentTo(fixture.client.name, "Names should be equal");
                result.Match.Type.Should().BeEquivalentTo(fixture.client.type, "Types should be equal");
                result.Match.Version.Should().BeEquivalentTo(fixture.client.version, "Versions should be equal");
            }

        }
    }
}
