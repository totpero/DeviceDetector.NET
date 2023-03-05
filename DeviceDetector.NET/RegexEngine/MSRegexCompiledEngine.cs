using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.RegularExpressions;

namespace DeviceDetectorNET.RegexEngine
{
    /// <summary>
    /// Microsoft Regex Engine for DeviceDetector.Net
    /// </summary>
    public class MSRegexCompiledEngine : IRegexEngine
    {
        private static Lazy<ConcurrentDictionary<string, Regex>> _staticRegExCache = new Lazy<ConcurrentDictionary<string, Regex>>();
        
        private Regex GetRegex(string pattern)
        {
            return _staticRegExCache.Value.GetOrAdd(pattern, (regexPattern) => new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled));
        }
        
        public bool Match(string input, string pattern)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            var match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
            return match.Success;
        }

        public IEnumerable<string> Matches(string input, string pattern)
        {
            var matches = GetRegex(pattern).Matches(input);
            return matches.Cast<Match>().SelectMany(m => m.Groups.Cast<Group>().Select(g => g.Value));
        }

        public IEnumerable<string> MatchesUniq(string input, string pattern)
        {
            var matches = GetRegex(pattern).Matches(input);
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
            return GetRegex(pattern).Replace(input, replacement);
        }
    }
}