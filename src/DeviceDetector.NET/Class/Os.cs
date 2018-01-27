using YamlDotNet.Serialization;

namespace DeviceDetector.NET.Class
{
    public class Os : IParseLibrary
    {
        [YamlMember(Alias = "regex")]
        public string Regex { get; set; }
        [YamlMember(Alias = "name")]
        public string Name { get; set; }
        [YamlMember(Alias = "version")]
        public string Version { get; set; }

    }
}
