using FluentAssertions;
using System.Collections.Generic;
using DeviceDetector.NET.Class.Device;
using DeviceDetector.NET.Parser.Device;
using DeviceDetector.NET.Results.Device;
using Xunit;

namespace DeviceDetector.NET.Tests.Parser.Devices
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
                .Be(341);
        }
    }
}
