using System.Collections.Generic;
using DeviceDetectorNET.Class.Device;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Device;

namespace DeviceDetectorNET.Parser.Device
{
    public class HbbTvParser : AbstractDeviceParser<IDictionary<string, DeviceModel>>
    {
        private const string Regex = @"(?:HbbTV|SmartTvA)/([1-9]{1}(?:\.[0-9]{1}){1,2})";
        public HbbTvParser()
        {
            FixtureFile = "regexes/device/televisions.yml";
            ParserName = "tv";
            regexList = GetRegexes();
        }

        /// <summary>
        /// Parses the current UA and checks whether it contains HbbTv or SmartTvA information
        /// </summary>
        /// <returns></returns>
        public override ParseResult<DeviceMatchResult> Parse()
        {
            var result = new ParseResult<DeviceMatchResult>();

            // only parse user agents containing fragments: hbbtv or SmartTvA
            if (!IsHbbTv()) return result;

            result = base.Parse();

            // always set device type to tv, even if no model/brand could be found
            if (!deviceType.HasValue)
                deviceType = DeviceType.DEVICE_TYPE_TV;

            if (result.Success) return result;

            result.Add(GetResult());
            return result;
        }

        public bool IsHbbTv()
        {
            return IsMatchUserAgent(Regex);
        }

        public string HbbTv()
        {
            var match = MatchUserAgent(Regex);
            return match.Length > 1 ? match[1] : string.Empty;
        }
    }
}