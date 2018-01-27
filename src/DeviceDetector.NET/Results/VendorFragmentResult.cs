using DeviceDetector.NET.Results.Device;

namespace DeviceDetector.NET.Results
{
    public class VendorFragmentResult : IDeviceMatchResult
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public int? Type { get => throw new System.NotSupportedException(); set => throw new System.NotSupportedException(); }
    }
}