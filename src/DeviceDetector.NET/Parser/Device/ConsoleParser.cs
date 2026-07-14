using System.Collections.Generic;
using DeviceDetectorNET.Class.Device;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Device;

namespace DeviceDetectorNET.Parser.Device
{
    public class ConsoleParser : AbstractDeviceParser<IDictionary<string, DeviceModel>>
    {
        public ConsoleParser()
        {
            FixtureFile = "regexes/device/consoles.yml";
            ParserName = "consoles";
            regexList = GetRegexes();
        }

        public override ParseResult<DeviceMatchResult> Parse()
        {
            var result = new ParseResult<DeviceMatchResult>();
            return PreMatchOverall() ? base.Parse() : result;
        }
    }
}