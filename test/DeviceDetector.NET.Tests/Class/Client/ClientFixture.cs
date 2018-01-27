namespace DeviceDetector.NET.Tests.Class.Client
{
    public class ClientFixture
    {
        public string user_agent { get; set; }
        public Client client { get; set; }
        public class Client
        {
            public string type { get; set; }
            public string name { get; set; }
            public string version { get; set; }
        }
    }
}