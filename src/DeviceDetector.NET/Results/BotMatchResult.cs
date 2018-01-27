using DeviceDetector.NET.Class;

namespace DeviceDetector.NET.Results
{
    public class BotMatchResult:IMatchResult
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Url { get; set; }
        public Producer Producer { get; set; }

    }
}