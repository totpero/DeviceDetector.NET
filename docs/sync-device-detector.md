# Sync with upstream device-detector

Working notes for [issue #107](https://github.com/totpero/DeviceDetector.NET/issues/107). The general
workflow is described in [wiki/Updating-Device-Detector-Data.md](../wiki/Updating-Device-Detector-Data.md).

## Upstream range

| | Commit | `git describe` |
|---|---|---|
| Previous pin | `15d1f109` | `6.5.1-45-g15d1f109` |
| New pin | `486b0c3d` | `6.5.2-42-g486b0c3d` |

95 upstream commits. Before this sync the rule files, the test fixtures and the lookup tables (brands,
browsers, operating systems, ...) matched the previous pin and the test suite passed 225/225.

## Status

- [x] Bump the `device-detector` submodule
- [x] Copy the updated regex YAML files (10 files) and test fixtures (65 files, `tv-6.yml` is new)
- [x] Port the parser changes
- [x] Port the new upstream tests
- [x] Verify the rules and the chunked matching, run the tests of the whole solution
- [x] Update the README detection lists and the wiki
- [x] Commit and open a pull request (branch `sync-device-detector-6.5.2`)
- [ ] Review and merge the pull request, then delete the branch
- [ ] Decide the package version and release notes

Where to continue: the pull request of the branch above, then the open points at the end of this file.

## Ported behaviour changes

| Area | Change |
|---|---|
| `DeviceDetector.SetClientHints` | Without client hints (or without an app in them), a `FBAB/` token in the user agent builds client hints with that app id. `GetInfoFromUserAgent` now always calls `SetClientHints`, as the PHP helper does. |
| `DeviceDetector.IsMobile` | Returns `false` for bots, even if their client hints say mobile. |
| Device type | An iOS 13+ device without a detected type is a phablet, with model `iPhone` if none was found. |
| Base parser | `GetRegexesForUserAgent` pre-matches the regexes in chunks (bots, operating systems, clients, devices). |
| Base parser | The desktop fragment now includes `Macintosh; Intel Mac OS X` and `X11; CrOS x86_64`, and excludes `Steam`. The client hints fragment has a new Android pattern. |
| User agent regexes | `/` is no longer escaped (PHP switched to the `#` delimiter). |
| Browsers | New: 360 Speed Browser, Mullvad Browser, Norton Neo, Stadium, Startpage, Teak Browser. Families, mobile only list and client hint mappings (`Copilot`, `Neo`) updated. The user agent engine version is only used if the engine is the same. |
| Operating systems | New: FortiOS, HyperOS, MyOS, SteamOS, YaOS, YodaOS-Master. Client hint mappings (`Mac OS X`, `MacIntel`, `Win32`), Lineage OS 24, Android short code `AND` for the Android only browser apps. |
| Devices | 23 new brands, `Shift Phones` renamed to `SHIFT`. A brand level `model` ends the model matching. |

PHP changes without a .NET counterpart: type and style cleanups in `ClientHints`, `AppHints`,
`BrowserHints`, `HbbTv`, `VendorFragment` and the PHP tooling.

Visible for library users: `Shift Phones` is now `SHIFT`; upstream also renamed `RoboForm` to
`RoboForm Passwort Manager` and removed `Facebook Audience Network` and `iOS Application`.

## .NET specific decisions

- **Chunk size is 10, not 100.** The results are the same for every size, only the speed differs. Warm,
  single threaded, microseconds per user agent (1033 fixture user agents):

  | Parser | Linear (before) | 10 | 100 (PHP) |
  |---|---|---|---|
  | operating system | 562 | 150 | 390 |
  | browser | 2313 | 791 | 1383 |
  | mobile device brands | 3918 | 2633 | 7274 |
  | bot | 2808 | 2688 | 2813 |
  | mobile app | 1637 | 1612 | 1517 |

  With 100 the brand parser, the most expensive one, is almost twice as slow as the linear scan. The one
  advantage of 100 is a faster very first parse when all parsers run (about 6.5 s, 17 s with 10 and
  23-28 s linear).
- **`AbstractParser.SetClientHints(null)` now clears the hints**, like PHP. Before, a reused
  `DeviceDetector` instance kept the hints of the previous parse in its parsers. With the new `FBAB` rule
  those hints can exist without the caller passing any, so a Chrome user agent was reported as
  `Facebook` (test `TestReusedInstanceDoesNotKeepClientHints`).
- `GetRegexesForUserAgent` falls back to single regexes on any exception of the regex engine. PHP checks
  `preg_last_error()`; in .NET each engine (built in, PCRE) reports its limits with its own exception type.
- The iOS 13 check uses `VersionComparer`, so `13` is lower than `13.0`, like PHP `version_compare`.
- `DeviceDetector.VERSION` stays `6.5.2`, the same as upstream. The package version and release notes are
  not changed by this sync.

## Verification

- All 20917 rule regexes and every chunk compile with the .NET engine.
- On 41298 fixture user agents, each of the 16 parsers returns the same result with and without chunks.
- `dotnet test DeviceDetector.NET.sln -c Release`: 326 passed (library 236, icon packs 87, Serilog 3).
- Lookup tables compared with PHP by script: no differences.
- Full `GetInfoFromUserAgent` on 1920 fixture user agents, old build against new build, two runs on the
  same machine: about 30% less time per user agent (11.9 to 8.5 ms and 6.6 to 4.4 ms), a faster first
  parse and about 30 MB (9%) more peak memory. Absolute times varied a lot between the runs.

## Open points

- Decide the package version and release notes before publishing a new NuGet package.
- Older differences from PHP, left as they are (not part of the upstream changes): the .NET bot and client
  parsers collect every match instead of stopping at the first one, and OS `versions` entries cannot
  override the `name` (no current rule does).
