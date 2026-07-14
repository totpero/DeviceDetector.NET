using System.Collections.Generic;

namespace DeviceDetectorNET.Icons.Common
{
    /// <summary>
    /// The subset of an icon resolver's options needed to probe for a file and build its returned path.
    /// Implemented by both <c>DeviceDetectorNET.Icons.IconResolverOptions</c> and
    /// <c>DeviceDetectorNET.Icons.Matomo.MatomoIconResolverOptions</c> so both can share <see cref="IconPathProber"/>.
    /// </summary>
    public interface IIconProbeOptions
    {
        string UrlBasePath { get; }
        string FallbackIconPath { get; }
        IList<string> ExtensionPriority { get; }
        IDictionary<string, string> NameReplacements { get; }
    }
}
