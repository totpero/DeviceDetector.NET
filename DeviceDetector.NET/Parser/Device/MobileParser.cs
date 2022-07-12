using System.Collections.Generic;
using DeviceDetectorNET.Class.Device;
using DeviceDetectorNET.Results.Device;

namespace DeviceDetectorNET.Parser.Device
{
    public class MobileParser : AbstractDeviceParser<IDictionary<string, DeviceModel>>
    {
        public MobileParser()
        {
            FixtureFile = "regexes/device/mobiles.yml";
            ParserName = "mobiles";
            regexList = GetRegexes();
        }
    }
}