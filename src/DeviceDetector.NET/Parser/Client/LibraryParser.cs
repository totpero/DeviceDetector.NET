using System.Collections.Generic;
using DeviceDetector.NET.Class.Client;
using DeviceDetector.NET.Results.Client;

namespace DeviceDetector.NET.Parser.Client
{
    public class LibraryParser : ClientParserAbstract<List<Library>, ClientMatchResult>
    {
        public LibraryParser()
        {
            FixtureFile = "regexes/client/libraries.yml";
            ParserName = "library";
            regexList = GetRegexes();
        }
    }
}