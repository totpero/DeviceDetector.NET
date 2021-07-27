        using FluentAssertions;
using DeviceDetectorNET.Parser.Device;
using Xunit;

namespace DeviceDetectorNET.Tests.Parser.Devices
{
    [Trait("Category", "ShellTvTest")]
    public class ShellTvTest
    {

        [Fact]
        public void IsShellTvTest()
        {
            var shellTvParser = new ShellTvParser();
            shellTvParser.SetUserAgent("Leff Shell LC390TA2A");
            shellTvParser.IsShellTv().Should().BeTrue();
            shellTvParser.SetUserAgent("Leff Shell");
            shellTvParser.IsShellTv().Should().BeFalse();
        }
    }
}