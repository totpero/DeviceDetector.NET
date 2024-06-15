using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using DeviceDetectorNET.Parser;
using DeviceDetectorNET.Tests.Class.Client;
using DeviceDetectorNET.Yaml;
using Xunit;

namespace DeviceDetectorNET.Tests.Parser
{
    [Trait("Category","Os")]

    public class OperatingSystemTest
    {
        private readonly List<OsFixture> _fixtureData;

        public OperatingSystemTest()
        {
            var path = $"{Utils.CurrentDirectory()}\\{@"Parser\fixtures\oss.yml"}";

            var parser = new YamlParser<List<OsFixture>>();
            _fixtureData = parser.ParseFile(path);

            //replace null
            _fixtureData = _fixtureData.Select(f =>
            {
                f.os.version ??= string.Empty;
                return f;
            }).ToList();
        }

        [Fact]
        public void OsTestParse()
        {
            Parallel.ForEach(_fixtureData, fixture =>
            {
                var operatingSystemParser = new OperatingSystemParser();
                operatingSystemParser.SetUserAgent(fixture.user_agent);
                if (fixture.headers != null)
                {
                    var clientHints = ClientHints.Factory(fixture.headers);
                    operatingSystemParser.SetClientHints(clientHints);
                }
                var result = operatingSystemParser.Parse();
                result.Success.Should().BeTrue("Match should be with success");

                result.Match.Name.Should().BeEquivalentTo(fixture.os.name, "Names should be equal");
                result.Match.ShortName.Should().BeEquivalentTo(fixture.os.short_name, "short_names should be equal");
                result.Match.Version.Should().BeEquivalentTo(fixture.os.version, "Versions should be equal");
            });
        }

        [Fact]
        public void TestX()
        {
            var os = new OperatingSystemParser();
            os.SetUserAgent("Mozilla/5.0 (Linux; Android 10; Mi Max Prime Build/QP1A.191005.007) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/74.0.3729.186 Mobile Safari/537.36");
            var clientHints = ClientHints.Factory(new(){ { "http-x-requested-with", "org.lineageos.jelly" } });
            os.SetClientHints(clientHints);
            var r = os.Parse();
            r.Should().NotBeNull();
            r.Match.Name.Should().Be("Lineage OS");
        }

        [Fact]
        public void TestOSInGroup()
        {
            var allOs = OperatingSystemParser.GetAvailableOperatingSystems();
            var familiesOs = OperatingSystemParser.GetAvailableOperatingSystemFamilies();
            foreach (var os in allOs.Keys)
            {
                var contains = false;
                foreach (var familyOs in familiesOs.Values)
                {
                    if (!familyOs.Contains(os)) continue;
                    contains = true;
                    break;
                }
                contains.Should().BeTrue();
            }
        }
        
        [Fact]
        public void TestFamilyOSExists()
        {
            var allFamilyOs = OperatingSystemParser.GetAvailableOperatingSystemFamilies();
            var allOs = OperatingSystemParser.GetAvailableOperatingSystems();
            
            foreach (var familyOs in allFamilyOs)
            {
                var contains = false;
                foreach (var familyOsItem in familyOs.Value)
                {
                    
                    if (!allOs.ContainsKey(familyOsItem)) continue;
                    contains = true;
                    break;
                }                
                contains.Should().BeTrue();
            }
        }

        [Fact]
        public void TestGetAvailableOperatingSystems()
        {
            var count = OperatingSystemParser.GetAvailableOperatingSystems().Count;
            count.Should().BeGreaterThan(70);
        }
        
        [Fact]
        public void TestGetAvailableOperatingSystemFamilies()
        {
            var count = OperatingSystemParser.GetAvailableOperatingSystemFamilies().Count;
            count.Should().Be(23);
        }

        [Theory]
        [InlineData("DEB", "4.5", "Debian 4.5")]
        [InlineData("WRT", "", "Windows RT")]
        [InlineData("WIN", "98", "Windows 98")]
        [InlineData("XXX", "4.5", "")]
        public void TestGetNameFromId(string os, string version, string expected)
        {
            var result = OperatingSystemParser.GetNameFromId(os, version);
            result.Should().BeEquivalentTo(expected);
        }
    }
}
