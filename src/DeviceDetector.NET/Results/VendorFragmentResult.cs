using DeviceDetectorNET.Results.Device;

namespace DeviceDetectorNET.Results
{
    public class VendorFragmentResult : IDeviceMatchResult
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public int? Type { get => throw new System.NotSupportedException(); set => throw new System.NotSupportedException(); }
    }
}