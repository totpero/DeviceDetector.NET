using System;

namespace DeviceDetectorNET.Results
{
    public class OsMatchResult:IMatchResult
    {
        public virtual string Name { get; set; }
        public virtual string ShortName { get; set; }
        public string Version { get; set; }
        public string Platform { get; set; }

        public override string ToString() =>
        $"ShortName: {ShortName}; " +
        $"{Environment.NewLine} " +
        $"Name: {Name};" +
        $"{Environment.NewLine} " +
        $"Version: {Version};" +
        $"{Environment.NewLine} " +
        $"Platform: {Platform};" ;
    }
}