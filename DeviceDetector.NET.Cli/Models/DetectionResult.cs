namespace DeviceDetector.Net.Cli.Models
{
    public class DetectionResult
    {
        public string UserAgent { get; set; }
        public string DeviceType { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string OsName { get; set; }
        public string OsVersion { get; set; }
        public string ClientType { get; set; }
        public string ClientName { get; set; }
        public string ClientVersion { get; set; }
        public bool IsBot { get; set; }
        public string BotName { get; set; }
    }
}
