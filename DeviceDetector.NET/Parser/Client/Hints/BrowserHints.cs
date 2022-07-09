using DeviceDetectorNET.Class.Client;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Client;

namespace DeviceDetectorNET.Parser.Client.Hints
{
    public class BrowserHints : ParserAbstract<HintsDictionary, HintsResult>
    {
        private BrowserHints()
        {
            FixtureFile = "regexes/client/hints/browsers.yml";
            ParserName = "BrowserHints";
            regexList = GetRegexes();
        }

        public BrowserHints(string ua, ClientHints clientHints = null)
           : base(ua, clientHints)
        {

        }

        public override ParseResult<HintsResult> Parse()
        {

            if (null == this.ClientHints)
            {
                return null;
            }

            var appId = this.ClientHints.GetApp();
            var name = this.regexList[appId] ?? null;

            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            return new ParseResult<HintsResult>(new HintsResult { Name = name });
        }
    }
}
