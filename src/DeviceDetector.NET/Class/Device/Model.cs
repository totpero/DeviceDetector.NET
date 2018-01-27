using YamlDotNet.Serialization;

namespace DeviceDetector.NET.Class.Device
{
    public class Model: IDeviceParseLibrary
    {
        [YamlMember(Alias = "regex")]
        public string Regex { get; set; }
        [YamlMember(Alias = "model")]
        public string Name { get; set; }
        [YamlMember(Alias = "device")] //mobile
        public string Device { get; set; }
        [YamlMember(Alias = "brand")] //mobile
        public string Brand { get; set; }
    }
}