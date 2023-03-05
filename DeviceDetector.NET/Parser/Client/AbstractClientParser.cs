using System.Collections.Generic;
using System.Linq;
using DeviceDetectorNET.Class.Client;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Client;

namespace DeviceDetectorNET.Parser.Client
{
    public abstract class AbstractClientParser<T> : AbstractParser<T, ClientMatchResult>, IAbstractClientParser
        where T : class, IEnumerable<IClientParseLibrary>
        // where TResult : class, IClientMatchResult, new()

    {
        protected AbstractClientParser()
        {

        }

        protected AbstractClientParser(string ua, ClientHints clientHints = null) : base(ua, clientHints)
        {

        }

        /**
        * Parses the current UA and checks whether it contains any client information
        *
        * @see $fixtureFile for file with list of detected clients
        *
        * Step 1: Build a big regex containing all regexes and match UA against it
        * -> If no matches found: return
        * -> Otherwise:
        * Step 2: Walk through the list of regexes in feed_readers.yml and try to match every one
        * -> Return the matched feed reader
        *
        * NOTE: Doing the big match before matching every single regex speeds up the detection
        *
        */
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
