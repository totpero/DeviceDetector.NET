using System.Collections.Generic;
using DeviceDetector.NET.Class.Client;
using DeviceDetector.NET.Results.Client;

namespace DeviceDetector.NET.Parser.Client
{
    public class MobileAppParser : ClientParserAbstract<List<MobileApp>, ClientMatchResult>
    {
        public MobileAppParser()
        {
            FixtureFile = "regexes/client/mobile_apps.yml";
            ParserName = "mobile app";
            regexList = GetRegexes();
        }
    }
}