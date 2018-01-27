using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using DeviceDetector.NET.Parser.Client;
using DeviceDetector.NET.Tests.Class.Client;
using DeviceDetector.NET.Yaml;

namespace DeviceDetector.NET.Tests.Parser.Client
{
    [Trait("Category", "Library")]
    public class LibraryTest
    {
        private readonly List<ClientFixture> _fixtureData;

        public LibraryTest()
        {
            var path = $"{Utils.CurrentDirectory()}\\{@"Parser\Client\fixtures\library.yml"}";

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
        public void LibraryTestParse()
        {
            var libraryParser = new LibraryParser();
            foreach (var fixture in _fixtureData)
            {
                libraryParser.SetUserAgent(fixture.user_agent);
                var result = libraryParser.Parse();
                result.Success.Should().BeTrue("Match should be with success");

                result.Match.Name.ShouldBeEquivalentTo(fixture.client.name,"Names should be equal");
                result.Match.Type.ShouldBeEquivalentTo(fixture.client.type, "Types should be equal");
                result.Match.Version.ShouldBeEquivalentTo(fixture.client.version, "Versions should be equal");
            }

        }
    }
}
