using System;
using DeviceDetectorNET.Icons.Common;

namespace DeviceDetectorNET.Icons.Matomo
{
    /// <summary>
    /// Resolves icon file paths against the official matomo-icons pack's real folder layout: flat
    /// <c>browsers/</c>, <c>os/</c>, <c>brand/</c> and <c>devices/</c> folders, no bot category, no
    /// intermediate fallback tiers (an unresolved lookup goes straight to <see cref="MatomoIconResolverOptions.FallbackIconPath"/>).
    /// </summary>
    public partial class MatomoIconResolver
    {
        private const string BrowsersPath = "/browsers";
        private const string OsPath = "/os";
        private const string BrandPath = "/brand";
        private const string DevicesPath = "/devices";

        private readonly IconPathProber _prober;

        public MatomoIconResolver(MatomoIconResolverOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            IconResolverOptionsValidation.Validate(options.PhysicalRootPath, options.FileExists, options.ExtensionPriority);

            var fileExists = options.FileExists ?? IconResolverOptionsValidation.DefaultFileExists(options.PhysicalRootPath);
            _prober = new IconPathProber(options, fileExists);
        }

        /// <summary>Looks up a browser icon by its DeviceDetector short code (e.g. <c>"FF"</c>).</summary>
        public string GetBrowser(string shortName)
        {
            return _prober.WithUrlBasePath(_prober.ResolveIcon(shortName, BrowsersPath));
        }

        /// <summary>Looks up an OS icon by its DeviceDetector short code (e.g. <c>"WIN"</c>).</summary>
        public string GetOs(string shortName)
        {
            return _prober.WithUrlBasePath(_prober.ResolveIcon(shortName, OsPath));
        }

        /// <summary>Looks up a brand icon by its full name (matomo-icons keys brand/ by full name, not short code).</summary>
        public string GetBrand(string brandName)
        {
            return _prober.WithUrlBasePath(_prober.ResolveIcon(brandName, BrandPath));
        }

        /// <summary>Looks up a device-type icon (e.g. <c>"smartphone"</c>, <c>"car browser"</c> — converted to snake_case via <see cref="MatomoIconResolverOptions.NameReplacements"/>).</summary>
        public string GetDeviceType(string typeName)
        {
            return _prober.WithUrlBasePath(_prober.ResolveIcon(typeName, DevicesPath));
        }

        /// <summary>matomo-icons has no bot category at all — always returns the configured fallback icon. Kept for API symmetry with <c>DeviceDetectorNET.Icons.IconResolver</c>.</summary>
        public string GetBot(string bot = null, string category = null)
        {
            return _prober.WithUrlBasePath(null);
        }
    }
}
