namespace DeviceDetector.NET.Results.Client
{
    public interface IClientMatchResult : IMatchResult
    {
        string Type { get; set; }
        string Version { get; set; }
    }
}