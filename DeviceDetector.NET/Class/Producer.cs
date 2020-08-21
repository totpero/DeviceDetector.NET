using System;
using System.Runtime.Serialization;
using YamlDotNet.Serialization;

namespace DeviceDetectorNET.Class
{
    [Serializable]
    [DataContract]
    public class Producer
    {
        [DataMember]
        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        [DataMember]
        [YamlMember(Alias = "url")]
        public string Url { get; set; }
    }
}
