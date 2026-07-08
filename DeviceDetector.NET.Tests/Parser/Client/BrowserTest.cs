using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using DeviceDetectorNET.Parser.Client;
using DeviceDetectorNET.Tests.Class.Client;
using DeviceDetectorNET.Yaml;
using System.Linq;
using System.Threading.Tasks;
using DeviceDetectorNET.Results.Client;

namespace DeviceDetectorNET.Tests.Parser.Client;

[Trait("Category", "Browser")]
public class BrowserTest
{
    private readonly List<BrowserFixture> _fixtureData;

    public BrowserTest()
    {
        var path = $"{Utils.CurrentDirectory()}\\{@"Parser\Client\fixtures\browser.yml"}";

        var parser = new YamlParser<List<BrowserFixture>>();
        _fixtureData = parser.ParseFile(path);

        //replace null
        _fixtureData = _fixtureData.Select(f =>
        {
            f.client.version ??= string.Empty;
            f.client.engine ??= string.Empty;
            f.client.engine_version ??= string.Empty;
            return f;
        }).ToList();
    }

    [Fact]
    public void TestGetAvailableBrowserFamilies()
    {
        BrowserParser.GetAvailableBrowserFamilies().Count.Should().BeGreaterThan(5);
    }

    [Fact]
    public void TestAllBrowsersTested()
    {
        BrowserParser.SetVersionTruncation(BrowserParser.VERSION_TRUNCATION_NONE);

        Parallel.ForEach(_fixtureData, fixture =>
        {
            var browsers = new BrowserParser();
            browsers.SetUserAgent(fixture.user_agent);
            if (fixture.headers != null)
            {
                browsers.SetClientHints(ClientHints.Factory(fixture.headers));
            }
            var result = browsers.Parse();

            result.Success.Should().BeTrue("Match should be with success");
            var browserResult = result.Match as BrowserMatchResult;

            browserResult.Should().NotBeNull("Match should be of type BrowserMatchResult");

            if (browserResult == null) return;

            browserResult.Engine.Should()
                .BeEquivalentTo(fixture.client.engine, "Engine should be equal " + fixture.user_agent);
            browserResult.EngineVersion.Should().BeEquivalentTo(fixture.client.engine_version.ToString(),
                "EngineVersion should be equal");
            browserResult.Name.Should().BeEquivalentTo(fixture.client.name, "Names should be equal");
            browserResult.Type.Should().BeEquivalentTo(fixture.client.type, "Type should be equal");
            browserResult.Version.Should().BeEquivalentTo(fixture.client.version, "Version should be equal");
        });
    }

    [Fact]
    public void TestGetAvailableClients()
    {
        var available = new BrowserParser().GetAvailableClients();
        BrowserParser.GetAvailableBrowsers().Count.Should().BeGreaterThanOrEqualTo(available.Count);
    }

    [Fact]
    public void TestShortCodesComparisonWithBrowsers()
    {
        var availableBrowsers = BrowserParser.GetAvailableBrowsers();
        var missing = BrowserParser.GetAvailableBrowserFamilies()
            .SelectMany(family => family.Value)
            .Where(shortCode => !availableBrowsers.ContainsKey(shortCode))
            .Distinct()
            .ToList();

        missing.Should().BeEmpty("these shortcodes do not match the list of browsers");
    }

    [Fact]
    public void TestStructureBrowsersYml()
    {
        var assembly = typeof(BrowserParser).Assembly;
        using var resource = assembly.GetManifestResourceStream("DeviceDetectorNET.regexes.client.browsers.yml");
        resource.Should().NotBeNull();

        var parser = new YamlParser<List<DeviceDetectorNET.Class.Client.Browser>>();
        var items = parser.ParseStream(resource);

        items.Should().NotBeEmpty();
        foreach (var item in items)
        {
            item.Regex.Should().NotBeNull("key \"regex\" must exist on every browsers.yml entry");
            item.Name.Should().NotBeNull("key \"name\" must exist on every browsers.yml entry");
            item.Version.Should().NotBeNull("key \"version\" must exist on every browsers.yml entry (regex: {0})", item.Regex);
        }
    }

    [Fact]
    public void TestBrowserFamiliesNoDuplicates()
    {
        foreach (var family in BrowserParser.GetAvailableBrowserFamilies())
        {
            var duplicates = family.Value
                .GroupBy(shortCode => shortCode)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToList();

            duplicates.Should().BeEmpty("family {0} contains duplicate shortcodes", family.Key);
        }
    }

    [Fact]
    public void TestBrowserHintsForAvailableBrowsers()
    {
        var browserHints = new DeviceDetectorNET.Parser.Client.Hints.BrowserHints(string.Empty);
        var regexListField = GetRegexListField(browserHints.GetType());
        var hints = (Dictionary<string, string>)regexListField.GetValue(browserHints);

        hints.Should().NotBeEmpty();
        foreach (var name in hints.Values)
        {
            BrowserParser.GetBrowserShortName(name).Should().NotBeNullOrEmpty(
                "browser name \"{0}\" from hints must be present in AvailableBrowsers", name);
        }
    }

    private static System.Reflection.FieldInfo GetRegexListField(System.Type type)
    {
        while (type != null)
        {
            var field = type.GetField("regexList",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null) return field;
            type = type.BaseType;
        }
        return null;
    }

    [Fact]
    public void TestEngineVerison()
    {
        BrowserParser.SetVersionTruncation(BrowserParser.VERSION_TRUNCATION_NONE);
        var browsers = new BrowserParser();
        browsers.SetUserAgent("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.80 Safari/537.36");

        //browsers.SetClientHints(ClientHints.Factory(
        //    new Dictionary<string, string>
        //{
        //        {"Sec-CH-UA",@"""(Not(A:Brand"";v=""8.0.0.0"", ""WaveBrowser"";v=""1.1.6.4"", ""WaveBrowser"";v=""1.1.6.4""" },
        //        {"Sec-CH-UA-Platform","Windows" },
        //        {"Sec-CH-UA-Mobile","?0" },
        //}));
        var result = browsers.Parse();
        var browserResult = result.Match as BrowserMatchResult;

        browserResult.Name.Should().NotBeEmpty();

    }

    [Fact]
    public void TestB()
    {
        BrowserParser.SetVersionTruncation(BrowserParser.VERSION_TRUNCATION_NONE);
        var browsers = new BrowserParser();
        browsers.SetUserAgent("Mozilla/5.0 (Macintosh; U; PPC; en-US; mimic; rv:9.3.0) Clecko/20120101 Classilla/CFM");

        var result = browsers.Parse();

        result.Success.Should().BeTrue("Match should be with success");

        if (result.Match is not BrowserMatchResult browserResult) return;

        browserResult.Name.Should().NotBeEmpty();
        browserResult.Version.Should().BeEmpty("Version should be empty when the regex captures nothing");
    }
}