using System.Collections.Generic;
using DeviceDetectorNET.Class.Client;
using DeviceDetectorNET.Results.Client;

namespace DeviceDetectorNET.Parser.Client
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