using System;
using System.Runtime.Serialization;

namespace DeviceDetectorNET.Results
{
    [Serializable]
    [DataContract]

    class UnknownOsMatchResult : OsMatchResult
    {
        [DataMember]
        public override string Name { get => "UNK"; }
        [DataMember]
        public override string ShortName { get => "UNK"; }
    }
}
