using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using DeviceDetectorNET.Parser;
using DeviceDetectorNET.Tests.Class.Client;
using DeviceDetectorNET.Yaml;
using Xunit;

namespace DeviceDetectorNET.Tests.Parser
{
    [Trait("Category", "Vendor")]
    public class VendorFragmentTest
    {
        private readonly List<VendorFragmentFixture> _fixtureData;

        public VendorFragmentTest()
        {
            var path = $"{Utils.CurrentDirectory()}\\{@"Parser\fixtures\vendorfragments.yml"}";

            var parser = new YamlParser<List<VendorFragmentFixture>>();
            _fixtureData = parser.ParseFile(path);

            //replace null
            _fixtureData = _fixtureData.Select(f =>
            {
                f.vendor ??= string.Empty;
                return f;
            }).ToList();
        }

        [Fact]
        public void VendorFragmentTestParse()
        {
            var vendorFragmentParser = new VendorFragmentParser();
            foreach (var fixture in _fixtureData)
            {
                vendorFragmentParser.SetUserAgent(fixture.useragent);
                var result = vendorFragmentParser.Parse();
                result.Success.Should().BeTrue("Match should be with success");

                // The vendor fragment parser returns the full brand name directly (the YAML key),
                // matching the PHP reference implementation (VendorFragment::parse returns ['brand' => $brand]).
                result.Match.Brand.Should()
                                 .BeEquivalentTo(fixture.vendor, "Brands should be equal");
            }
        }
    }
}
