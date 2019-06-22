using DeviceDetectorNET.Class;
using System;

namespace DeviceDetectorNET.Results
{
    public class BotMatchResult: IBotMatchResult
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Url { get; set; }
        public Producer Producer { get; set; }

        public override string ToString() =>
        $"Category: {Category}; " +
        $"{Environment.NewLine} " +
        $"Name: {Name};" +
        $"{Environment.NewLine} " +
        $"Url: {Url};" +
        $"{Environment.NewLine} " +
        $"Producer: {Producer?.Name};";
    }
}