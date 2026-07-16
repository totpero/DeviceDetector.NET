using System.Collections.Generic;
using DeviceDetectorNET.Icons.Matomo;
using Shouldly;
using Xunit;

namespace DeviceDetectorNET.Icons.Matomo.Tests
{
    public class MatomoIconResolverFileExistsOverrideTests
    {
        [Fact]
        public void UsesFileExistsDelegateInsteadOfPhysicalDisk()
        {
            var existingPaths = new HashSet<string> { "/os/WIN.png" };

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions
            {
                FileExists = existingPaths.Contains
            });

            resolver.GetOs("WIN").ShouldBe("/assets/images/devicedetector/os/WIN.png");
            resolver.GetOs("MAC").ShouldBe("/assets/images/devicedetector/Matomo.svg");
        }
    }
}
