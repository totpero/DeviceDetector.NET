using System.Collections.Generic;
using System.Linq;
using DeviceDetectorNET.Class.Client;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Client;

namespace DeviceDetectorNET.Parser.Client
{
    public abstract class ClientParserAbstract<T> : ParserAbstract<T, ClientMatchResult>, IClientParserAbstract
        where T : class, IEnumerable<IClientParseLibrary>
        // where TResult : class, IClientMatchResult, new()

    {
        protected ClientParserAbstract()
        {

        }

        protected ClientParserAbstract(string ua) : base(ua)
        {

        }

        public new virtual ParseResult<ClientMatchResult> Parse()
        {
            var result = new ParseResult<ClientMatchResult>();
            if (!PreMatchOverall()) return result;

            foreach (var regex in regexList)
            {
                var matches = MatchUserAgent(regex.Regex);

                if (matches.Length > 0)
                {
                    var match = new ClientMatchResult
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
