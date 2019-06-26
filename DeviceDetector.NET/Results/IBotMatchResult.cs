using DeviceDetectorNET.Class;

namespace DeviceDetectorNET.Results
{
    public interface IBotMatchResult : IMatchResult
    {
        string Category { get; set; }
        string Url { get; set; }
        Producer Producer { get; set; }
    }
}