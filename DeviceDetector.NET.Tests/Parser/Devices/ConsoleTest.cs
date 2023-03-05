using System.Collections.Generic;
using FluentAssertions;
using DeviceDetectorNET.Parser.Device;
using DeviceDetectorNET.Tests.Class.Client.Device;
using DeviceDetectorNET.Yaml;
using Xunit;
using DeviceDetectorNET.Tests.Class.Client;

namespace DeviceDetectorNET.Tests.Parser.Devices
{
    [Trait("Category", "ConsoleTest")]
    public class ConsoleTest
    {
        private readonly List<DeviceModelFixture> _fixtureData;

        public ConsoleTest()
        {
            var path = $"{Utils.CurrentDirectory()}\\{@"Parser\Devices\fixtures\console.yml"}";

            var parser = new YamlParser<List<DeviceModelFixture>>();
            _fixtureData = parser.ParseFile(path);

            //replace null
            //_fixtureData = _fixtureData.Select(f =>
            //{
            //    f.client.version = f.client.version ?? "";
            //    return f;
            //}).ToList();
        }


        [Fact]
        public void ConsoleTestParse()
        {
            var consoleParser = new ConsoleParser();
            foreach (var fixture in _fixtureData)
            {
                consoleParser.SetUserAgent(fixture.user_agent);
                var result = consoleParser.Parse();
                result.Success.Should().BeTrue("Match should be with success to " + fixture.device.model);

                result.Match.Type.Should().Be(DeviceDetectorNET.Parser.Device.Devices.DeviceTypes[fixture.device.type],
                    "Types should be equal");

                result.Match.Brand.Should().BeEquivalentTo(fixture.device.brand, "Brand should be equal");
                result.Match.Model.Should().BeEquivalentTo(fixture.device.model, "Model should be equal");

            }
        }
    }
}