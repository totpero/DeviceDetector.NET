using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using DeviceDetector.NET.Parser.Client;
using DeviceDetector.NET.Tests.Class.Client;
using DeviceDetector.NET.Yaml;
using Xunit;

namespace DeviceDetector.NET.Tests.Parser.Client
{
    [Trait("Category", "MediaPlayer")]
    public class MediaPlayerTest
    {
        private readonly List<ClientFixture> _fixtureData;

        public MediaPlayerTest()
        {
            var path = $"{Utils.CurrentDirectory()}\\{@"Parser\Client\fixtures\mediaplayer.yml"}";

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
        public void MediaPlayerTestParse()
        {
            var mediaPlayerParser = new MediaPlayerParser();
            foreach (var fixture in _fixtureData)
            {
                mediaPlayerParser.SetUserAgent(fixture.user_agent);
                var result = mediaPlayerParser.Parse();
                result.Success.Should().BeTrue("Match should be with success");

                result.Match.Name.ShouldBeEquivalentTo(fixture.client.name,"Names should be equal");
                result.Match.Type.ShouldBeEquivalentTo(fixture.client.type, "Types should be equal");
                result.Match.Version.ShouldBeEquivalentTo(fixture.client.version, "Versions should be equal");
            }

        }
    }
}
