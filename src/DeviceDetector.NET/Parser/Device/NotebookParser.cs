using System.Collections.Generic;
using DeviceDetectorNET.Class.Device;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Device;

namespace DeviceDetectorNET.Parser.Device
{
    public class NotebookParser : AbstractDeviceParser<IDictionary<string, DeviceModel>>
    {
        public NotebookParser()
        {
            FixtureFile = "regexes/device/notebooks.yml";
            ParserName = "Notebooks";
            regexList = GetRegexes();
        }

        public override ParseResult<DeviceMatchResult> Parse()
        {
            var result = new ParseResult<DeviceMatchResult>();

            if (!IsMatchUserAgent("FBMD/"))
            {
                return result;
            }
            return PreMatchOverall() ? base.Parse() : result;
        }
    }
}