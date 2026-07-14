using YamlDotNet.Serialization;

namespace DeviceDetectorNET.Class
{
    public class OsVersion
    {
        [YamlMember(Alias = "regex")]
        public string Regex { get; set; }
        [YamlMember(Alias = "version")]
        public string Version { get; set; }

    }
}
