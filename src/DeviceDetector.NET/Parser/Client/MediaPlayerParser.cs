using System.Collections.Generic;
using DeviceDetector.NET.Class.Client;
using DeviceDetector.NET.Results.Client;

namespace DeviceDetector.NET.Parser.Client
{
    public class MediaPlayerParser : ClientParserAbstract<List<MediaPlayer>, ClientMatchResult>
    {
        public MediaPlayerParser()
        {
            FixtureFile = "regexes/client/mediaplayers.yml";
            ParserName = "mediaplayer";
            regexList = GetRegexes();
        }
    }
}