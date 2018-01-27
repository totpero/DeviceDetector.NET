using System.Collections.Generic;
using DeviceDetector.NET.Class.Device;
using DeviceDetector.NET.Results;
using DeviceDetector.NET.Results.Device;

namespace DeviceDetector.NET.Parser.Device
{
    public class ConsoleParser : DeviceParserAbstract<IDictionary<string, DeviceModel>, DeviceMatchResult>
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