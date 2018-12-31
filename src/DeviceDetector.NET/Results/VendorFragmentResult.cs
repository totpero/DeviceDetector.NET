using DeviceDetectorNET.Results.Device;
using System;

namespace DeviceDetectorNET.Results
{
    public class VendorFragmentResult : IDeviceMatchResult
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public int? Type { get => throw new System.NotSupportedException(); set => throw new System.NotSupportedException(); }

        public override string ToString() =>
       $"Name: {Name};" +
       $"{Environment.NewLine} " +
       $"Brand: {Brand};";
    }
}