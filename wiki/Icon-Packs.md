# Icon Packs

DeviceDetector.NET can resolve icon file paths (bots, browsers, operating systems, clients, device
brands/types) for a detection result, so a web app can render a matching icon next to "you're using
Chrome on Windows" style UI. This is split across three small packages under `src/Icons/`:

| Package | Purpose |
|---|---|
| [`DeviceDetector.NET.Icons`](https://github.com/totpero/DeviceDetector.NET/tree/master/src/Icons/DeviceDetector.NET.Icons) | Resolver for the unofficial [Simbiat/DeviceDetectorIcons](https://github.com/Simbiat/DeviceDetectorIcons) pack's hierarchical, full-name-keyed layout |
| [`DeviceDetector.NET.Icons.Matomo`](https://github.com/totpero/DeviceDetector.NET/tree/master/src/Icons/DeviceDetector.NET.Icons.Matomo) | Resolver for the official [matomo-icons](https://github.com/matomo-org/matomo-icons) pack's flat folder layout |
| [`DeviceDetector.NET.Icons.Common`](https://github.com/totpero/DeviceDetector.NET/tree/master/src/Icons/DeviceDetector.NET.Icons.Common) | Shared primitives (`IIconProbeOptions`, `IconPathProber`, options validation) used by both resolvers above — not meant to be referenced directly |

**Neither resolver package bundles icon image assets.** You point the resolver at your own copy of an
icon pack on disk (or behind an `IFileProvider`/CDN); matomo-icons is GPLv3-licensed and Simbiat's pack
has its own license, so shipping the images inside an Apache-2.0 NuGet package isn't appropriate. Pick
one pack, install the matching resolver package, and use the other project's README only for background
if you're curious about the alternative convention.

## Which one to use

- **Official Matomo icons** (recommended if you don't already have an icon set) → install
  `DeviceDetector.NET.Icons.Matomo`, and check out https://github.com/matomo-org/matomo-icons `dist/`
  folder locally (or serve it from a CDN).
- **Simbiat's DeviceDetectorIcons pack**, or any pack that follows its folder convention → install
  `DeviceDetector.NET.Icons`.

The two are not interchangeable — each resolver's fallback chain and folder-key scheme is hardcoded to
match its specific pack.

## `DeviceDetector.NET.Icons` (Simbiat convention)

Folder layout expected under `PhysicalRootPath`:

```
bot/                  bot/category/
client/type/          client/browser/       client/browser/family/   client/browser/engine/
client/os/            client/os/family/
device/brand/         device/type/
```

Fallback chain per category (first existing file wins):

- **Browser**: `client/browser/<name>` → `client/browser/family/<family>` → `client/browser/engine/<engine>` → `client/type/browser`
- **OS**: `client/os/<name>` → `client/os/family/<family>` → `client/type/os`
- **Client** (non-browser — feed readers, media players, PIMs, libraries, mobile apps): `client/<type>/<name>` → `client/type/<type>`
- **Brand**: `device/brand/<brand>` → `device/type/<deviceType>`
- **Bot**: `bot/<name>` → `bot/category/<category>`

```csharp
using DeviceDetectorNET;
using DeviceDetectorNET.Icons;

var resolver = new IconResolver(new IconResolverOptions
{
    PhysicalRootPath = "/var/www/wwwroot/assets/images/devicedetector"
});

var parseResult = DeviceDetector.GetInfoFromUserAgent(userAgentString);
var icons = resolver.GetIcons(parseResult.Match);
// icons.BotIcon, icons.OsIcon, icons.ClientIcon, icons.BrandIcon, icons.DeviceTypeIcon
```

DI registration:

```csharp
builder.Services.AddDeviceDetectorIcons(options =>
{
    options.PhysicalRootPath = Path.Combine(builder.Environment.WebRootPath, "assets/images/devicedetector");
});
```

Default `ExtensionPriority`: `svg, avif, webp, png, jpg, jpeg, jxl, heic, gif`. Default `NameReplacements`
covers filesystem-unsafe upstream names (`OS/2` → `OS2`, `GNU/Linux` → `GNULinux`, etc.).

The resolution algorithm is a C# port of Simbiat/DeviceDetectorIcons' `DDCIcons.php` (MIT licensed,
© 2021 Dmitrii Kustov) — see the package README's [Attribution](https://github.com/totpero/DeviceDetector.NET/blob/master/src/Icons/DeviceDetector.NET.Icons/README.md#attribution) section for the full license text.

## `DeviceDetector.NET.Icons.Matomo` (Matomo convention)

matomo-icons uses flat folders with a mixed key scheme — no bot category, no intermediate fallback tiers:

| Category | Folder | Key |
|---|---|---|
| Browser | `browsers/` | DeviceDetector short code (`FF`, `AA`, ...) |
| OS | `os/` | DeviceDetector short code (`WIN`, `AND`, ...) |
| Brand | `brand/` | Full brand name (`Acer`, `Apple`, ...) — not a short code |
| Device type | `devices/` | snake_case type name (`smartphone`, `car_browser`, ...) |
| Bot | *(none)* | matomo-icons has no bot icons — `GetBot()` always returns the fallback |

An unresolved lookup in any category goes straight to `FallbackIconPath` (no family/engine/category
fallback tiers exist here, unlike the Simbiat resolver above).

```csharp
using DeviceDetectorNET;
using DeviceDetectorNET.Icons.Matomo;

var resolver = new MatomoIconResolver(new MatomoIconResolverOptions
{
    // Folder that directly contains matomo-icons' browsers/, os/, brand/ and devices/ — typically
    // a checkout of matomo-icons' dist/ folder.
    PhysicalRootPath = "/var/www/wwwroot/assets/images/devicedetector"
});

var parseResult = DeviceDetector.GetInfoFromUserAgent(userAgentString);
var icons = resolver.GetIcons(parseResult.Match);
// icons.OsIcon, icons.ClientIcon, icons.BrandIcon, icons.DeviceTypeIcon
// icons.BotIcon is always null/fallback.
```

DI registration:

```csharp
builder.Services.AddMatomoDeviceDetectorIcons(options =>
{
    options.PhysicalRootPath = Path.Combine(builder.Environment.WebRootPath, "assets/images/devicedetector");
});
```

Default `ExtensionPriority`: `png, svg, jpg, gif, ico` (matomo-icons' `dist/` is currently PNG-only
end-to-end; the rest is headroom for a future upstream change). Default `NameReplacements` bridges:

- The five multi-word `Devices.GetDeviceName()` results to matomo-icons' snake_case filenames
  (`"car browser"` → `car_browser`, `"feature phone"` → `feature_phone`, `"smart display"` →
  `smart_display`, `"portable media player"` → `portable_media_player`, `"smart speaker"` →
  `smart_speaker`).
- ~25 `brand/` entries whose real filenames use underscores or altered characters instead of the exact
  brand name (`"Sony Ericsson"` → `Sony_Ericsson.png`, `"Barnes & Noble"` → `Barnes_Noble.png`, etc.).

## Both packages: hosting behind an `IFileProvider` or CDN

By default existence is checked with `File.Exists` against `PhysicalRootPath`. Set `FileExists` to back
it with something else instead (an ASP.NET Core `IFileProvider`, a CDN manifest, ...):

```csharp
var resolver = new MatomoIconResolver(new MatomoIconResolverOptions
{
    FileExists = relativePath => fileProvider.GetFileInfo(relativePath).Exists,
    UrlBasePath = "https://cdn.example.com/devicedetector-icons"
});
```

The same `FileExists`/`UrlBasePath` options exist on `IconResolverOptions` for the Simbiat resolver.

## Keeping resolver assumptions in sync with upstream packs

The maintainer keeps two **optional, large** git submodules under `icon-packs/` purely as read-only
references to re-verify each resolver's hardcoded folder/naming assumptions as the upstream packs
evolve — nothing under `icon-packs/` is ever shipped in a NuGet package, and neither submodule is
required to build or test this repo. See
[docs/icon-packs-sync.md](https://github.com/totpero/DeviceDetector.NET/blob/master/docs/icon-packs-sync.md)
for the full bump/re-verify workflow (folder set, key scheme, new `DeviceType`/`Devices` members,
extension set, `NameReplacements` coverage).

## Adding support for a new icon pack

`DeviceDetector.NET.Icons.Common` exists so a third resolver for a different pack convention can reuse
`IIconProbeOptions`, `IconResolverOptionsValidation` and `IconPathProber` without inheriting a shared
resolver base class (folder layouts and fallback rules differ too much between packs for that to help).
The convention to copy, from the Common package's README:

1. `<Pack>IconResolverOptions : IIconProbeOptions`, with pack-specific defaults for `ExtensionPriority`,
   `NameReplacements` and `FallbackIconPath`.
2. `<Pack>IconResolver`, constructed with `IconResolverOptionsValidation.Validate(...)` then a private
   `IconPathProber` built from `options.FileExists ?? IconResolverOptionsValidation.DefaultFileExists(...)`.
3. A `ServiceCollectionExtensions.Add<Pack>DeviceDetectorIcons(this IServiceCollection, Action<TOptions>)`
   extension method registering the resolver as a singleton, matching
   `DeviceDetectorNET.Icons.Matomo.ServiceCollectionExtensions.AddMatomoDeviceDetectorIcons`.
4. A `<Pack>.Tests` project mirroring `DeviceDetector.NET.Icons.Matomo.Tests` (construction, options
   defaults, lookups, `FileExists` override, result-type overloads, DI, end-to-end).
