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

            // Gecko/Clecko expose the engine version through the "rv:" token only when accompanied by an
            // 8-10 digit Gecko build number. If that specific pattern matches we use it, otherwise we fall
            // through to the generic engine-token matching (mirroring the PHP reference implementation).
            if (_engine.Equals("Gecko", StringComparison.OrdinalIgnoreCase) || _engine.Equals("Clecko", StringComparison.OrdinalIgnoreCase))
            {
                var geckoMatches = GetRegexEngine()
                    .MatchesUniq(UserAgent, "[ ](?:rv[: ]([0-9.]+)).*(?:g|cl)ecko/\\d{8,10}").ToArray();
                if (geckoMatches.Length > 0)
                {
                    result.Add(new ClientMatchResult { Name = geckoMatches[0] });
                    return result;
                }
            }

            var engineToken = _engine;

            if (_engine.Equals("Blink", StringComparison.OrdinalIgnoreCase))
            {
                engineToken = "Chr[o0]me|Chromium|Cronet";
            }

            if (_engine.Equals("Arachne", StringComparison.OrdinalIgnoreCase))
            {
                engineToken = "Arachne\\/5\\.";
            }

            if (_engine.Equals("LibWeb", StringComparison.OrdinalIgnoreCase))
            {
                engineToken = "LibWeb\\+LibJs";
            }

            var matches = GetRegexEngine()
                .MatchesUniq(UserAgent,
                    $@"(?:{engineToken})\s*[/_]?\s*((?(?=\d+\.\d)\d+[.\d]*|\d{{1,7}}(?=(?:\D|$))))").ToArray();

            if (matches.Length <= 0) return result;

            // PHP uses preg_match (first occurrence) and array_pop (the single captured group).
            result.Add(new ClientMatchResult { Name = matches.First() });
            return result;
        }
    }
}