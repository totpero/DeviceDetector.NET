namespace DeviceDetector.NET.Results
{
    public class OsMatchResult:IMatchResult
    {
        public virtual string Name { get; set; }
        public virtual string ShortName { get; set; }
        public string Version { get; set; }
        public string Platform { get; set; }
    }
}