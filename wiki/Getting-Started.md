# Getting Started

## Installation

Add the [`DeviceDetector.NET`](https://www.nuget.org/packages/DeviceDetector.NET) NuGet package to your project:

```
dotnet add package DeviceDetector.NET
```

The core package targets `netstandard2.0`, `net462`, `net8.0`, `net9.0` and `net10.0`, so it works from classic .NET Framework projects up to the latest .NET.

## Basic usage

```csharp
using DeviceDetectorNET;

// OPTIONAL: return full version numbers instead of only major.minor
// (default truncates to VERSION_TRUNCATION_MINOR); see VERSION_TRUNCATION_*
// constants on DeviceDetectorNET.Parser.VersionTruncation
// using DeviceDetectorNET.Parser;
DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);

var userAgent = Request.Headers["User-Agent"]; // the UA string you want to parse
var headers = Request.Headers.ToDictionary(a => a.Key, a => a.Value.ToArray().FirstOrDefault());
var clientHints = ClientHints.Factory(headers); // Client Hints are optional, see Client-Hints page

var dd = new DeviceDetector(userAgent, clientHints);

// OPTIONAL: set a caching method — see the Caching page
// using DeviceDetectorNET.Cache;
dd.SetCache(new DictionaryCache());

// OPTIONAL: GetBot() only returns non-null if a bot was detected (skips extra parsing)
dd.DiscardBotInformation();

// OPTIONAL: skip bot detection entirely — bots are then parsed as regular devices
dd.SkipBotDetection();

dd.Parse();

if (dd.IsBot())
{
    // handle bots, spiders, crawlers, ...
    var botInfo = dd.GetBot();
}
else
{
    var clientInfo = dd.GetClient();   // browser, feed reader, media player, PIM, ...
    var osInfo = dd.GetOs();
    var device = dd.GetDeviceName();
    var brand = dd.GetBrandName();
    var model = dd.GetModel();
}
```

## One-shot helper

If you don't need to tweak any options, `DeviceDetector.GetInfoFromUserAgent` runs the whole pipeline in one call and returns a `ParseResult<DeviceDetectorResult>`:

```csharp
using DeviceDetectorNET;

var result = DeviceDetector.GetInfoFromUserAgent(userAgent, clientHints);

if (result.Success)
{
    var info = result.Match; // DeviceDetectorResult
}
```

This is exactly what the bundled [demo API](ASP.NET-Core-Integration) and the [Serilog enricher](ASP.NET-Core-Integration) use internally.

## OS family / browser family helpers

```csharp
using DeviceDetectorNET.Parser;
var osFamily = OperatingSystemParser.GetOsFamily(dd.GetOs().Match?.Name);

using DeviceDetectorNET.Parser.Client;
var browserFamily = BrowserParser.GetBrowserFamily(dd.GetBrowserClient().Match?.Name);
```

## Custom regex rules directory

The library ships the YAML regex rules under `regexes/` next to the assembly. To point at your own copy (e.g. a newer checkout of the upstream `device-detector` rules):

```csharp
using DeviceDetectorNET;

DeviceDetectorSettings.RegexesDirectory = @"C:\YamlRegexsFiles\";
```

## Next steps

- [Device and Client Type Checks](Device-and-Client-Type-Checks) — all the `IsSmartphone()`, `IsBrowser()`, etc. helpers
- [Client Hints](Client-Hints) — improving accuracy on modern Chromium browsers
- [Caching](Caching) — avoid re-parsing the same User-Agent repeatedly
- [Bot Detection](Bot-Detection) — cheap bot-only detection path
