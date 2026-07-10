# CacheBuilder Tool

[`DeviceDetector.NET.CacheBuilder`](https://github.com/totpero/DeviceDetector.NET/tree/master/DeviceDetector.NET.CacheBuilder) is a standalone CLI tool that pre-parses a list of known User-Agent strings and writes the results into the [persistent LiteDB parse cache](Caching#2-persistent-parse-result-cache-parsecache) ahead of time. Ship the resulting `.db` file with your application to avoid paying the regex-parsing cost for your most common/expected UAs on first request.

This is conceptually similar to warming a cache from an access-log-derived UA list, a technique documented for the [upstream PHP project](https://github.com/matomo-org/device-detector#listing-all-user-agents-from-your-logs) — you can reuse the same approach to build your input file:

```bash
zcat /path/to/access/logs* | awk -F'"' '{print $6}' | sort | uniq -c | sort -rn | head -n20000 > top-user-agents.txt
```

## Usage

```
DeviceDetector.NET.CacheBuilder -i <input-file> [options]
```

| Option | Short | Default | Description |
|---|---|---|---|
| `--input` | `-i` | *(required)* | Text file with one User-Agent string per line |
| `--output` | `-o` | `DeviceDetector.db` | Output cache database filename |
| `--yaml` |  | *(bundled rules)* | Directory of YAML regex rule files to use instead of the bundled ones |
| `--append` | `-a` | `false` | Append to an existing cache database instead of overwriting it |
| `--cacheTimeoutDays` |  | `365` | How many days entries stay valid before expiring |
| `--skipBotDetection` |  | `false` | Apply `DeviceDetector.SkipBotDetection()` while building the cache (must match how you configure `DeviceDetector` at runtime — the flag affects the cache key) |

### Example

```bash
dotnet DeviceDetector.NET.CacheBuilder.dll -i top-user-agents.txt -o DeviceDetector.db --cacheTimeoutDays 90
```

This produces `DeviceDetector.db`. At runtime, point your app at the same directory/filename via `DeviceDetectorSettings` and set a matching (or longer) expiration so the pre-built entries remain valid:

```csharp
using DeviceDetectorNET;

DeviceDetectorSettings.ParseCacheDBDirectory = @"C:\app\cache\";
DeviceDetectorSettings.ParseCacheDBFilename = "DeviceDetector.db";
DeviceDetectorSettings.ParseCacheDBExpiration = TimeSpan.FromDays(90);
```

## Important: keep settings consistent

Because the persistent cache key incorporates parsing settings (notably `SkipBotDetection`), the flags used when **building** the cache must match the flags your application uses when **consuming** it — otherwise cached entries are effectively invisible (a cache miss) at runtime, silently falling back to live parsing. If you change `SkipBotDetection` or the YAML rule set version, rebuild the cache.

See [Caching](Caching) for how this fits alongside the fragment cache and the in-memory LRU cache.
