using FluentAssertions;
using DeviceDetector.NET.Parser.Device;
using Xunit;

namespace DeviceDetector.NET.Tests.Parser.Devices
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
            hbbTvParser.HbbTv()[1].Should().BeEquivalentTo("1.1.1");
        }
    }
}