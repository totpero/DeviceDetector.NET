using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Core;
using Serilog.Enrichers;
using Serilog.Events;
using Shouldly;
using Xunit;

namespace Serilog.Enrichers.AspNetCore.DeviceDetector.Tests;

public class DeviceDetectorEnricherTests
{
    private const string ChromeUserAgent =
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/128.0.0.0 Safari/537.36";

    private static DefaultHttpContext CreateHttpContext(string userAgent)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["User-Agent"] = userAgent;
        return httpContext;
    }

    private static ILogger CreateLogger(IHttpContextAccessor accessor, List<LogEvent> events) =>
        new LoggerConfiguration()
            .Enrich.With(new DeviceDetectorEnricher(accessor))
            .WriteTo.Sink(new DelegatingSink(events.Add))
            .CreateLogger();

    [Fact]
    public void Enrich_AddsDeviceDetectorProperty_WhenUserAgentPresent()
    {
        var accessor = new HttpContextAccessor { HttpContext = CreateHttpContext(ChromeUserAgent) };
        var events = new List<LogEvent>();

        CreateLogger(accessor, events).Information("test");

        events.Count.ShouldBe(1);
        events[0].Properties.ContainsKey("DeviceDetector").ShouldBeTrue();
    }

    [Fact]
    public void Enrich_ReusesCachedProperty_ForSubsequentEventsInSameRequest()
    {
        var accessor = new HttpContextAccessor { HttpContext = CreateHttpContext(ChromeUserAgent) };
        var events = new List<LogEvent>();
        var logger = CreateLogger(accessor, events);

        logger.Information("first");
        logger.Information("second");

        events.Count.ShouldBe(2);
        events[0].Properties["DeviceDetector"].ShouldBeSameAs(events[1].Properties["DeviceDetector"]);
    }

    [Fact]
    public void Enrich_DoesNothing_WhenHttpContextIsNull()
    {
        var accessor = new HttpContextAccessor { HttpContext = null };
        var events = new List<LogEvent>();

        CreateLogger(accessor, events).Information("test");

        events.Count.ShouldBe(1);
        events[0].Properties.ContainsKey("DeviceDetector").ShouldBeFalse();
    }

    private sealed class DelegatingSink(Action<LogEvent> write) : ILogEventSink
    {
        public void Emit(LogEvent logEvent) => write(logEvent);
    }
}
