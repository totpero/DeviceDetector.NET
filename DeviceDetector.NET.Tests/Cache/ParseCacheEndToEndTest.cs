using System;
using System.IO;
using Shouldly;
using Xunit;

namespace DeviceDetectorNET.Tests.Cache
{
    // End-to-end regression test for https://github.com/totpero/DeviceDetector.NET/issues/95
    // "Using System.Text.Json results in incomplete cache conversion": the first (uncached)
    // parse of a user agent returned a correct result, but re-parsing the same user agent -
    // which is served from the LiteDB-backed parse cache - came back with the client/browser
    // information missing.
    [Trait("Category", "ParseCacheSerialization")]
    public class ParseCacheEndToEndTest
    {
        private const string UserAgent =
            "Mozilla/5.0 (iPhone; CPU iPhone OS 17_4 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/17.4 Mobile/15E148 Safari/604.1";

        [Fact]
        public void ParsingSameUserAgentTwice_ReturnsSameBrowserResult_OnCacheHit()
        {
            var previousExpiration = DeviceDetectorSettings.ParseCacheDBExpiration;
            var previousDirectory = DeviceDetectorSettings.ParseCacheDBDirectory;
            var previousFilename = DeviceDetectorSettings.ParseCacheDBFilename;

            var tempDir = Path.Combine(Path.GetTempPath(), "DeviceDetectorNET_ParseCacheTest_" + Guid.NewGuid().ToString("N"));

            try
            {
                DeviceDetectorSettings.ParseCacheDBDirectory = tempDir;
                DeviceDetectorSettings.ParseCacheDBFilename = "test-cache.db";
                DeviceDetectorSettings.ParseCacheDBExpiration = TimeSpan.FromDays(1);

                var firstRun = new DeviceDetector(UserAgent);
                firstRun.Parse();
                firstRun.IsBrowser().ShouldBeTrue();
                var firstBrowser = firstRun.GetBrowserClient();
                firstBrowser.Success.ShouldBeTrue();

                // A brand-new DeviceDetector instance parsing the same user agent must be
                // served from the cache entry written by the run above.
                var cachedRun = new DeviceDetector(UserAgent);
                cachedRun.Parse();
                cachedRun.IsBrowser().ShouldBeTrue();
                var cachedBrowser = cachedRun.GetBrowserClient();

                cachedBrowser.Success.ShouldBeTrue();
                cachedBrowser.Match.ShouldNotBeNull();
                cachedBrowser.Match.Name.ShouldBe(firstBrowser.Match.Name);
                cachedBrowser.Match.Version.ShouldBe(firstBrowser.Match.Version);
                cachedBrowser.Match.ShortName.ShouldBe(firstBrowser.Match.ShortName);
                cachedBrowser.Match.Engine.ShouldBe(firstBrowser.Match.Engine);
                cachedBrowser.Match.Family.ShouldBe(firstBrowser.Match.Family);

                cachedRun.GetOs().Success.ShouldBeTrue();
                cachedRun.GetOs().Match.Name.ShouldBe(firstRun.GetOs().Match.Name);
            }
            finally
            {
                DeviceDetectorSettings.ParseCacheDBExpiration = previousExpiration;
                DeviceDetectorSettings.ParseCacheDBDirectory = previousDirectory;
                DeviceDetectorSettings.ParseCacheDBFilename = previousFilename;

                // Best-effort cleanup: the LiteDB file stays open for the lifetime of the
                // process (ParseCache is a process-wide singleton), so deletion can fail.
                try
                {
                    if (Directory.Exists(tempDir))
                    {
                        Directory.Delete(tempDir, true);
                    }
                }
                catch (IOException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                }
            }
        }
    }
}
