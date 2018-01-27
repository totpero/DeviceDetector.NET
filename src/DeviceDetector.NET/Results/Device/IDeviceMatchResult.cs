namespace DeviceDetector.NET.Results.Device
{
    public interface IDeviceMatchResult : IMatchResult
    {
        string Brand { get; set; }
        int? Type { get; set; }
    }
}