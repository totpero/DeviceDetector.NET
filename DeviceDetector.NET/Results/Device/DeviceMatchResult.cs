using System;
using System.Runtime.Serialization;

namespace DeviceDetectorNET.Results.Device
{
    [Serializable]
    [DataContract]
    public class DeviceMatchResult : IDeviceMatchResult
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Brand { get; set; }
        [DataMember]
        public int? Type { get; set; }
        [DataMember]
        public string Model { get; set; }

        public override string ToString() =>
         $"Type: {Type}; " +
         $"{Environment.NewLine} " +
         $"Name: {Name};" +
         $"{Environment.NewLine} " +
         $"Brand: {Brand};" +
         $"{Environment.NewLine} " +
         $"Model: {Model};";
    }
}