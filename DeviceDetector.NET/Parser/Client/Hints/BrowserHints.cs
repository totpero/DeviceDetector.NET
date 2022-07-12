using DeviceDetectorNET.Class.Client;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Client;

namespace DeviceDetectorNET.Parser.Client.Hints
{
    public class BrowserHints : AbstractParser<HintsDictionary, HintsResult>
    {

        private void Init()
        {

            FixtureFile = "regexes/client/hints/browsers.yml";
            ParserName = "BrowserHints";
            regexList = GetRegexes();
        }
        private BrowserHints()
        {
            Init();
        }

        public BrowserHints(string ua, ClientHints clientHints = null)
           : base(ua, clientHints)
        {
            Init();
        }

        public override ParseResult<HintsResult> Parse()
        {
            var result = new ParseResult<HintsResult>();
            if (null == this.ClientHints)
                return result;

            var appId = this.ClientHints.GetApp();
            if (string.IsNullOrEmpty(appId))
                return result;

            var name = regexList.ContainsKey(appId) ? this.regexList[appId] : null;

            if (string.IsNullOrEmpty(name))
                return result;

            return result.Add(new HintsResult { Name = name });
        }
    }
}
