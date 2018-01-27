using YamlDotNet.Serialization;

namespace DeviceDetector.NET.Class.Client
{
    public class BrowserEngine: IClientParseLibrary
    {
        [YamlMember(Alias = "regex")]
        public string Regex { get; set; }
        [YamlMember(Alias = "name")]
        public string Name { get; set; }
        [YamlIgnore]//@todo:change logic
        public string Version { get; set; }
    }
}
