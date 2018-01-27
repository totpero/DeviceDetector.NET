using DeviceDetector.NET.Results.Client;

namespace DeviceDetector.NET.Results
{
    public class DeviceDetectorResult
    {
        public DeviceDetectorResult()
        {
            OsFamily = "Unknown";
            BrowserFamily = "Unknown";
        }
        public string UserAgent { get; set; }
        public BotMatchResult Bot { get; set; }
        public OsMatchResult Os { get; set; }
        public ClientMatchResult Client { get; set; }
        public string DeviceType { get; set; }
        public string DeviceBrand { get; set; }
        public string DeviceModel { get; set; }
        public string OsFamily { get; set; }
        public string BrowserFamily { get; set; }
    }
}