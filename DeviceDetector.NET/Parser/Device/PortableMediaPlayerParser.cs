using System.Collections.Generic;
using DeviceDetectorNET.Class.Device;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Device;

namespace DeviceDetectorNET.Parser.Device
{
    public class PortableMediaPlayerParser : AbstractDeviceParser<IDictionary<string, DeviceModel>>
    {
        public PortableMediaPlayerParser()
        {
            FixtureFile = "regexes/device/portable_media_player.yml";
            ParserName = "portablemediaplayer";
            regexList = GetRegexes();
        }

        public override ParseResult<DeviceMatchResult> Parse()
        {
            var result = new ParseResult<DeviceMatchResult>();
            return PreMatchOverall() ? base.Parse() : result;
        }
    }
}