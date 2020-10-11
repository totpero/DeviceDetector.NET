using System;
using System.Runtime.Serialization;

namespace DeviceDetectorNET.Results.Client
{
    [Serializable]
    [DataContract]
    public class ClientMatchResult:IClientMatchResult
    {
        [DataMember]
        public virtual string Type { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public string Version { get; set; }

        public override string ToString() =>
          $"Type: {Type}; " +
          $"{Environment.NewLine} " +
          $"Name: {Name};" +
          $"{Environment.NewLine} " +
          $"Version: {Version};" +
          $"{Environment.NewLine} ";
    }
}