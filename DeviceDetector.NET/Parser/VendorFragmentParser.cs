using System.Collections.Generic;
using System.Linq;
using DeviceDetectorNET.Parser.Device;
using DeviceDetectorNET.Results;

namespace DeviceDetectorNET.Parser
{
    public class VendorFragmentParser : AbstractParser<Dictionary<string,string[]>, VendorFragmentResult>
    {
        public VendorFragmentParser()
        {
            FixtureFile = "regexes/vendorfragments.yml";
            ParserName = "vendorfragments";
            regexList = GetRegexes();
        }

        public override ParseResult<VendorFragmentResult> Parse()
        {
            var result = new ParseResult<VendorFragmentResult>();
            foreach (var brands in regexList)
            {
                foreach (var brand in brands.Value)
                {
                    if (IsMatchUserAgent(brand + "[^a-z0-9]+"))
                    {
                        result.Add(new VendorFragmentResult
                        {
                            Name = brands.Key,
                            Brand = Devices.DeviceBrands
                                .FirstOrDefault(d => d.Value.Equals(brands.Key)).Key
                        });
                    }
                }
            }
            return result;
        }
    }
}
