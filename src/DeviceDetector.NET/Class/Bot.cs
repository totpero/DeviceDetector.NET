using YamlDotNet.Serialization;

namespace DeviceDetectorNET.Class
{
    public class Bot : IParseLibrary
    {
        [YamlMember(Alias = "regex")]
        public string Regex { get; set; }
        [YamlMember(Alias = "name")]
        public string Name { get; set; }
        [YamlMember(Alias = "category")]
        public string Category { get; set; }
        [YamlMember(Alias = "url")]
        public string Url { get; set; }
        [YamlMember(Alias = "producer")]
        public Producer Producer { get; set; }
    }
}
