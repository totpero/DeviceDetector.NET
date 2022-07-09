using System.Collections.Generic;

namespace DeviceDetectorNET.Tests.Class
{
    public class DeviceDetectorFixture
    {
        public string user_agent { get; set; }
        
        public object os { get; set; }
        public ClientDevice client { get; set; }
        public Device device { get; set; }
        public string os_family { get; set; }
        public string browser_family { get; set; }
        public Dictionary<string, string> headers { get; set; }
        public Bot bot { get; set; }

    }

    public class Os
    {
        public string name { get; set; }
        public string short_name { get; set; }
        public string version { get; set; }
        public string platform { get; set; }
    }

    public class ClientDevice
    {
        public string type { get; set; }
        public string name { get; set; }
        public string short_name { get; set; }
        public string version { get; set; }
        public string engine { get; set; }
        public string engine_version { get; set; }
    }

    public class Device
    {
        public string type { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
    }

    public class Bot
    {
        public string name { get; set; }
        public string category { get; set; }
        public string url { get; set; }

    }
}
