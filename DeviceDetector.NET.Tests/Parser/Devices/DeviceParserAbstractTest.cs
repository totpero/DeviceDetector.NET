using FluentAssertions;
using Xunit;

namespace DeviceDetectorNET.Tests.Parser.Devices
{
    [Trait("Category", "DeviceParserAbstract")]
    public class DeviceParserAbstractTest
    {

        [Fact]
        public void testGetAvailableDeviceTypes()
        {
            var available = DeviceDetectorNET.Parser.Device.Devices.DeviceTypes;
            available.Count.Should().BeGreaterThan(5);
            available.Should().ContainKey("desktop");
        }
        
        [Fact]
        public void testGetAvailableDeviceTypeNames()
        {
            var available = DeviceDetectorNET.Parser.Device.Devices.GetAvailableDeviceTypeNames();
            available.Count.Should().BeGreaterThan(5);
            available.Should().Contain("desktop");
        }
        
        [Fact]
        public void testGetFullName()
        {
            DeviceDetectorNET.Parser.Device.Devices.GetFullName("Invalid").Should().BeEmpty();
            DeviceDetectorNET.Parser.Device.Devices.GetFullName("AU").Should().Be("Asus");
            DeviceDetectorNET.Parser.Device.Devices.GetFullName("GO").Should().Be("Google");

        }
    }
}
