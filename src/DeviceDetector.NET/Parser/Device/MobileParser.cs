using System.Collections.Generic;
using DeviceDetectorNET.Class.Device;
using DeviceDetectorNET.Results.Device;

namespace DeviceDetectorNET.Parser.Device
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