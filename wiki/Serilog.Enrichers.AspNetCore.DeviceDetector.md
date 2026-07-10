# Serilog.Enrichers.AspNetCore.DeviceDetector

A [Serilog](https://serilog.net/) enricher that attaches device, client, operating system, brand and model information — parsed from the current ASP.NET Core request's `User-Agent` header via [DeviceDetector.NET](Home) — to every log event written while handling that request.

## Install

```
dotnet add package Serilog.Enrichers.AspNetCore.DeviceDetector
```

Targets `net8.0`, `net9.0` and `net10.0`. Depends on `Serilog`, `Serilog.AspNetCore`, and (via project reference) `DeviceDetector.NET` itself.

## Setup

Register `IHttpContextAccessor`, then wire the enricher into Serilog through `UseSerilog`, passing the app's `IServiceProvider` so it can resolve the accessor:

```csharp
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.WithDeviceDetector(services)
    .WriteTo.Console());

var app = builder.Build();
```

`AddHttpContextAccessor()` is required. `WithDeviceDetector(IServiceProvider)` calls `serviceProvider.GetRequiredService<IHttpContextAccessor>()` internally — if you forget to register it, you'll get a clear DI resolution exception at startup rather than a silent no-op.

## What you get

Every log event emitted while an `HttpContext` is active gets a structured `DeviceDetector` property containing the parsed device/client/OS/brand/model result (the same shape as `DeviceDetector.GetInfoFromUserAgent` — see [Getting Started](Getting-Started#one-shot-helper)). Because the property is destructured, structured sinks (Seq, Elasticsearch, JSON file sinks, etc.) expose it as queryable sub-fields rather than one opaque string.

```json
{
  "DeviceDetector": {
    "UserAgent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) ...",
    "DeviceType": "desktop",
    "OsFamily": "Windows",
    "BrowserFamily": "Chrome",
    "Client": { "Name": "Chrome", "Version": "128.0.0.0", "...": "..." },
    "Os": { "Name": "Windows", "Version": "10", "...": "..." }
  }
}
```

## Per-request caching

The User-Agent is parsed **at most once per request**, not once per log call: the first `Enrich` call for a request runs `DeviceDetector.GetInfoFromUserAgent` and stores the resulting Serilog property on `HttpContext.Items`; every subsequent log event in the same request reuses that stored property instead of re-parsing. This is covered by a dedicated test (`Enrich_ReusesCachedProperty_ForSubsequentEventsInSameRequest`) in [`Serilog.Enrichers.AspNetCore.DeviceDetector.Tests`](https://github.com/totpero/DeviceDetector.NET/tree/master/Serilog.Enrichers.AspNetCore.DeviceDetector.Tests).

This cache is scoped to `HttpContext.Items`, so it's automatically request-scoped and requires no manual setup or cleanup — unlike the [general-purpose caching layers](Caching) DeviceDetector.NET otherwise offers, you don't need to configure anything here to avoid redundant parsing within a request.

`VersionTruncation.VERSION_TRUNCATION_NONE` is forced for this parse, regardless of what `DeviceDetector.SetVersionTruncation` is set to elsewhere in your app — logs always get full version numbers.

## No-op cases

The enricher silently does nothing (adds no property) when:
- There is no active `HttpContext` — e.g. during startup, in a hosted/background service, or in code running outside a request.
- The request has no `User-Agent` header.

This makes it safe to leave configured globally; it only ever adds information, never throws for missing context.

## Relationship to the demo API and DI wrapper

This enricher solves *logging*, specifically. If you need parsed device info **in application logic** (not just attached to logs), see [ASP.NET Core Integration](ASP.NET-Core-Integration) for the demo `HomeController` pattern and the [dependency-injection wrapper](ASP.NET-Core-Integration#dependency-injection-pattern) written in response to [issue #83](https://github.com/totpero/DeviceDetector.NET/issues/83) — the two are independent and commonly used together (one enriches logs, the other drives behavior).
