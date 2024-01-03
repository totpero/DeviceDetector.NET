using System.Collections.Generic;
using DeviceDetectorNET.Class.Client;
using DeviceDetectorNET.Parser.Client.Hints;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Client;

namespace DeviceDetectorNET.Parser.Client
{
    public class MobileAppParser : AbstractClientParser<List<MobileApp>>
    {
        public const string AppParserName = "mobile app";

        /// <summary>
        /// AppHints | null
        /// </summary>
        private AppHints appHints;

        private void Init()
        {
            FixtureFile = "regexes/client/mobile_apps.yml";
            ParserName = AppParserName;
            regexList = GetRegexes();
        }
        private MobileAppParser()
        {
            Init();
        }

        public MobileAppParser(string ua = "", ClientHints clientHints = null) 
            : base(ua, clientHints)
        {
            Init();
            appHints = new AppHints(ua, clientHints);
        }

        /// <summary>
        /// Sets the client hints to parse
        /// </summary>
        public override void SetClientHints(ClientHints clientHints)
        {
            base.SetClientHints(clientHints);
            appHints.SetClientHints(clientHints);
        }

        /// <summary>
        /// Sets the user agent to parse
        /// </summary>
        /// <param name="ua"></param>
        public override void SetUserAgent(string ua)
        {
            base.SetUserAgent(ua);
            appHints.SetUserAgent(ua);
        }
        public override ParseResult<ClientMatchResult> Parse()
        {
            var result = base.Parse();
            var name = result?.Match?.Name ?? string.Empty;
            var version = result?.Match?.Version ?? string.Empty;
            var appHash = appHints.Parse();
            if (appHash.Success && appHash.Match.Name != name)
            {
                name = appHash.Match.Name;
                version = string.Empty;
            }

            result = new ParseResult<ClientMatchResult>();
            if (string.IsNullOrEmpty(name))
                return result;

            result.Add(new ClientMatchResult {  
                Type = ParserName , 
                Name = name, 
                Version = version 
            });
            return result;
        }
    }
}