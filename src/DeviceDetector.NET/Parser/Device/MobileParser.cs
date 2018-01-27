using System.Collections.Generic;
using DeviceDetector.NET.Class.Device;
using DeviceDetector.NET.Results.Device;

namespace DeviceDetector.NET.Parser.Device
{
    public class MobileParser : DeviceParserAbstract<IDictionary<string, DeviceModel>, DeviceMatchResult>
    {
        public MobileParser()
        {
            FixtureFile = "regexes/device/mobiles.yml";
            ParserName = "mobiles";
            regexList = GetRegexes();
        }
    }
}