using System;
using System.Collections.Generic;
using DeviceDetectorNET.Icons.Common;

namespace DeviceDetectorNET.Icons
{
    /// <summary>
    /// Configuration for <see cref="IconResolver"/>.
    /// </summary>
    public class IconResolverOptions : IIconProbeOptions
    {
        public IconResolverOptions()
        {
            UrlBasePath = "/assets/images/devicedetector";
            FallbackIconPath = "/Matomo.svg";
            ExtensionPriority = new List<string> { "svg", "avif", "webp", "png", "jpg", "jpeg", "jxl", "heic", "gif" };
            NameReplacements = new Dictionary<string, string>
            {
                ["OS/2"] = "OS2",
                ["GNU/Linux"] = "GNULinux",
                ["MTK / Nucleus"] = "MTK  Nucleus",
                ["Perl REST::Client"] = "Perl RESTClient",
                ["HTTP:Tiny"] = "HTTP Tiny",
                ["AUX"] = "ＡＵＸ",
                ["MariaDB/MySQL Knowledge Base"] = "MariaDB MySQL Knowledge Base",
                ["Sandoba//Crawler"] = "Sandoba Crawler",
                ["WeSEE:Search"] = "WeSEE Search",
                ["Yeti/Naverbot"] = "Yeti Naverbot"
            };
        }

        /// <summary>
        /// Folder that directly contains the icon pack's <c>bot/</c>, <c>client/</c> and <c>device/</c>
        /// subfolders. Required unless <see cref="FileExists"/> is set.
        /// </summary>
        public string PhysicalRootPath { get; set; }

        /// <summary>
        /// Prefix prepended to every path returned by <see cref="IconResolver"/>.
        /// </summary>
        public string UrlBasePath { get; set; }

        /// <summary>
        /// Path (with <see cref="UrlBasePath"/> prefix) returned when nothing in a fallback chain resolves.
        /// </summary>
        public string FallbackIconPath { get; set; }

        /// <summary>
        /// File extensions to probe, in priority order.
        /// </summary>
        public IList<string> ExtensionPriority { get; set; }

        /// <summary>
        /// Filesystem-unsafe name substitutions applied before probing for a file.
        /// </summary>
        public IDictionary<string, string> NameReplacements { get; set; }

        /// <summary>
        /// Overrides the existence check used instead of <c>File.Exists</c> against
        /// <see cref="PhysicalRootPath"/> — e.g. to back it with an <c>IFileProvider</c>.
        /// </summary>
        public Func<string, bool> FileExists { get; set; }
    }
}
