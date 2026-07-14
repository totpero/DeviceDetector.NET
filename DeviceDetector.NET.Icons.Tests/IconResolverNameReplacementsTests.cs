using DeviceDetectorNET.Icons;
using Shouldly;
using Xunit;

namespace DeviceDetectorNET.Icons.Tests
{
    public class IconResolverNameReplacementsTests
    {
        [Fact]
        public void MergesCustomReplacementsOverDefaults()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("client/os/MyCustomSafeName.svg");

            var options = new IconResolverOptions { PhysicalRootPath = dir.RootPath };
            options.NameReplacements["Custom/Unsafe:Name"] = "MyCustomSafeName";

            var resolver = new IconResolver(options);

            resolver.GetOs("Custom/Unsafe:Name").ShouldBe("/assets/images/devicedetector/client/os/MyCustomSafeName.svg");
        }

        [Fact]
        public void StillAppliesDefaultReplacementsWhenExtended()
        {
            using var dir = new TempIconDirectory();
            dir.CreateFile("client/os/OS2.svg");

            var options = new IconResolverOptions { PhysicalRootPath = dir.RootPath };
            options.NameReplacements["Another/Unsafe"] = "AnotherSafe";

            var resolver = new IconResolver(options);

            resolver.GetOs("OS/2").ShouldBe("/assets/images/devicedetector/client/os/OS2.svg");
        }
    }
}
