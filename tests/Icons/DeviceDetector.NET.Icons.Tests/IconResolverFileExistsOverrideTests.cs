using System.Collections.Generic;
using DeviceDetectorNET.Icons;
using Shouldly;
using Xunit;

namespace DeviceDetectorNET.Icons.Tests
{
    public class IconResolverFileExistsOverrideTests
    {
        [Fact]
        public void UsesFileExistsDelegateInsteadOfPhysicalDisk()
        {
            var existingPaths = new HashSet<string> { "/device/type/tv.svg" };

            var resolver = new IconResolver(new IconResolverOptions
            {
                FileExists = existingPaths.Contains
            });

            resolver.GetDeviceType("tv").ShouldBe("/assets/images/devicedetector/device/type/tv.svg");
            resolver.GetDeviceType("desktop").ShouldBe("/assets/images/devicedetector/Matomo.svg");
        }
    }
}
