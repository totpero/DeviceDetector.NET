using YamlDotNet.Serialization;

namespace DeviceDetector.NET.Class.Client
{
    public class Browser : IParseLibrary, IClientParseLibrary
    {
        [YamlMember(Alias = "regex")]
        public string Regex { get; set; }
        [YamlMember(Alias = "name")]
        public string Name { get; set; }
        [YamlMember(Alias = "version")]
        public string Version { get; set; }
        [YamlMember(Alias = "engine")]
        public Engine Engine { get; set; }
    }
}
