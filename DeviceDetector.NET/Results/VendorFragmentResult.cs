using DeviceDetectorNET.Results.Device;
using System;
using System.Runtime.Serialization;

namespace DeviceDetectorNET.Results
{
    [Serializable]
    [DataContract]
    public class VendorFragmentResult : IDeviceMatchResult
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Brand { get; set; }
        [DataMember]
        public int? Type { get => throw new System.NotSupportedException(); set => throw new System.NotSupportedException(); }

        public override string ToString() =>
       $"Name: {Name};" +
       $"{Environment.NewLine} " +
       $"Brand: {Brand};";
    }
}