using System.Collections.Generic;
using Shouldly;
using DeviceDetectorNET.Parser.Device;
using DeviceDetectorNET.Tests.Class.Client.Device;
using DeviceDetectorNET.Yaml;
using Xunit;

namespace DeviceDetectorNET.Tests.Parser.Devices
{
    [Trait("Category", "CarBrowserTest")]
    public class CarBrowserTest
    {
        private readonly List<DeviceModelFixture> _fixtureData;

        public CarBrowserTest()
        {
            var path = $"{Utils.CurrentDirectory()}\\{@"Parser\Devices\fixtures\car_browser.yml"}";

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
        public void CarBrowserTestParse()
        {
            var carBrowserParser = new CarBrowserParser();
            foreach (var fixture in _fixtureData)
            {
                carBrowserParser.SetUserAgent(fixture.user_agent);
                var result = carBrowserParser.Parse();
                result.Success.ShouldBeTrue("Match should be with success to " + fixture.device.model);

                result.Match.Type.ShouldBe(DeviceDetectorNET.Parser.Device.Devices.DeviceTypes[fixture.device.type], "Types should be equal");
                result.Match.Brand.ShouldBeIgnoringCase(fixture.device.brand, "Brand should be equal");
                result.Match.Model.ShouldBeIgnoringCase(fixture.device.model, "Model should be equal");
            }

        }
    }
}