using DeviceDetectorNET.Cache;
using DeviceDetectorNET.Results;

namespace DeviceDetectorNET.Parser
{
    public interface IAbstractParser <TResult> where TResult : class, IMatchResult, new()
    {
        string FixtureFile { get; }
        string ParserName { get; }

        void SetUserAgent(string ua);
        void SetClientHints(ClientHints clientHints = null);
        void SetCache(ICache cacheProvider);

        ParseResult<TResult> Parse();
    }
}
