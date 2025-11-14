using System.Collections.Generic;
using DeviceDetectorNET;
using DeviceDetectorNET.Parser;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;
using System.Linq;

namespace Serilog.Enrichers;

public class DeviceDetectorEnricher(IHttpContextAccessor httpContextAccessor) : ILogEventEnricher
{
    private const string DeviceDetectorPropertyName = "DeviceDetector";
    private const string DeviceDetectorItemKey = "Serilog_DeviceDetector";


    /// <summary>
    ///     Initializes a new instance of the <see cref="DeviceDetectorEnricher" /> class.
    /// </summary>
    public DeviceDetectorEnricher() : this(new HttpContextAccessor())
    {
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        HttpContext httpContext = httpContextAccessor!.HttpContext;
        if (httpContext == null) return;

        //if (httpContext.Items.TryGetValue(DeviceDetectorItemKey, out object value) &&
        //    value is LogEventProperty logEventProperty)
        //{

        //}
        //else
        //{

        //}

        if (httpContext.Request!.Headers!.TryGetValue("User-Agent", out var userAgent))
        {
            DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);
            var headers = httpContext.Request!.Headers.ToDictionary(a => a.Key, a => a.Value.ToArray().FirstOrDefault());
            var clientHints = ClientHints.Factory(headers);  // client hints are optional
            var result = DeviceDetector.GetInfoFromUserAgent(userAgent, clientHints);

            var deviceDetectorProperty = propertyFactory.CreateProperty(DeviceDetectorPropertyName, result, destructureObjects: true);
            httpContext.Items.TryAdd(DeviceDetectorItemKey, deviceDetectorProperty);
            logEvent.AddPropertyIfAbsent(deviceDetectorProperty);
        }
    }
}
