# Bot Detection

Bots (crawlers, spiders, monitoring/uptime tools, etc.) are detected using their own YAML rule set (`regexes/bots.yml`), separate from device/client/OS rules.

## Via the full `DeviceDetector` pipeline

```csharp
dd.Parse();

if (dd.IsBot())
{
    var botInfo = dd.GetBot(); // ParseResult<BotMatchResult>
    // botInfo.Match.Name, .Category, .Url, .Producer, ...
}
```

Two options reduce the cost of bot handling when you don't need full detail:

```csharp
// GetBot() will only return non-null if a bot was actually detected — otherwise it just
// speeds up the check a little, real detail is still parsed when a bot IS found
dd.DiscardBotInformation();

// Skip bot detection entirely — every UA (including real bots) is parsed as a regular device/client
dd.SkipBotDetection();
```

Both must be set **before** calling `Parse()`.

## Bot-only parsing (cheapest option)

If bot/not-bot is the *only* question you need answered — e.g. to short-circuit request handling before any device/client parsing — use `BotParser` directly, skipping the rest of the pipeline entirely:

```csharp
using DeviceDetectorNET.Parser;

var botParser = new BotParser();
botParser.SetUserAgent(userAgent);

// OPTIONAL: Parse() then returns a non-null "is a bot" marker instead of full bot details
botParser.DiscardDetails = true;

var result = botParser.Parse();

if (result != null)
{
    // it's a bot — skip further processing
    return;
}

// handle non-bot requests as usual
```

This mirrors the upstream PHP pattern (`new BotParser(); $botParser->setUserAgent($ua); $botParser->discardDetails(); $botParser->parse();`) exactly, and is the recommended approach for high-traffic endpoints (analytics ingestion, rate limiting, etc.) that mainly care about excluding bot traffic cheaply.

## Notes

- Bot rules are independent of device/OS/client rules, so a UA can be `IsBot() == true` while never being classified as a smartphone/browser/etc — bot detection short-circuits the rest of the pipeline by design when a match is found and `SkipBotDetection()` wasn't called.
- See [Updating Device Detector Data](Updating-Device-Detector-Data) if a known bot isn't being recognized — the fix is almost always an upstream YAML rule update rather than a .NET code change.
