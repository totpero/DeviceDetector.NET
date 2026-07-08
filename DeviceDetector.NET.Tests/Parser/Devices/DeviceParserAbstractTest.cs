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

        /// <summary>
        /// Checking the correct operation of the HasUserAgentClientHintsFragment method,
        /// equivalent to the PHP DeviceParserAbstractTest::testHasUserAgentClientHintsFragment
        /// </summary>
        [Theory]
        [InlineData(false, "Mozilla/5.0 (Linux; Android 9; K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/138.0.7204.180 Mobile Safari/537.36 Telegram-Android/12.2.10 (Zte ZTE Blade A3 2020RU; Android 9; SDK 28; LOW)")]
        [InlineData(false, "Mozilla/5.0 (Linux; Android 10; K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/142.0.7444.171 Mobile Safari/537.36 Telegram-Android/12.2.7 (Itel itel W5006X; Android 10; SDK 29; LOW)")]
        [InlineData(true, "Mozilla/5.0 (Linux; Android 16; K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/142.0.7444.171 Mobile Safari/537.36")]
        [InlineData(true, "Mozilla/5.0 (Linux; Android 14; K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/139.0.0.0 Mobile Safari/537.36")]
        [InlineData(true, "Mozilla/5.0 (Linux; Android 11) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/126.0.0.0 Mobile DuckDuckGo/5 Safari/537.36")]
        [InlineData(false, "Mozilla/5.0 (Linux; Android 15; K) Telegram-Android/12.2.10 (Tecno TECNO CL6; Android 15; SDK 35; AVERAGE)")]
        [InlineData(true, "Mozilla/5.0 (Linux; Android 10; K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/142.0.0.0 Mobile Safari/537.36")]
        [InlineData(true, "Mozilla/5.0 (Linux; Android 10; K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/123.0.0.0 Mobile Safari/537.36 AlohaBrowser/5.10.4")]
        [InlineData(true, "Mozilla/5.0 (Linux; Android 10; K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/132.0.227.6834 Safari/537.36  SberBrowser/3.4.0.1123")]
        [InlineData(true, "Mozilla/5.0 (Linux; Android 14; K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/139.0.7232.2 Mobile Safari/537.36 YaApp_Android/22.116.1 YaSearchBrowser/9.20")]
        [InlineData(true, "Mozilla/5.0 (Linux; Android 10; K) AppleWebKit/537.36 (KHTML, like G -ecko) Chrome/142.0.0.0 Safari/537.36 EdgA/142.0.0.0")]
        [InlineData(true, "Mozilla/5.0 (Linux; Android 10; K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/123.0.6312.118 Mobile Safari/537.36 XiaoMi/MiuiBrowser/14.33.0-gn")]
        public void testHasUserAgentClientHintsFragment(bool expectedResult, string userAgent)
        {
            var parser = new DeviceDetectorNET.Parser.Device.MobileParser();
            parser.SetUserAgent(userAgent);

            var method = FindMethod(parser.GetType(), "HasUserAgentClientHintsFragment");
            method.Should().NotBeNull();

            var result = (bool)method.Invoke(parser, System.Array.Empty<object>());
            result.Should().Be(expectedResult, "useragent: {0}", userAgent);
        }

        private static System.Reflection.MethodInfo FindMethod(System.Type type, string name)
        {
            while (type != null)
            {
                var method = type.GetMethod(name,
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (method != null) return method;
                type = type.BaseType;
            }
            return null;
        }
    }
}
