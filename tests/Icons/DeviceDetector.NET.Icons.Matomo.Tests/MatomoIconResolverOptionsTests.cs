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
            options.NameReplacements.Count.ShouldBe(30);
            options.NameReplacements["feature phone"].ShouldBe("feature_phone");
            options.NameReplacements["car browser"].ShouldBe("car_browser");
            options.NameReplacements["smart display"].ShouldBe("smart_display");
            options.NameReplacements["portable media player"].ShouldBe("portable_media_player");
            options.NameReplacements["smart speaker"].ShouldBe("smart_speaker");

            // matomo-icons' brand/ folder itself isn't fully consistent — these ~25 brands use
            // underscores/altered characters in their real filename despite DeviceMatchResult.Brand
            // giving the name with spaces or special characters.
            options.NameReplacements["Azumi Mobile"].ShouldBe("Azumi_Mobile");
            options.NameReplacements["Barnes & Noble"].ShouldBe("Barnes_Noble");
            options.NameReplacements["Cherry Mobile"].ShouldBe("Cherry_Mobile");
            options.NameReplacements["ComTrade Tesla"].ShouldBe("ComTrade_Tesla");
            options.NameReplacements["Crius Mea"].ShouldBe("Crius_Mea");
            options.NameReplacements["Echo Mobiles"].ShouldBe("Echo_Mobiles");
            options.NameReplacements["Eks Mobility"].ShouldBe("Eks_Mobility");
            options.NameReplacements["General Mobile"].ShouldBe("General_Mobile");
            options.NameReplacements["IMO Mobile"].ShouldBe("IMO_Mobile");
            options.NameReplacements["Kempler & Strauss"].ShouldBe("Kempler_Strauss");
            options.NameReplacements["Krüger&Matz"].ShouldBe("Kr_ger_Matz");
            options.NameReplacements["Land Rover"].ShouldBe("Land_Rover");
            options.NameReplacements["Le Pan"].ShouldBe("Le_Pan");
            options.NameReplacements["Manta Multimedia"].ShouldBe("Manta_Multimedia");
            options.NameReplacements["NUU Mobile"].ShouldBe("NUU_Mobile");
            options.NameReplacements["NYX Mobile"].ShouldBe("NYX_Mobile");
            options.NameReplacements["O+"].ShouldBe("O_");
            options.NameReplacements["PCD Argentina"].ShouldBe("PCD_Argentina");
            options.NameReplacements["RT Project"].ShouldBe("RT_Project");
            options.NameReplacements["Silent Circle"].ShouldBe("Silent_Circle");
            options.NameReplacements["Sony Ericsson"].ShouldBe("Sony_Ericsson");
            options.NameReplacements["Tecno Mobile"].ShouldBe("Tecno_Mobile");
            options.NameReplacements["Tunisie Telecom"].ShouldBe("Tunisie_Telecom");
            options.NameReplacements["U.S. Cellular"].ShouldBe("U_S_Cellular");
            options.NameReplacements["öwn"].ShouldBe("_wn");
            options.PhysicalRootPath.ShouldBeNull();
            options.FileExists.ShouldBeNull();
        }
    }
}
