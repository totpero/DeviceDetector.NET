namespace DeviceDetectorNET.Results.Client
{
    public class ClientMatchResult:IClientMatchResult
    {
        public virtual string Type { get; set; }
        public virtual string Name { get; set; }
        public string Version { get; set; }
    }
}