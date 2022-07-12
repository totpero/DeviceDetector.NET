using System.Collections.Generic;
using DeviceDetectorNET.Class.Device;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Device;

namespace DeviceDetectorNET.Parser.Device
{
    public class CarBrowserParser : AbstractDeviceParser<IDictionary<string, DeviceModel>>
    {
        public CarBrowserParser()
        {
            FixtureFile = "regexes/device/car_browsers.yml";
            ParserName = "car browser";
            regexList = GetRegexes();
        }

        public override ParseResult<DeviceMatchResult> Parse()
        {
            var result = new ParseResult<DeviceMatchResult>();
            return PreMatchOverall() ? base.Parse() : result;
        }
    }
}
