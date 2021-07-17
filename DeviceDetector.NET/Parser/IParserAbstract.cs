using DeviceDetectorNET.Cache;
using DeviceDetectorNET.Results;

namespace DeviceDetectorNET.Parser
{
    public interface IParserAbstract <TResult> where TResult : class, IMatchResult, new()
    {
        string FixtureFile { get; }
        string ParserName { get; }

        void SetUserAgent(string ua);
        void SetCache(ICache cacheProvider);

        ParseResult<TResult> Parse();
    }
}
