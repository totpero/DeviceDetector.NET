namespace DeviceDetector.NET.Results.Client
{
    public class BrowserMatchResult : ClientMatchResult
    {
        public string ShortName { get; set; }
        public string Engine { get; set; }
        public string EngineVersion { get; set; }
    }
}