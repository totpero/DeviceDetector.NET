using System.Collections.Generic;
using DeviceDetectorNET.Class.Client;
using DeviceDetectorNET.Results.Client;

namespace DeviceDetectorNET.Parser.Client
{
    public class MobileAppParser : ClientParserAbstract<List<MobileApp>>
    {
        public MobileAppParser()
        {
            FixtureFile = "regexes/client/mobile_apps.yml";
            ParserName = "mobile app";
            regexList = GetRegexes();
        }
    }
}