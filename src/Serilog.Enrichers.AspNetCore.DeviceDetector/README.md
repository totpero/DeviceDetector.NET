Serilog.Enrichers.AspNetCore.DeviceDetector
============================================

A [Serilog](https://serilog.net/) enricher that adds device, client, operating system, brand and
model information parsed from the current ASP.NET Core request's `User-Agent` header, using
[DeviceDetector.NET](https://github.com/totpero/DeviceDetector.NET).

## Install

```
dotnet add package Serilog.Enrichers.AspNetCore.DeviceDetector
```

## Usage

Register `IHttpContextAccessor` and wire up the enricher when configuring Serilog through `UseSerilog`,
so it can resolve the accessor from the application's service provider:

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

Every log event written while handling an HTTP request will then include a `DeviceDetector` property
with the parsed device, client, OS, brand and model information. The result is computed once per
request and cached on `HttpContext.Items`, so subsequent log events in the same request reuse it
instead of re-parsing the User-Agent header.

If there is no active `HttpContext` (e.g. during startup, or in a background service), the enricher
is a no-op.
