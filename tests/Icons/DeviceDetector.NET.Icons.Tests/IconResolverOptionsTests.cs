using DeviceDetectorNET.Icons;
using Shouldly;
using Xunit;

namespace DeviceDetectorNET.Icons.Tests
{
    public class IconResolverOptionsTests
    {
        [Fact]
        public void DefaultsMatchUpstreamDDCIcons()
        {
            var options = new IconResolverOptions();

            options.UrlBasePath.ShouldBe("/assets/images/devicedetector");
            options.FallbackIconPath.ShouldBe("/Matomo.svg");
            options.ExtensionPriority.ShouldBe(new[] { "svg", "avif", "webp", "png", "jpg", "jpeg", "jxl", "heic", "gif" });
            options.NameReplacements.Count.ShouldBe(10);
            options.NameReplacements["OS/2"].ShouldBe("OS2");
            options.NameReplacements["GNU/Linux"].ShouldBe("GNULinux");
            options.NameReplacements["MTK / Nucleus"].ShouldBe("MTK  Nucleus");
            options.NameReplacements["Perl REST::Client"].ShouldBe("Perl RESTClient");
            options.NameReplacements["HTTP:Tiny"].ShouldBe("HTTP Tiny");
            options.NameReplacements["AUX"].ShouldBe("ＡＵＸ");
            options.NameReplacements["MariaDB/MySQL Knowledge Base"].ShouldBe("MariaDB MySQL Knowledge Base");
            options.NameReplacements["Sandoba//Crawler"].ShouldBe("Sandoba Crawler");
            options.NameReplacements["WeSEE:Search"].ShouldBe("WeSEE Search");
            options.NameReplacements["Yeti/Naverbot"].ShouldBe("Yeti Naverbot");
            options.PhysicalRootPath.ShouldBeNull();
            options.FileExists.ShouldBeNull();
        }
    }
}
