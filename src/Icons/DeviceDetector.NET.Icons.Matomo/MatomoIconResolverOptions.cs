using System;
using System.Collections.Generic;
using DeviceDetectorNET.Icons.Common;

namespace DeviceDetectorNET.Icons.Matomo
{
    /// <summary>
    /// Configuration for <see cref="MatomoIconResolver"/>.
    /// </summary>
    public class MatomoIconResolverOptions : IIconProbeOptions
    {
        public MatomoIconResolverOptions()
        {
            UrlBasePath = "/assets/images/devicedetector";
            FallbackIconPath = "/Matomo.svg";
            ExtensionPriority = new List<string> { "png", "svg", "jpg", "gif", "ico" };
            NameReplacements = new Dictionary<string, string>
            {
                // Devices.GetDeviceName() returns these with spaces; matomo-icons' devices/ folder uses snake_case.
                ["feature phone"] = "feature_phone",
                ["car browser"] = "car_browser",
                ["smart display"] = "smart_display",
                ["portable media player"] = "portable_media_player",
                ["smart speaker"] = "smart_speaker"
            };
        }

        /// <summary>
        /// Folder that directly contains matomo-icons' <c>browsers/</c>, <c>os/</c>, <c>brand/</c> and
        /// <c>devices/</c> subfolders — typically a checkout of matomo-icons' <c>dist/</c> folder.
        /// Required unless <see cref="FileExists"/> is set.
        /// </summary>
        public string PhysicalRootPath { get; set; }

        /// <summary>
        /// Prefix prepended to every path returned by <see cref="MatomoIconResolver"/>.
        /// </summary>
        public string UrlBasePath { get; set; }

        /// <summary>
        /// Path (with <see cref="UrlBasePath"/> prefix) returned when a lookup doesn't resolve.
        /// </summary>
        public string FallbackIconPath { get; set; }

        /// <summary>
        /// File extensions to probe, in priority order. Defaults to <c>png</c> first since matomo-icons'
        /// <c>dist/</c> folder is currently PNG-only end to end; the other entries are headroom for a
        /// future upstream change, not evidence of current use.
        /// </summary>
        public IList<string> ExtensionPriority { get; set; }

        /// <summary>
        /// Filesystem-unsafe or naming-convention substitutions applied before probing for a file.
        /// Defaults to bridging <see cref="DeviceDetectorNET.Parser.Device.Devices.GetDeviceName(int)"/>'s
        /// space-separated multi-word device-type names (e.g. <c>"car browser"</c>) to matomo-icons'
        /// snake_case <c>devices/</c> file names (e.g. <c>car_browser.svg</c>).
        /// </summary>
        public IDictionary<string, string> NameReplacements { get; set; }

        /// <summary>
        /// Overrides the existence check used instead of <c>File.Exists</c> against
        /// <see cref="PhysicalRootPath"/> — e.g. to back it with an <c>IFileProvider</c>.
        /// </summary>
        public Func<string, bool> FileExists { get; set; }
    }
}
