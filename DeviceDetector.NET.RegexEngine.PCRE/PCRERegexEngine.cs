using System.Collections.Generic;
using System.Linq;
using PCRE;

namespace DeviceDetectorNET.RegexEngine.PCRE;

public class PcreRegexEngine: IRegexEngine
{
    public bool Match(string input, string pattern)
    {
        var match = PcreRegex.Match(input, pattern);
        return match.Success;
    }

    public IEnumerable<string> Matches(string input, string pattern)
    {
        var matches = PcreRegex.Matches(input, pattern, PcreOptions.IgnoreCase);
        return matches.SelectMany(m => m.Groups.Select(g => g.Value));
    }

    public IEnumerable<string> MatchesUniq(string input, string pattern)
    {
        var matches = PcreRegex.Matches(input, pattern, PcreOptions.IgnoreCase);
        foreach (var match in matches)
        {
            foreach (var group in match.Groups)
            {
                if (!match.Value.Equals(group.Value))
                {
                    yield return group.Value;
                }
            }
        }
    }

    public string Replace(string input, string pattern, string replacement)
    {
        return PcreRegex.Replace(input, pattern, replacement);
    }
}