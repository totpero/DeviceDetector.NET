using System.Collections.Generic;
using DeviceDetectorNET.Icons.Common;

namespace DeviceDetectorNET.Icons.Common.Tests
{
    internal sealed class FakeProbeOptions : IIconProbeOptions
    {
        public string UrlBasePath { get; set; } = "/assets";
        public string FallbackIconPath { get; set; } = "/fallback.svg";
        public IList<string> ExtensionPriority { get; set; } = new List<string> { "svg", "png" };
        public IDictionary<string, string> NameReplacements { get; set; } = new Dictionary<string, string>();
    }
}
