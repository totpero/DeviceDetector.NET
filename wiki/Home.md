# DeviceDetector.NET

[![Build status](https://ci.appveyor.com/api/projects/status/baf9r5iqnp4flwkm?svg=true)](https://ci.appveyor.com/project/totpero/devicedetector-net)
[![NuGet](https://img.shields.io/nuget/v/DeviceDetector.NET.svg?style=flat-square)](https://www.nuget.org/packages/DeviceDetector.NET)

**DeviceDetector.NET** is the universal device detection library for .NET. It parses HTTP `User-Agent` strings (and, optionally, [User-Agent Client Hints](https://developer.mozilla.org/en-US/docs/Web/HTTP/Client_hints)) to detect:

- **Devices** — desktop, smartphone, tablet, phablet, TV, console, car browser, camera, portable media player, wearable, smart display, smart speaker, peripheral
- **Clients** — browsers, feed readers, media players, mobile apps, PIMs, libraries
- **Operating systems** — name, version, family
- **Bots** — crawlers, spiders, monitoring tools
- **Brands and models** — for mobile/hardware devices

It is a faithful port of the popular PHP library [matomo-org/device-detector](https://github.com/matomo-org/device-detector), tracked as a git submodule ([`device-detector`](https://github.com/totpero/DeviceDetector.NET/tree/master/device-detector)) so the regex rule set stays in sync with upstream. **For most detection questions ("why wasn't X detected as Y"), the [upstream PHP documentation and issue tracker](https://github.com/matomo-org/device-detector) apply directly** — the rules themselves are shared, only the code that executes them differs.

## Where to start

| I want to... | Go to |
|---|---|
| Add the library to my project and parse my first User-Agent | [Getting Started](Getting-Started) |
| Check `Is*()` helpers (`IsSmartphone`, `IsBrowser`, ...) | [Device and Client Type Checks](Device-and-Client-Type-Checks) |
| Use Client Hints instead of / together with User-Agent | [Client Hints](Client-Hints) |
| Speed up repeated parsing with caching | [Caching](Caching) |
| Use a PCRE-compatible regex engine for better upstream YAML compatibility | [Regex Engines](Regex-Engines) |
| Detect bots without paying for full parsing | [Bot Detection](Bot-Detection) |
| Use it in ASP.NET Core app logic (DI, `IsMobile()` per request) | [ASP.NET Core Integration](ASP.NET-Core-Integration) |
| Render an icon (browser/OS/brand/device) next to a detection result | [Icon Packs](Icon-Packs) |
| Enrich ASP.NET Core logs with device info | [Serilog.Enrichers.AspNetCore.DeviceDetector](Serilog.Enrichers.AspNetCore.DeviceDetector) |
| Pre-warm a cache database for known User-Agents | [CacheBuilder Tool](CacheBuilder-Tool) |
| Update the bundled detection rules from the PHP project | [Updating Device Detector Data](Updating-Device-Detector-Data) |
| Report a bug or send a pull request | [Contributing](Contributing) |
| Something isn't detected correctly | [FAQ](FAQ) |

## Repository layout

- [`DeviceDetector.NET`](https://github.com/totpero/DeviceDetector.NET/tree/master/DeviceDetector.NET) — the core library (NuGet package `DeviceDetector.NET`)
- [`DeviceDetector.NET.RegexEngine.PCRE`](https://github.com/totpero/DeviceDetector.NET/tree/master/DeviceDetector.NET.RegexEngine.PCRE) — optional PCRE.NET-backed regex engine
- [`DeviceDetector.NET.CacheBuilder`](https://github.com/totpero/DeviceDetector.NET/tree/master/DeviceDetector.NET.CacheBuilder) — CLI tool to pre-build a parse-result cache
- [`DeviceDetector.NET.Web`](https://github.com/totpero/DeviceDetector.NET/tree/master/DeviceDetector.NET.Web) — minimal ASP.NET Core demo API
- [`src/Icons`](https://github.com/totpero/DeviceDetector.NET/tree/master/src/Icons) — icon-path resolver packages: `DeviceDetector.NET.Icons` (Simbiat pack convention), `DeviceDetector.NET.Icons.Matomo` (official matomo-icons convention), `DeviceDetector.NET.Icons.Common` (shared primitives) — see [Icon Packs](Icon-Packs)
- [`Serilog.Enrichers.AspNetCore.DeviceDetector`](https://github.com/totpero/DeviceDetector.NET/tree/master/Serilog.Enrichers.AspNetCore.DeviceDetector) — Serilog enricher that attaches device info to log events, with its own [`...Tests`](https://github.com/totpero/DeviceDetector.NET/tree/master/Serilog.Enrichers.AspNetCore.DeviceDetector.Tests) project
- [`DeviceDetector.NET.Tests`](https://github.com/totpero/DeviceDetector.NET/tree/master/DeviceDetector.NET.Tests) — xUnit + Shouldly test suite, mirrors upstream PHP fixtures
- [`device-detector`](https://github.com/totpero/DeviceDetector.NET/tree/master/device-detector) — git submodule pointing at the upstream PHP project (source of truth for YAML regex rules)

## License

DeviceDetector.NET is licensed under the **Apache License 2.0** — see [LICENSE](https://github.com/totpero/DeviceDetector.NET/blob/master/LICENSE). Note that the upstream PHP project it ports (`matomo-org/device-detector`) is licensed under **LGPL v3 or later**; if you redistribute the vendored YAML rule data itself, check compliance with the upstream license as well.
