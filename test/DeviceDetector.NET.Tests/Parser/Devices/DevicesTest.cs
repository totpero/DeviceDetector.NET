using FluentAssertions;
using System.Collections.Generic;
using DeviceDetectorNET.Class.Device;
using DeviceDetectorNET.Parser.Device;
using DeviceDetectorNET.Results.Device;
using Xunit;

namespace DeviceDetectorNET.Tests.Parser.Devices
{
    [Trait("Category", "DevicesTest")]
    public class DevicesTest
    {
        [Fact]
        public void DeviceTypesTest()
        {
            DeviceParserAbstract<Dictionary<string, DeviceModel>, DeviceMatchResult>
                .DeviceTypes
                .Count
                .Should()
                .Be(11);
        }

        [Fact]
        public void DeviceBrandsTest()
        {
            DeviceParserAbstract<Dictionary<string, DeviceModel>, DeviceMatchResult>
                .DeviceBrands
                .Count
                .Should()
                .Be(386);
        }
    }
}
