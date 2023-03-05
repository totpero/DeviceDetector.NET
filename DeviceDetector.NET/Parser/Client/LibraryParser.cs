using System.Collections.Generic;
using DeviceDetectorNET.Class.Client;
using DeviceDetectorNET.Results.Client;

namespace DeviceDetectorNET.Parser.Client
{
    public class LibraryParser : AbstractClientParser<List<Library>>
    {
        public LibraryParser()
        {
            FixtureFile = "regexes/client/libraries.yml";
            ParserName = "library";
            regexList = GetRegexes();
        }
    }
}