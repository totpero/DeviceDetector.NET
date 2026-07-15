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

        public string UrlBasePath { get; set; }

        public string FallbackIconPath { get; set; }

        public IList<string> ExtensionPriority { get; set; }

        public IDictionary<string, string> NameReplacements { get; set; }

        /// <summary>
        /// Overrides the existence check used instead of <c>File.Exists</c> against
        /// <see cref="PhysicalRootPath"/> — e.g. to back it with an <c>IFileProvider</c>.
        /// </summary>
        public Func<string, bool> FileExists { get; set; }
    }
}
