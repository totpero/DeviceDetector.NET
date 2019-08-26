using System.Collections.Generic;

namespace DeviceDetectorNET.RegexEngine
{
    public interface IRegexEngine
    {
        bool Match(string input, string pattern);
        IEnumerable<string> Matches(string input, string pattern);
        IEnumerable<string> MatchesUniq(string input, string pattern);
        string Replace(string input, string pattern, string replacement);
    }
}