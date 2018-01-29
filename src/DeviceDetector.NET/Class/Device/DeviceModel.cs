using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace DeviceDetectorNET.Class.Device
{
    public class DeviceModel: Model
    {
        [YamlMember(Alias = "models")]
        public List<Model> Models { get; set; }
    }
}