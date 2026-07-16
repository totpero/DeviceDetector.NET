using System;
using System.IO;
using DeviceDetectorNET.Icons.Matomo;
using Shouldly;
using Xunit;

namespace DeviceDetectorNET.Icons.Matomo.Tests
{
    /// <summary>
    /// Exercises <see cref="MatomoIconResolver"/> against the REAL vendored matomo-icons submodule
    /// (icon-packs/matomo-icons/dist), not a synthetic <see cref="TempIconDirectory"/>. This is the test
    /// that would have caught the ~25-brand underscore/altered-filename gap fixed alongside it (see
    /// MatomoIconResolverOptions.NameReplacements) and is meant to catch future upstream drift in the
    /// real file layout.
    /// </summary>
    public class MatomoIconResolverRealPackIntegrationTests
    {
        [Fact]
        public void ResolvesRepresentativeIconsFromTheRealVendoredMatomoIconsPack()
        {
            var distPath = FindRealMatomoIconsDistPath();
            if (distPath == null)
            {
                // The icon-packs/matomo-icons git submodule isn't initialized/checked out in this
                // environment (e.g. a CI checkout that skips submodules, or a shallow/partial clone).
                // Rather than hard-failing the whole suite over an environment setup gap unrelated to
                // this package's own logic, this test skips itself gracefully. Every code path it would
                // exercise is already covered against synthetic fixtures by MatomoIconResolverLookupTests
                // and MatomoIconResolverOptionsTests — this test's unique value (catching real upstream
                // drift) simply doesn't apply when the real pack isn't present to drift against.
                return;
            }

            var resolver = new MatomoIconResolver(new MatomoIconResolverOptions { PhysicalRootPath = distPath });

            // Plain single-word brand, stored as-is (no NameReplacements entry needed).
            resolver.GetBrand("Apple").ShouldBe("/assets/images/devicedetector/brand/Apple.png");

            // One of the ~25 brands whose real filename uses underscores instead of the full name with
            // spaces — this is exactly the case this fix's NameReplacements entries resolve.
            resolver.GetBrand("Sony Ericsson").ShouldBe("/assets/images/devicedetector/brand/Sony_Ericsson.png");

            // Browser short code.
            resolver.GetBrowser("FF").ShouldBe("/assets/images/devicedetector/browsers/FF.png");

            // OS short code.
            resolver.GetOs("WIN").ShouldBe("/assets/images/devicedetector/os/WIN.png");

            // Device type.
            resolver.GetDeviceType("smartphone").ShouldBe("/assets/images/devicedetector/devices/smartphone.png");
        }

        /// <summary>
        /// Walks up from the test assembly's output directory looking for the repo root (marked by
        /// <c>DeviceDetector.NET.sln</c>), then returns <c>icon-packs/matomo-icons/dist</c> underneath it
        /// if that submodule folder is actually checked out (has a non-empty <c>brand/</c> subfolder).
        /// Walking up from <see cref="AppContext.BaseDirectory"/> rather than assuming a fixed relative
        /// depth (e.g. <c>../../../../../..</c>) keeps this robust across Debug/Release, TFM changes, and
        /// differences in how various CI runners lay out build output.
        /// </summary>
        private static string FindRealMatomoIconsDistPath()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);

            for (var i = 0; i < 12 && dir != null; i++, dir = dir.Parent)
            {
                var slnPath = Path.Combine(dir.FullName, "DeviceDetector.NET.sln");
                if (!File.Exists(slnPath))
                {
                    continue;
                }

                var distPath = Path.Combine(dir.FullName, "icon-packs", "matomo-icons", "dist");
                var brandPath = Path.Combine(distPath, "brand");
                if (Directory.Exists(brandPath) && Directory.GetFiles(brandPath).Length > 0)
                {
                    return distPath;
                }

                return null;
            }

            return null;
        }
    }
}
