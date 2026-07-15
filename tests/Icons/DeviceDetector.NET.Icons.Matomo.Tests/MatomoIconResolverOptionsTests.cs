using DeviceDetectorNET.Icons.Matomo;
using Shouldly;
using Xunit;

namespace DeviceDetectorNET.Icons.Matomo.Tests
{
    public class MatomoIconResolverOptionsTests
    {
        [Fact]
        public void DefaultsMatchTheMatomoIconsDistLayout()
        {
            var options = new MatomoIconResolverOptions();

            options.UrlBasePath.ShouldBe("/assets/images/devicedetector");
            options.FallbackIconPath.ShouldBe("/Matomo.svg");
            options.ExtensionPriority.ShouldBe(new[] { "png", "svg", "jpg", "gif", "ico" });
            options.NameReplacements.Count.ShouldBe(5);
            options.NameReplacements["feature phone"].ShouldBe("feature_phone");
            options.NameReplacements["car browser"].ShouldBe("car_browser");
            options.NameReplacements["smart display"].ShouldBe("smart_display");
            options.NameReplacements["portable media player"].ShouldBe("portable_media_player");
            options.NameReplacements["smart speaker"].ShouldBe("smart_speaker");
            options.PhysicalRootPath.ShouldBeNull();
            options.FileExists.ShouldBeNull();
        }
    }
}
