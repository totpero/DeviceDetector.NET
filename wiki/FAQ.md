# FAQ

### Is a wrong detection a DeviceDetector.NET bug or a rules issue?

First check whether the [upstream PHP project](https://github.com/matomo-org/device-detector) gets the same result for the same UA (e.g. via their online demo/tests, or by running the PHP library directly). Two cases:

- **PHP also gets it wrong** → it's a rules issue; report it upstream at [matomo-org/device-detector/issues](https://github.com/matomo-org/device-detector/issues). DeviceDetector.NET will inherit the fix on the next [data sync](Updating-Device-Detector-Data).
- **PHP gets it right, .NET doesn't** → it's a porting bug in this repository; please open an issue here with the User-Agent string and the expected vs. actual result.

### Why does `GetOs()` / `GetClient()` return an empty/unsuccessful result?

Make sure you called `dd.Parse()` before reading any getter — nothing is parsed until `Parse()` runs. Also check `ParseResult<T>.Success` before touching `.Match`; an unmatched UA returns `Success == false` with `Match == null`.

### Why do version numbers look truncated (e.g. "16" instead of "16.0.3")?

By default, versions are truncated to `VERSION_TRUNCATION_MINOR` (major.minor). Call this once, before parsing:

```csharp
DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);
```

See the `VERSION_TRUNCATION_*` constants in `DeviceDetectorNET.Parser.VersionTruncation` for the other levels (major only, major.minor, major.minor.patch, full/none).

### Do I need Client Hints for the library to work?

No — Client Hints are entirely optional. Pass `null` (or an empty `ClientHints.Factory(...)` result) and detection falls back to the User-Agent string alone. See [Client Hints](Client-Hints) for when they help.

### Calling an `IsX()` helper throws `AccessViolationException` — why?

Every `Is*()` boolean helper (and `Is(ClientType)`) requires `Parse()` to have already run. Call `dd.Parse()` before any `dd.IsSmartphone()`, `dd.IsBrowser()`, etc.

### How do I avoid re-parsing the same User-Agent over and over?

See the dedicated [Caching](Caching) page — there are three complementary layers (fragment cache, persistent LiteDB parse cache, in-memory LRU cache of `DeviceDetector` instances).

### How do I use this via dependency injection — I just want `@inject`-and-call `IsMobile()`?

There's no built-in `IServiceCollection` extension (this was the core complaint in [issue #83](https://github.com/totpero/DeviceDetector.NET/issues/83)), but wrapping it yourself is a few lines — see [ASP.NET Core Integration → Dependency injection pattern](ASP.NET-Core-Integration#dependency-injection-pattern) for a ready-to-use `IDeviceDetectionService` you can register as a singleton and inject anywhere.

### Can I use this outside ASP.NET Core?

Yes — the core `DeviceDetector.NET` package has no ASP.NET Core dependency; it just needs a `User-Agent` string (and optionally a headers dictionary for Client Hints). The ASP.NET Core-specific pieces ([demo API](ASP.NET-Core-Integration), Serilog enricher) are separate, optional packages/projects.

### Where do the detected OS/browser/library lists come from?

The full, auto-generated lists of every detected OS, browser, browser engine and library are kept up to date in the project [README](https://github.com/totpero/DeviceDetector.NET#readme) rather than duplicated here, since they're regenerated from the YAML rule set periodically and would otherwise drift.

### Is there a hosted/online demo?

The [`DeviceDetector.NET.Web`](ASP.NET-Core-Integration) project is a minimal local demo you can run yourself; there's no public hosted instance.
