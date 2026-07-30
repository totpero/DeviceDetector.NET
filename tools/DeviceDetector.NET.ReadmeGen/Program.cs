using System.Text;
using System.Text.RegularExpressions;
using DeviceDetectorNET.Parser;
using DeviceDetectorNET.Parser.Client;
using DeviceDetectorNET.Parser.Client.Browser;
using DeviceDetectorNET.Parser.Device;

// Regenerates the "## What Device Detector is able to detect" section of README.md
// from the library's own data, mirroring device-detector's misc/updateReadme.php.

string FindRepoRoot()
{
    var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
    while (dir != null && !File.Exists(Path.Combine(dir.FullName, "DeviceDetector.NET.sln")))
        dir = dir.Parent;
    return dir?.FullName ?? Directory.GetCurrentDirectory();
}

int NatCompare(string a, string b)
{
    var ra = Regex.Matches(a, @"\d+|\D+").Select(m => m.Value).ToArray();
    var rb = Regex.Matches(b, @"\d+|\D+").Select(m => m.Value).ToArray();
    int len = Math.Min(ra.Length, rb.Length);
    for (int i = 0; i < len; i++)
    {
        string pa = ra[i], pb = rb[i];
        bool na = int.TryParse(pa, out int ia);
        bool nb = int.TryParse(pb, out int ib);
        int cmp = na && nb ? ia.CompareTo(ib) : string.Compare(pa, pb, StringComparison.OrdinalIgnoreCase);
        if (cmp != 0) return cmp;
    }
    return ra.Length.CompareTo(rb.Length);
}

List<string> Sorted(IEnumerable<string> items)
{
    var list = items.Distinct().ToList();
    list.Sort(NatCompare);
    return list;
}

var repoRoot = FindRepoRoot();
var readmePath = Path.Combine(repoRoot, "README.md");
var botsFixturePath = Path.Combine(repoRoot, "tests", "DeviceDetector.NET.Tests", "fixtures", "bots.yml");

var sb = new StringBuilder();
void Line(string s = "") => sb.Append(s).Append('\n');

Line("## What Device Detector is able to detect");
Line();
Line("The lists below are auto generated and updated from time to time. Some of them might not be complete.");
Line();
Line($"*Last update: {DateTime.UtcNow:yyyy/MM/dd}*");
Line();

void Section(string title, IEnumerable<string> items, string? footnote = null)
{
    Line($"### {title}:");
    Line();
    sb.Append(string.Join(", ", Sorted(items)));
    if (footnote != null)
        sb.Append(footnote);
    Line();
    Line();
}

Section("List of detected operating systems", OperatingSystemParser.GetAvailableOperatingSystems().Values);
Section("List of detected browsers", BrowserParser.GetAvailableBrowsers().Values);
Section("List of detected browser engines", EngineParser.GetAvailableEngines());
Section("List of detected libraries", new LibraryParser().GetAvailableClients());
Section("List of detected media players", new MediaPlayerParser().GetAvailableClients());
Section(
    "List of detected mobile apps",
    new MobileAppParser().GetAvailableClients(),
    " and *mobile apps using [AFNetworking](https://github.com/AFNetworking/AFNetworking) or [Electron](https://github.com/electron/electron)*"
);
Section("List of detected PIMs (personal information manager)", new PimParser().GetAvailableClients());
Section("List of detected feed readers", new FeedReaderParser().GetAvailableClients());
Section("List of brands with detected devices", Devices.DeviceBrands.Values);

var botsYaml = File.ReadAllText(botsFixturePath);
var botNames = Regex.Matches(botsYaml, @"^    name: (.+)$", RegexOptions.Multiline)
    .Select(m => m.Groups[1].Value.Trim())
    .Select(n => (n.StartsWith('"') && n.EndsWith('"')) || (n.StartsWith('\'') && n.EndsWith('\''))
        ? n.Substring(1, n.Length - 2)
        : n)
    .Where(n => n != "Generic Bot");
Line("### List of detected bots:");
Line();
sb.Append(string.Join(", ", Sorted(botNames)));
Line();

var hasBom = File.ReadAllBytes(readmePath) is [0xEF, 0xBB, 0xBF, ..];
var readme = File.ReadAllText(readmePath);
var markers = new[]
{
    "## What Device Detector is able to detect",
    "The lists below are auto generated and updated from time to time.",
};
var markerIndex = markers
    .Select(m => readme.IndexOf(m, StringComparison.Ordinal))
    .Where(i => i >= 0)
    .DefaultIfEmpty(-1)
    .Min();
var head = markerIndex >= 0 ? readme[..markerIndex] : readme.TrimEnd('\r', '\n') + "\n\n";

File.WriteAllText(readmePath, head + sb, new UTF8Encoding(hasBom));
Console.WriteLine($"README.md detection lists regenerated ({readmePath}).");
