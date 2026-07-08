using Shouldly;
using Xunit;

namespace DeviceDetectorNET.Tests.Parser.Devices
{
    [Trait("Category", "DevicesTest")]
    public class DevicesTest
    {
        [Fact]
        public void DeviceTypesTest()
        {
            DeviceDetectorNET.Parser.Device.Devices
                             .DeviceTypes
                             .Count
                             .ShouldBe(14);
        }

        [Fact]
        public void DeviceBrandsTest()
        {
            DeviceDetectorNET.Parser.Device.Devices
                             .DeviceBrands
                             .Count
                             .ShouldBe(2138);
        }
    }
}
