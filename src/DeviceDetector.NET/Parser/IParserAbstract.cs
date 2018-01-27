using DeviceDetector.NET.Cache;

namespace DeviceDetector.NET.Parser
{
    public interface IParserAbstract
    {
        string FixtureFile { get; }
        string ParserName { get; }

        void SetUserAgent(string ua);
        void SetCache(ICache cacheProvider);
    }
}
