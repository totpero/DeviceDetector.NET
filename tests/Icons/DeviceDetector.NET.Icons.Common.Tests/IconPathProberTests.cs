using System.Collections.Generic;
using DeviceDetectorNET.Icons.Common;
using Shouldly;
using Xunit;

namespace DeviceDetectorNET.Icons.Common.Tests
{
    public class IconPathProberTests
    {
        [Fact]
        public void ResolveIconReturnsNullForNullOrEmptyName()
        {
            var prober = new IconPathProber(new FakeProbeOptions(), _ => true);

            prober.ResolveIcon(null, "/folder").ShouldBeNull();
            prober.ResolveIcon(string.Empty, "/folder").ShouldBeNull();
        }

        [Fact]
        public void ResolveIconPrefersHigherPriorityExtension()
        {
            var existing = new HashSet<string> { "/folder/Name.svg", "/folder/Name.png" };
            var prober = new IconPathProber(new FakeProbeOptions(), existing.Contains);

            prober.ResolveIcon("Name", "/folder").ShouldBe("/folder/Name.svg");
        }

        [Fact]
        public void ResolveIconReturnsNullWhenNothingMatches()
        {
            var prober = new IconPathProber(new FakeProbeOptions(), _ => false);

            prober.ResolveIcon("Name", "/folder").ShouldBeNull();
        }

        [Fact]
        public void ResolveIconSanitizesNameBeforeProbing()
        {
            var existing = new HashSet<string> { "/folder/NameSafe.svg" };
            var options = new FakeProbeOptions();
            options.NameReplacements["Name/Unsafe"] = "NameSafe";
            var prober = new IconPathProber(options, existing.Contains);

            prober.ResolveIcon("Name/Unsafe", "/folder").ShouldBe("/folder/NameSafe.svg");
        }

        [Fact]
        public void WithUrlBasePathPrefixesAResolvedPath()
        {
            var prober = new IconPathProber(new FakeProbeOptions(), _ => true);

            prober.WithUrlBasePath("/folder/Name.svg").ShouldBe("/assets/folder/Name.svg");
        }

        [Fact]
        public void WithUrlBasePathFallsBackWhenPathIsNull()
        {
            var prober = new IconPathProber(new FakeProbeOptions(), _ => true);

            prober.WithUrlBasePath(null).ShouldBe("/assets/fallback.svg");
        }
    }
}
