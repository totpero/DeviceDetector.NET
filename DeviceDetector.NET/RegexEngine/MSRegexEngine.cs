using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DeviceDetectorNET.RegexEngine
{
    /// <summary>
    /// Microsoft Regex Engine for DeviceDetector.Net
    /// </summary>
    public class MsRegexEngine: IRegexEngine
    {
        public bool Match(string input, string pattern)
        {
            var match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
            return match.Success;
        }

        public IEnumerable<string> Matches(string input, string pattern)
        {
            var matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase);
            return matches.Cast<Match>().SelectMany(m => m.Groups.Cast<Group>().Select(g => g.Value));
        }

        public IEnumerable<string> MatchesUniq(string input, string pattern)
        {
            var matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                foreach (Group group in match.Groups)
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
            return Regex.Replace(input, pattern, replacement);
        }
    }
}