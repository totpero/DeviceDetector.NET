using DeviceDetectorNET.Class.Client;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Client;
using System.Xml.Linq;

namespace DeviceDetectorNET.Parser.Client.Hints
{
    public class AppHints : AbstractParser<HintsDictionary, HintsResult>
    {
        private AppHints()
        {
            FixtureFile = "regexes/client/hints/apps.yml";
            ParserName = "AppHints";
            regexList = GetRegexes();
        }

        public AppHints(string ua, ClientHints clientHints = null)
            :base(ua, clientHints)
        {

        }

        /// <summary>
        ///  Get application name if is in collection
        /// </summary>
        /// <returns></returns>
        public override ParseResult<HintsResult> Parse()
        {

            if (null == this.ClientHints) {
                return null;
            }

            var appId = this.ClientHints.GetApp();
            var name = this.regexList[appId] ?? null;

            if (string.IsNullOrEmpty(name)) {
                return null;
            }
            return new ParseResult<HintsResult>(new HintsResult { Name = name });
        }
    }
}
