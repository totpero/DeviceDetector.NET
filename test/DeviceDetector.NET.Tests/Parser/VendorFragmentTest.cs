using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using DeviceDetector.NET.Parser;
using DeviceDetector.NET.Tests.Class.Client;
using DeviceDetector.NET.Yaml;
using Xunit;

namespace DeviceDetector.NET.Tests.Parser
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
                f.vendor = f.vendor ?? "";
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

                result.Match.Brand.ShouldBeEquivalentTo(fixture.vendor, "Brands should be equal");
            }
        }
    }
}
