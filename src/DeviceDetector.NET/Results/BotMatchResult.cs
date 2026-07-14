using DeviceDetectorNET.Class;
using System;
using System.Runtime.Serialization;

namespace DeviceDetectorNET.Results
{
    [Serializable]
    [DataContract]
    public class BotMatchResult: IBotMatchResult
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public Producer Producer { get; set; }

        public override string ToString() =>
        $"Category: {Category}; " +
        $"{Environment.NewLine} " +
        $"Name: {Name};" +
        $"{Environment.NewLine} " +
        $"Url: {Url};" +
        $"{Environment.NewLine} " +
        $"Producer: {Producer?.Name};";
    }
}