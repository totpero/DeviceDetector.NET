# Updating Device Detector Data

DeviceDetector.NET is a manual port of [matomo-org/device-detector](https://github.com/matomo-org/device-detector) (PHP). The detection **rules** (YAML regex files) and **test fixtures** are the actual source of truth for what gets detected — they're copied over from upstream, not written from scratch. The **parsing code** (C# parsers) is a manual translation of the PHP parser logic and needs to be kept in sync by hand whenever upstream changes parsing behavior, not just data.

## The PHP submodule

The upstream project is vendored as a git submodule at [`device-detector/`](https://github.com/totpero/DeviceDetector.NET/tree/master/device-detector) in the repository root — it is not published or shipped, it exists purely as a reference copy to diff against.

```bash
# fetch new upstream tags
git -C device-detector fetch

# move the submodule checkout to a specific upstream tag
git -C device-detector checkout <tag>
```

## Update workflow

1. **Check current vs. latest upstream version**
   ```bash
   git -C device-detector tag --sort=-creatordate | head -5
   ```
2. **Diff what changed between the currently-vendored tag and the target tag**, file by file, e.g.:
   ```bash
   git -C device-detector diff <old_tag>...<new_tag> -- regexes/bots.yml
   git -C device-detector diff <old_tag>...<new_tag> -- Parser/
   ```
3. **Copy updated YAML rule files** from `device-detector/regexes/` into the equivalent path under `DeviceDetector.NET/regexes/` (same relative structure: `bots.yml`, `oss.yml`, `vendorfragments.yml`, `client/browsers.yml`, `device/mobiles.yml`, etc.). A direct diff is a good sanity check before/after copying:
   ```bash
   diff device-detector/regexes/bots.yml DeviceDetector.NET/regexes/bots.yml
   ```
4. **Port PHP parser/logic changes to C#** — anything in `device-detector/Parser/**/*.php` that changed behavior (not just data) needs a matching hand-written change in the corresponding `DeviceDetector.NET/Parser/**/*.cs` file. This is the step that requires the most care/judgment; there's no automated translation.
5. **Copy updated test fixtures** from `device-detector/Tests/fixtures/` into `DeviceDetector.NET.Tests/fixtures/`:
   ```bash
   diff device-detector/Tests/fixtures/ DeviceDetector.NET.Tests/fixtures/
   ```
   If `bots.yml` added new fields, `BotFixture.cs` (and other fixture DTOs) may need matching new properties.
6. **Rebuild and run the full test suite**:
   ```bash
   dotnet test DeviceDetector.NET.Tests/DeviceDetector.NET.Tests.csproj -c Release --nologo
   ```

## Things that trip people up

- **YAML files are embedded resources.** A change to a `.yml` file under `DeviceDetector.NET/regexes/` requires a full rebuild before it takes effect — running against a stale build will silently keep using the old rules.
- **Tests run in parallel** (`Parallel.ForEach` across fixtures) — a failure surfaces as an `AggregateException` wrapping possibly several individual assertion failures; read the whole aggregate, not just the first line.
- **Some pre-existing fixture mismatches are known/expected** rather than regressions (e.g. certain brand short-codes for Sony/Toshiba tablets have historically differed) — check whether a failing test was already failing before your change before assuming you broke something.

## Reporting detection bugs

If a specific User-Agent is mis-detected and you've confirmed **upstream PHP produces the same (wrong) result**, the fix belongs in the [upstream project](https://github.com/matomo-org/device-detector/issues) — DeviceDetector.NET will pick it up on the next sync. Only open an issue on DeviceDetector.NET directly if the .NET port disagrees with what the PHP version (or the vendored fixtures) says it should return — that's a porting bug, not a rules bug. See [FAQ](FAQ) for how to tell the difference.
