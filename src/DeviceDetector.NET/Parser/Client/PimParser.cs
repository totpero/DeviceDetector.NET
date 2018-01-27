using System.Collections.Generic;
using DeviceDetector.NET.Class.Client;
using DeviceDetector.NET.Results.Client;

namespace DeviceDetector.NET.Parser.Client
{
    public class PimParser : ClientParserAbstract<List<Pim>, ClientMatchResult>
    {
        public PimParser()
        {
            FixtureFile = "regexes/client/pim.yml";
            ParserName = "pim";
            regexList = GetRegexes();
        }
    }
}