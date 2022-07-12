using System;
using System.Runtime.Serialization;

namespace DeviceDetectorNET.Results.Client
{
    [Serializable]
    [DataContract]
    public class BrowserMatchResult : ClientMatchResult
    {
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public string Engine { get; set; }
        [DataMember]
        public string EngineVersion { get; set; }

        [DataMember]
        public string Family { get; set; }

        public override string ToString() => 
            base.ToString() +
            $"ShortName: {ShortName}; " +
            $"{Environment.NewLine} " +
            $"Engine: {Engine}; " +
            $"{Environment.NewLine} " +
            $"EngineVersion: {EngineVersion};" +
            $"{Environment.NewLine} " +
            $"Family: {Family};" ;
    }
}