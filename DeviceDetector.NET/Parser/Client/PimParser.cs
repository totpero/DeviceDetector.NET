using System.Collections.Generic;
using DeviceDetectorNET.Class.Client;

namespace DeviceDetectorNET.Parser.Client
{
    public class PimParser : ClientParserAbstract<List<Pim>>
    {
        public PimParser()
        {
            FixtureFile = "regexes/client/pim.yml";
            ParserName = "pim";
            regexList = GetRegexes();
        }
    }
}