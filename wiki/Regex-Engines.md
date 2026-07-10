# Regex Engines

The upstream [`device-detector`](https://github.com/matomo-org/device-detector) rule set is written as **PCRE**-flavored regular expressions (Perl-Compatible Regular Expressions), because the source project is PHP. .NET's built-in `System.Text.RegularExpressions` engine is close to PCRE but not identical — a small subset of upstream patterns rely on PCRE-only syntax or matching semantics.

DeviceDetector.NET abstracts the regex engine behind `DeviceDetectorNET.RegexEngine.IRegexEngine`:

```csharp
public interface IRegexEngine
{
    bool Match(string input, string pattern);
    IEnumerable<string> Matches(string input, string pattern);
    IEnumerable<string> MatchesUniq(string input, string pattern);
    string Replace(string input, string pattern, string replacement);
}
```

Two implementations ship with the project:

## `MSRegexEngine` / `MSRegexCompiledEngine` (default)

Located in `DeviceDetectorNET.RegexEngine`, backed by `System.Text.RegularExpressions.Regex`. No extra dependency, works everywhere the core package works (including `netstandard2.0`/`net462`). `MSRegexCompiledEngine` (the default, used automatically when no engine is set) uses `RegexOptions.Compiled` for faster repeated matching at the cost of slower first-use JIT compilation; `MSRegexEngine` skips `Compiled` — prefer it for short-lived processes (CLIs, Lambdas/functions) where paying the compilation cost isn't worth it.

## `PcreRegexEngine` (optional, better upstream fidelity)

Provided by the separate [`DeviceDetector.NET.RegexEngine.PCRE`](https://www.nuget.org/packages/DeviceDetector.NET.RegexEngine.PCRE) package, backed by [PCRE.NET](https://github.com/AArnott/PCRE.NET) — a real PCRE engine. Use it when you hit edge cases where the .NET regex engine disagrees with the PHP project's results for the same UA (rare, but possible for patterns using PCRE-specific constructs).

```
dotnet add package DeviceDetector.NET.RegexEngine.PCRE
```

```csharp
using DeviceDetectorNET.RegexEngine.PCRE;

var dd = new DeviceDetector(userAgent, clientHints);
dd.SetRegexEngine(new PcreRegexEngine());
dd.Parse();
```

`DeviceDetector.SetRegexEngine(IRegexEngine)` (and the equivalent method on individual parsers, `AbstractParser.SetRegexEngine`) swaps the engine used for all subsequent matching; `GetRegexEngine()` lazily defaults to `MSRegexCompiledEngine` if none was set.

This package targets `net10.0` only (it depends on native PCRE bindings), so it's not available to classic .NET Framework or older TFMs — those consumers stay on the default `System.Text.RegularExpressions`-based engine.

## When does this matter?

For the vast majority of User-Agents both engines agree. Reach for the PCRE engine only if:

- Automated tests comparing against the upstream PHP fixtures show a mismatch for a specific pattern, or
- You've confirmed (e.g. via the upstream issue tracker) that a rule uses PCRE-only syntax such as certain lookaround/backreference constructs not supported by `System.Text.RegularExpressions`.

If you're unsure, start with the default engine — it's simpler, has no extra dependency, and covers virtually all real-world traffic correctly.
