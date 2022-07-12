using FluentAssertions;
using DeviceDetectorNET.Parser.Device;
using Xunit;

namespace DeviceDetectorNET.Tests.Parser.Devices
{
    [Trait("Category", "HbbTvTest")]
    public class HbbTvTest
    {

        [Fact]
        public void IsHbbTvTest()
        {
            var hbbTvParser = new HbbTvParser();
            hbbTvParser.SetUserAgent("Opera/9.80 (Linux mips ; U; HbbTV/1.1.1 (; Philips; ; ; ; ) CE-HTML/1.0 NETTV/3.2.1; en) Presto/2.6.33 Version/10.70");
            hbbTvParser.IsHbbTv().Should().BeTrue();
            hbbTvParser.HbbTv().Should().BeEquivalentTo("1.1.1");
        }
    }
}