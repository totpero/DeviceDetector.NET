namespace DeviceDetectorNET.Tests.Class
{
    public class BotFixture
    {
        public string user_agent { get; set; }
        public Bot bot { get; set; }
        public string client { get; set; }

        public class Bot
        {
            public Bot()
            {
                producer = new Producer();
            }
            public string name { get; set; }
            public string category { get; set; }
            public string url { get; set; }
            public string platform { get; set; }
            public Producer producer { get; set; }

            public class Producer
            {
                public Producer()
                {
                    name = "";
                    url = "";
                }
                public string name { get; set; }
                public string url { get; set; }
            }
        }
    }
}
