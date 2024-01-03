using System;
using System.Collections.Generic;
using System.Linq;
using DeviceDetectorNET.Class.Client;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Client;

namespace DeviceDetectorNET.Parser.Client.Browser.Engine
{
    public class VersionParser : AbstractClientParser<List<IClientParseLibrary>>
    {
        private readonly string _engine;
        public VersionParser(string ua, string engine):base(ua)
        {
            _engine = engine;
        }

        public override ParseResult<ClientMatchResult> Parse()
        {
            var result = new ParseResult<ClientMatchResult>();
            if (string.IsNullOrEmpty(_engine))
            {
                return result;
            }

            string[] matches;

            if (string.Compare(_engine, "Gecko", StringComparison.OrdinalIgnoreCase) == 0)
            {
                matches = GetRegexEngine()
                    .MatchesUniq(UserAgent, @"[ ](?:rv[: ]([0-9\.]+)).*gecko/[0-9]{2,10}").ToArray();
            }
            else
            {
                var engineToken = _engine;

                if (string.Compare(_engine, "Blink", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    engineToken = "Chrome|Cronet";
                }

                matches = GetRegexEngine()
                    .MatchesUniq(UserAgent,
                        $@"(?:{engineToken})\s*/?\s*((?(?=\d+\.\d)\d+[.\d]*|\d{{1,7}}(?=(?:\D|$))))").ToArray();
            }

            if (matches.Length <= 0) return result;

            foreach (var match in matches)
            {
                result.Add(new ClientMatchResult { Name = match });
            }
            return result;
        }
    }
}