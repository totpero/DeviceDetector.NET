using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Xunit;
using DeviceDetectorNET.Parser.Client;
using DeviceDetectorNET.Tests.Class.Client;
using DeviceDetectorNET.Yaml;

namespace DeviceDetectorNET.Tests.Parser.Client
{
    [Trait("Category", "FeedReader")]
    public class FeedReaderTest
    {
        private readonly List<ClientFixture> _fixtureData;

        public FeedReaderTest()
        {
            var path = $"{Utils.CurrentDirectory()}\\{@"Parser\Client\fixtures\feed_reader.yml"}";

            var parser = new YamlParser<List<ClientFixture>>();
            _fixtureData = parser.ParseFile(path);

            //replace null
            _fixtureData = _fixtureData.Select(f =>
            {
                f.client.version ??= string.Empty;
                return f;
            }).ToList();
        }

        [Fact]
        public void FeedReaderTestParse()
        {
            var feedReaderParser = new FeedReaderParser();
            foreach (var fixture in _fixtureData)
            {
                feedReaderParser.SetUserAgent(fixture.user_agent);
                var result = feedReaderParser.Parse();
                result.Success.ShouldBeTrue("Match should be with success");

                result.Match.Name.ShouldBeIgnoringCase(fixture.client.name, "Names should be equal");
                result.Match.Version.ShouldBeIgnoringCase(fixture.client.version, "Versions should be equal");

                result.Match.Type.ShouldBeIgnoringCase(fixture.client.type, "Types should be equal");
            }
        }
    }
}
