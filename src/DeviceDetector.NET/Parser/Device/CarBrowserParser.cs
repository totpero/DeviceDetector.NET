using System.Collections.Generic;
using DeviceDetector.NET.Class.Device;
using DeviceDetector.NET.Results;
using DeviceDetector.NET.Results.Device;

namespace DeviceDetector.NET.Parser.Device
{
    public class CarBrowserParser : DeviceParserAbstract<IDictionary<string, DeviceModel>, DeviceMatchResult>
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
