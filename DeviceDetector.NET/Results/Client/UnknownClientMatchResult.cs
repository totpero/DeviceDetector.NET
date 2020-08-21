using System;
using System.Runtime.Serialization;

namespace DeviceDetectorNET.Results.Client
{
    [Serializable]
    [DataContract]
    class UnknownClientMatchResult : ClientMatchResult
    {
        [DataMember]

        public override string Type { get => "UNK"; }
        [DataMember]
        public override string Name { get => "UNK"; }
    }
}
