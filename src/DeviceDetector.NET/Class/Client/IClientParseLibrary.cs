namespace DeviceDetector.NET.Class.Client
{
    public interface IClientParseLibrary : IParseLibrary
    {
        string Version { get; set; }
    }
}