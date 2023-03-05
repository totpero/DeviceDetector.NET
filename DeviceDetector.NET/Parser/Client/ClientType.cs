namespace DeviceDetectorNET.Parser.Client
{
    public class ClientType
    {
        public static ClientType Browser => new ClientType(new BrowserParser());
        public static ClientType FeedReader => new ClientType(new FeedReaderParser());
        public static ClientType Library => new ClientType(new LibraryParser());
        public static ClientType MediaPlayer => new ClientType(new MediaPlayerParser());
        public static ClientType MobileApp => new ClientType(new MobileAppParser());
        public static ClientType PIM => new ClientType(new PimParser());

        private ClientType(IAbstractClientParser client)
        {
            Client = client;
            Name = client.ParserName;
            FixtureFile = client.FixtureFile;
        }

        public IAbstractClientParser Client { get; }
        public string Name { get; }
        public string FixtureFile { get; }
    }
}
