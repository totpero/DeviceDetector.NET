using System;
using System.Runtime.Serialization;

namespace DeviceDetectorNET.Results
{
    [Serializable]
    [DataContract]
    public class OsMatchResult:IMatchResult
    {
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string ShortName { get; set; }
        [DataMember]
        public string Version { get; set; }
        [DataMember]
        public string Platform { get; set; }
        [DataMember]
        public string Family { get; set; }

        public override string ToString() =>
        $"ShortName: {ShortName}; " +
        $"{Environment.NewLine} " +
        $"Name: {Name};" +
        $"{Environment.NewLine} " +
        $"Version: {Version};" +
        $"{Environment.NewLine} " +
        $"Platform: {Platform};" +
        $"{Environment.NewLine} " +
        $"Family: {Family};" ;
    }
}