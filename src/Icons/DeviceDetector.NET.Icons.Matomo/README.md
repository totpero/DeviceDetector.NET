DeviceDetector.NET.Icons.Matomo
================================

Resolves icon file paths for [DeviceDetector.NET](https://github.com/totpero/DeviceDetector.NET)
detection results — browsers, operating systems and device brands/types — against the official
[matomo-icons](https://github.com/matomo-org/matomo-icons) pack's real folder layout.

This package does **not** bundle icon image assets (matomo-icons is GPLv3-licensed; this package is
Apache-2.0). Point it at your own checkout of matomo-icons' `dist/` folder.

## Folder convention

Unlike [`DeviceDetector.NET.Icons`](../DeviceDetector.NET.Icons/README.md) (which drives the Simbiat pack's
hierarchical, full-name-keyed layout), matomo-icons uses flat folders with a mixed key scheme:

| Category | Folder | Key |
|---|---|---|
| Browser | `browsers/` | DeviceDetector short code (`FF`, `AA`, ...) |
| OS | `os/` | DeviceDetector short code (`WIN`, `AND`, ...) |
| Brand | `brand/` | Full brand name (`Acer`, `Apple`, ...) — **not** a short code |
| Device type | `devices/` | Snake_case type name (`smartphone`, `car_browser`, ...) |
| Bot | *(none)* | matomo-icons has no bot category — always resolves to the fallback icon |

There are no intermediate fallback tiers (no family/engine/category subfolders) — an unresolved lookup goes
straight to `FallbackIconPath`.

Note: matomo-icons' own `brand/` folder isn't perfectly consistent either — a small number of brand names
(e.g. `"Sony Ericsson"`, `"Land Rover"`, `"Barnes & Noble"`) are stored under underscore-separated or
otherwise-altered filenames rather than the exact full name. `MatomoIconResolverOptions`'s default
`NameReplacements` already covers the known cases; if you find one that's missing, add it to your own
`NameReplacements` override the same way.

## Install

```
dotnet add package DeviceDetector.NET.Icons.Matomo
```

## Usage

```csharp
using DeviceDetectorNET;
using DeviceDetectorNET.Icons.Matomo;

var resolver = new MatomoIconResolver(new MatomoIconResolverOptions
{
    // Folder that directly contains matomo-icons' browsers/, os/, brand/ and devices/ subfolders
    // — typically a checkout of matomo-icons' dist/ folder.
    PhysicalRootPath = "/var/www/wwwroot/assets/images/devicedetector"
});

var parseResult = DeviceDetector.GetInfoFromUserAgent(userAgentString);
var icons = resolver.GetIcons(parseResult.Match);

// icons.OsIcon, icons.ClientIcon (browser icon, or the fallback for non-browser clients), icons.BrandIcon, icons.DeviceTypeIcon
// icons.BotIcon is always null/fallback — matomo-icons has no bot icons.
```

Or register it for dependency injection:

```csharp
using DeviceDetectorNET.Icons.Matomo;

builder.Services.AddMatomoDeviceDetectorIcons(options =>
{
    options.PhysicalRootPath = Path.Combine(builder.Environment.WebRootPath, "assets/images/devicedetector");
});
```

Individual lookups are also available:

```csharp
resolver.GetBrowser("FF");        // Firefox, by DeviceDetector short code
resolver.GetOs("WIN");            // Windows, by DeviceDetector short code
resolver.GetBrand("Apple");       // by full brand name
resolver.GetDeviceType("car browser"); // converted to devices/car_browser via NameReplacements
```

### Hosting icons behind an `IFileProvider` or CDN

```csharp
var resolver = new MatomoIconResolver(new MatomoIconResolverOptions
{
    FileExists = relativePath => fileProvider.GetFileInfo(relativePath).Exists,
    UrlBasePath = "https://cdn.example.com/devicedetector-icons"
});
```

## Keeping this package's assumptions in sync with upstream

See [docs/icon-packs-sync.md](https://github.com/totpero/DeviceDetector.NET/blob/master/docs/icon-packs-sync.md)
for the workflow used to re-verify matomo-icons' folder/naming conventions as it evolves.
