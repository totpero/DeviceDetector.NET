using System.Collections.Generic;
using DeviceDetectorNET.Class.Device;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Device;

namespace DeviceDetectorNET.Parser.Device
{
    public class HbbTvParser : DeviceParserAbstract<IDictionary<string, DeviceModel>>
    {
        public HbbTvParser()
        {
            FixtureFile = "regexes/device/televisions.yml";
            ParserName = "tv";
            regexList = GetRegexes();
        }

        public override ParseResult<DeviceMatchResult> Parse()
        {
            var result = new ParseResult<DeviceMatchResult>();

            // only parse user agents containing hbbtv fragment
            if (!IsHbbTv()) return result;

            // always set device type to tv, even if no model/brand could be found
            deviceType = DeviceType.DEVICE_TYPE_TV;

            result = base.Parse();
            if (!result.Success)
            {
                result.Add(new DeviceMatchResult { Brand = string.Empty, Name = string.Empty, Type = deviceType });
            }
            return result;
        }

        public bool IsHbbTv()
        {
            var regex = @"HbbTV/([1-9]{1}(?:\.[0-9]{1}){1,2})";
            return IsMatchUserAgent(regex);
        }

        public string[] HbbTv()
        {
            var regex = @"HbbTV/([1-9]{1}(?:\.[0-9]{1}){1,2})";
            return MatchUserAgent(regex);
        }
    }
}