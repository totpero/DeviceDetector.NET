using System;
using System.Runtime.Serialization;

namespace DeviceDetectorNET.Results.Device
{
    [Serializable]
    [DataContract]
    public class DeviceMatchResult : IDeviceMatchResult
    {
        /// <summary>
        /// Model
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Brand { get; set; }
        [DataMember]
        public int? Type { get; set; }
        [DataMember]
        public string Model => Name;

        public override string ToString() =>
         $"Type: {Type}; " +
         $"{Environment.NewLine} " +
         $"Name (Model): {Name};" +
         $"{Environment.NewLine} " +
         $"Brand: {Brand};" +
         $"{Environment.NewLine}";
    }
}