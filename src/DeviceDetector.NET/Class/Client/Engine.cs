using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace DeviceDetector.NET.Class.Client
{
    public class Engine
    {
        [YamlMember(Alias = "default")]
        public string Default { get; set; }
        [YamlMember(Alias = "versions")]
        public Dictionary<string,string> Versions { get; set; }

    }
}
