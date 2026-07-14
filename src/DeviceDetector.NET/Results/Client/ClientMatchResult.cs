using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DeviceDetectorNET.Results.Client
{
    [Serializable]
    [DataContract]
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$clientResultType", UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType)]
    [JsonDerivedType(typeof(ClientMatchResult), typeDiscriminator: "client")]
    [JsonDerivedType(typeof(BrowserMatchResult), typeDiscriminator: "browser")]
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