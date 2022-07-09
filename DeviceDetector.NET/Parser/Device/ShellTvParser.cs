using System.Collections.Generic;
using DeviceDetectorNET.Class.Device;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Device;

namespace DeviceDetectorNET.Parser.Device
{
    public class ShellTvParser : DeviceParserAbstract<IDictionary<string, DeviceModel>>
    {
        public ShellTvParser()
        {
            FixtureFile = "regexes/device/shell_tv.yml";
            ParserName = "shelltv";
            regexList = GetRegexes();
        }

        public override ParseResult<DeviceMatchResult> Parse()
        {
            var result = new ParseResult<DeviceMatchResult>();

            // only parse user agents containing Shelltv fragment
            if (!IsShellTv()) return result;

            // always set device type to tv, even if no model/brand could be found
            deviceType = DeviceType.DEVICE_TYPE_TV;

            result = base.Parse();

            if (result.Success) return result;

            result.Add(new DeviceMatchResult
                {Brand = string.Empty, Name = string.Empty, Type = deviceType});

            return result;
        }

        public bool IsShellTv()
        {
            const string regex = @"[a-z]+[ _]Shell[ _]\w{6}|tclwebkit(\d+[\.\d]*)";
            return IsMatchUserAgent(regex);
        }
    }
}