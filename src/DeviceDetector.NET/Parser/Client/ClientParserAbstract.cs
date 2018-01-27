using System.Collections.Generic;
using System.Linq;
using DeviceDetector.NET.Class.Client;
using DeviceDetector.NET.Results;
using DeviceDetector.NET.Results.Client;

namespace DeviceDetector.NET.Parser.Client
{
    public abstract class ClientParserAbstract<T, TResult> : ParserAbstract<T, TResult>, IClientParserAbstract
        where T : class, IEnumerable<IClientParseLibrary>
        where TResult : class, IClientMatchResult, new()

    {
        protected ClientParserAbstract()
        {

        }

        protected ClientParserAbstract(string ua) : base(ua)
        {

        }

        public new virtual ParseResult<TResult> Parse()
        {
            var result = new ParseResult<TResult>();
            if (!PreMatchOverall()) return result;

            foreach (var regex in regexList)
            {
                var matches = MatchUserAgent(regex.Regex);

                if (matches.Length > 0)
                {
                    var match = new TResult
                    {
                        Type = ParserName,
                        Name = BuildByMatch(regex.Name, matches),
                        Version = BuildVersion(regex.Version, matches)
                    };

                    result.Add(match);

                }
            }

            return result;
        }

        public List<string> GetAvailableClients()
        {

            return regexList.Where(r => r.Name != "$1").Select(r => r.Name).Distinct().OrderBy(o => o).ToList();
        }
    }
}
