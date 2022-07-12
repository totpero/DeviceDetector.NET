using System.Collections.Generic;
using DeviceDetectorNET.Class.Client;

namespace DeviceDetectorNET.Parser.Client
{
    public class MediaPlayerParser : AbstractClientParser<List<MediaPlayer>>
    {
        public MediaPlayerParser()
        {
            FixtureFile = "regexes/client/mediaplayers.yml";
            ParserName = "mediaplayer";
            regexList = GetRegexes();
        }
    }
}