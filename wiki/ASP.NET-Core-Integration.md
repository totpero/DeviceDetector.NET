# ASP.NET Core Integration

This page covers the minimal demo API bundled with the repository and a recommended dependency-injection pattern for using DeviceDetector.NET directly in application logic. For logging integration, see the dedicated [Serilog.Enrichers.AspNetCore.DeviceDetector](Serilog.Enrichers.AspNetCore.DeviceDetector) page.

## Demo API — `DeviceDetector.NET.Web`

[`HomeController`](https://github.com/totpero/DeviceDetector.NET/blob/master/DeviceDetector.NET.Web/Controllers/HomeController.cs) exposes a single `GET /Home` endpoint that parses the *caller's* User-Agent and Client Hints and returns the full result as JSON — handy as a quick manual test/demo of what the library reports for your current browser:

```csharp
[HttpGet]
public object Get()
{
    DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);

    var userAgent = Request.Headers.UserAgent;
    var headers = Request.Headers.ToDictionary(a => a.Key, a => a.Value.ToArray().FirstOrDefault());
    var clientHints = ClientHints.Factory(headers);

    var result = DeviceDetector.GetInfoFromUserAgent(userAgent, clientHints);
    return result;
}
```

Run it locally to see the shape of `ParseResult<DeviceDetectorResult>` returned by `GetInfoFromUserAgent` for any real request hitting your dev machine.

## Dependency injection pattern

[Issue #83](https://github.com/totpero/DeviceDetector.NET/issues/83) raised a fair complaint: there's no built-in `IServiceCollection` extension, and the library isn't itself injectable — `DeviceDetector` is a per-parse object you construct with a User-Agent, not a long-lived service. If what you want is `@inject` a service and call `IsMobile(Request)` from a Razor page or controller with no further ceremony, wrap it yourself with a small service like this:

```csharp
public interface IDeviceDetectionService
{
    bool IsMobile(HttpRequest request);
    DeviceDetector Detect(HttpRequest request);
}

public class DeviceDetectionService : IDeviceDetectionService
{
    public DeviceDetector Detect(HttpRequest request)
    {
        var headers = request.Headers.ToDictionary(a => a.Key, a => a.Value.ToArray().FirstOrDefault());
        var clientHints = ClientHints.Factory(headers);

        var dd = new DeviceDetector(request.Headers.UserAgent, clientHints);
        dd.Parse();
        return dd;
    }

    public bool IsMobile(HttpRequest request) => Detect(request).IsMobile();
}
```

Register it once, as a singleton — the service itself holds no per-request state, so one instance can safely serve every request:

```csharp
builder.Services.AddSingleton<IDeviceDetectionService, DeviceDetectionService>();
```

Then, in a Razor page or controller:

```razor
@inject IDeviceDetectionService DeviceDetection

@if (DeviceDetection.IsMobile(Context.Request))
{
    <p>Mobile layout</p>
}
```

### Do I need to worry about caching here?

Not by default. `new DeviceDetector(...)` followed by `Parse()` already benefits from the process-wide fragment cache described in [Caching](Caching) with zero extra setup — `SetCache(new DictionaryCache())` is what `DeviceDetector` uses internally already; calling it explicitly only matters if you want to swap in a different `ICache` implementation. Creating a fresh `DeviceDetector` instance per request is intentional and cheap: instances are lightweight, and the *data* they end up reusing (regex fragment results) already lives in a shared static cache, not in the instance itself.

If profiling shows parsing itself (not instance allocation) is a bottleneck under load, and you don't need Client Hints for the check in question, swap `Detect`'s body for [`LRUCachedDeviceDetector.GetDeviceDetector(userAgent)`](Caching#3-in-memory-lru-cache-of-whole-devicedetector-instances) instead of constructing a new instance — see the [Caching](Caching#setcache-vs-lrucacheddevicedetector) page for why that trade-off (no Client Hints support) exists and when it's worth it.

## Choosing where to detect

- Use the [Serilog enricher](Serilog.Enrichers.AspNetCore.DeviceDetector) for anything you want automatically attached to *all* logs for a request (errors, warnings, audit trails).
- Use `DeviceDetector.GetInfoFromUserAgent` directly, or the DI wrapper above, when you need the parsed result **in application logic**, not just in logs (e.g. serving a different response for mobile vs. desktop).
