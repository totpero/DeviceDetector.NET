using System.Collections.Generic;
using FluentAssertions;
using DeviceDetectorNET.Parser.Device;
using DeviceDetectorNET.Tests.Class.Client.Device;
using DeviceDetectorNET.Yaml;
using Xunit;

namespace DeviceDetectorNET.Tests.Parser.Devices
{
    [Trait("Category", "NotebookTest")]
    public class NotebookTest
    {
        private readonly List<DeviceModelFixture> _fixtureData;

        public NotebookTest()
        {
            var path = $"{Utils.CurrentDirectory()}\\{@"Parser\Devices\fixtures\notebook.yml"}";
            var parser = new YamlParser<List<DeviceModelFixture>>();
            _fixtureData = parser.ParseFile(path);
        }

        [Fact]
        public void NotebookTestParse()
        {
            foreach (var fixture in _fixtureData)
            {
                var notebookParser = new NotebookParser();
                notebookParser.SetUserAgent(fixture.user_agent);
                var result = notebookParser.Parse();
                result.Success.Should().BeTrue("Match should be with success to " + fixture.device.model);

                result.Match.Type.Should().Be(DeviceDetectorNET.Parser.Device.Devices.DeviceTypes[fixture.device.type],
                    "Types should be equal");
                result.Match.Brand.Should().BeEquivalentTo(fixture.device.brand, "Brand should be equal");
                result.Match.Model.Should().BeEquivalentTo(fixture.device.model, "Model should be equal");

            }
        }
    }
}