using DeviceDetectorNET.Class;

namespace DeviceDetectorNET.Results
{
    public class BotMatchResult: IBotMatchResult
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Url { get; set; }
        public Producer Producer { get; set; }

    }
}