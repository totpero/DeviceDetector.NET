# Caching

Parsing a User-Agent string runs a large number of regular expressions against the YAML rule set, so it's relatively expensive. DeviceDetector.NET offers three independent caching layers — pick the ones that fit your workload; they compose.

## 1. In-process fragment cache (`ICache`)

`DeviceDetector.SetCache(...)` installs a cache used internally by the parsers to memoize intermediate results (mirrors the PHP `setCache()` / `CacheInterface`):

```csharp
using DeviceDetectorNET.Cache;

dd.SetCache(new DictionaryCache());
```

- **`DictionaryCache`** — the built-in default. Backed by a `ConcurrentDictionary<string, object>` shared statically across all `DeviceDetector` instances in the process (equivalent to PHP's static array cache). Good for a single process/app instance; not shared across machines or app restarts. It never evicts entries on its own, so unbounded UA cardinality means unbounded memory growth.
- **`BitFasterConcurrentLruCache`** — provided by the separate [`DeviceDetector.NET.Cache.BitFaster`](https://github.com/totpero/DeviceDetector.NET/tree/master/DeviceDetector.NET.Cache.BitFaster) project, backed by [BitFaster.Caching](https://github.com/bitfaster/BitFaster.Caching)'s `ConcurrentLru`. Use it instead of `DictionaryCache` when you want a bounded fragment cache that evicts least-recently-used entries once a capacity is reached, rather than growing forever:

  ```csharp
  using DeviceDetectorNET.Cache.BitFaster;

  dd.SetCache(new BitFasterConcurrentLruCache(capacity: 10_000));
  ```

  Like the [PCRE regex engine](Regex-Engines#pcreregexengine-optional-better-upstream-fidelity), this lives in its own project so the core package doesn't force the `BitFaster.Caching` dependency on consumers who don't need it.
- Implement `DeviceDetectorNET.Cache.ICache` (`Fetch`, `Contains`, `Save`, `Delete`, `FlushAll`) to plug in your own store (e.g. a distributed cache) if you need cross-process sharing — there is no built-in Redis/Memcached bridge as there is for the PHP PSR-6/PSR-16 bridges, so you provide the adapter.

## 2. Persistent parse-result cache (`ParseCache`)

A second, opt-in layer persists **full parse results** (not just internal fragments) to a local [LiteDB](https://www.litedb.org/) database file. This avoids re-running the regex pipeline entirely for a UA seen before, even across process restarts.

> **The cache key is `$"{userAgent}_{skipBotDetection}_{discardBotInformation}_{versionTruncation}"` — it does *not* include Client Hints.** If you parse the same User-Agent with different `ClientHints` (e.g. different `Sec-CH-UA-*` values across requests from otherwise-identical browsers), the second call reuses whatever was cached for the first, silently ignoring the new hints. If your traffic relies on Client Hints for accuracy, either disable this cache (leave `ParseCacheDBExpiration` at its default `TimeSpan.Zero`) or fold the relevant Client Hints values into your own cache key when wrapping `DeviceDetector` (see [ASP.NET Core Integration](ASP.NET-Core-Integration#dependency-injection-pattern) for a wrapper example). This is also why [`LRUCachedDeviceDetector`](#3-in-memory-lru-cache-of-whole-devicedetector-instances) below deliberately has no `ClientHints` parameter — it doesn't try to solve this and shouldn't be assumed to.

It's disabled by default (`ParseCacheDBExpiration = TimeSpan.Zero`). Enable it via `DeviceDetectorSettings`:

```csharp
using DeviceDetectorNET;

DeviceDetectorSettings.ParseCacheDBDirectory = @"C:\cache\";     // default: app directory
DeviceDetectorSettings.ParseCacheDBFilename = "DeviceDetectorNET.db"; // default filename
DeviceDetectorSettings.ParseCacheDBExpiration = TimeSpan.FromDays(365);
```

Once `ParseCacheDBExpiration` is non-zero, `Parse()` transparently checks this database before doing any regex work, and stores the result afterwards. Expired entries are purged automatically. This is the same mechanism used by the [CacheBuilder tool](CacheBuilder-Tool) to pre-warm a cache file you ship with your app.

## 3. In-memory LRU cache of whole `DeviceDetector` instances

For hot paths where you call `DeviceDetector` repeatedly for the same UAs within a single process (e.g. per-request logging), `LRUCachedDeviceDetector` keeps a bounded, time-limited in-memory cache of fully-parsed `DeviceDetector` instances:

```csharp
using DeviceDetectorNET.Cache;

var dd = LRUCachedDeviceDetector.GetDeviceDetector(userAgent);
```

Tune its size/duration through `DeviceDetectorSettings`:

```csharp
DeviceDetectorSettings.LRUCacheMaxSize = 10_000;              // default 10,000 entries
DeviceDetectorSettings.LRUCacheCleanPercentage = 30;          // % evicted once full (LRU-ish, by usage then age)
DeviceDetectorSettings.LRUCacheMaxDuration = TimeSpan.FromMinutes(10); // default 10 minutes
```

> `LRUCachedDeviceDetector` does not account for `SetVersionTruncation` — if you change version truncation at runtime, cached instances were parsed under whichever setting was active when they were added.

## SetCache vs LRUCachedDeviceDetector

This distinction confused enough people (see [issue #83](https://github.com/totpero/DeviceDetector.NET/issues/83)) that it's worth spelling out explicitly:

|  | `dd.SetCache(new DictionaryCache())` | `LRUCachedDeviceDetector.GetDeviceDetector(ua)` |
|---|---|---|
| What it caches | Internal regex-matching fragments *inside one parse* | Entire, already-parsed `DeviceDetector` **instances**, keyed by UA |
| Scope | Storage is a process-wide static dictionary, but you still create/own the `DeviceDetector` instance and call `Parse()` yourself | Returns a shared, ready-to-use instance — you never call `new DeviceDetector(...)` or `Parse()` yourself |
| Client Hints | Full support — pass them to the `DeviceDetector` constructor as usual | **Not supported** — the method signature only takes a `userAgent` string, so results are UA-only |
| When to use | Always harmless to call; it's cheap and the default anyway | When you only have a UA string (no Client Hints) and want to skip re-parsing entirely for repeat UAs within the process |

In short: `SetCache` does not replace the need to call `Parse()` — it just makes each `Parse()` call reuse intermediate matching work across instances. `LRUCachedDeviceDetector` is the one that skips `Parse()` altogether for a UA it has already seen, but at the cost of ignoring Client Hints. If you need both — Client Hints **and** whole-instance caching across requests — you have to write the equivalent of `LRUCachedDeviceDetector` yourself, using a cache key that includes whatever Client Hints values matter to you (see the [ASP.NET Core Integration](ASP.NET-Core-Integration#dependency-injection-pattern) page for a starting point).

## Which one should I use?

- **Just starting out / low traffic**: nothing extra needed, the default `DictionaryCache` fragment cache is enough.
- **High request volume, same process**: add `LRUCachedDeviceDetector` in front of your parsing code.
- **Need results to survive restarts, or want to pre-compute for a known UA list**: enable the persistent `ParseCache` and/or use the [CacheBuilder tool](CacheBuilder-Tool).
- **Multi-instance / distributed deployment**: implement `ICache` against your distributed cache of choice; the persistent `ParseCache` file is local to disk and not distributed.
