using DeviceDetectorNET.Cache;
using DeviceDetectorNET.Parser;
using Shouldly;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DeviceDetectorNET.Tests;

[Trait("Category", "Concurrency")]
public class ConcurrencyIssue29Test
{
    private static readonly string[] UserAgents =
    {
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.0.0 Safari/537.36",
        "Mozilla/5.0 (Linux; Android 10; SM-G960F) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.120 Mobile Safari/537.36",
        "Mozilla/5.0 (iPhone; CPU iPhone OS 14_6 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0 Mobile/15E148 Safari/604.1",
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.114 Safari/537.36",
        "VLC/3.0.11.1 LibVLC/3.0.11.1",
        "Winamp/5.63",
        "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)",
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.0.0 Safari/537.36 Edg/103.0.1264.62",
        "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.114 Safari/537.36",
        "Spotify/8.6.98"
    };

    // Reproduces the scenario from https://github.com/totpero/DeviceDetector.NET/issues/29 :
    // many threads constructing DeviceDetector instances concurrently (e.g. one per web request),
    // which each build fresh parser instances that populate/read the shared static regex cache
    // (DeviceDetectorNET.Cache.DictionaryCache) at the same time.
    [Fact]
    public void ParseUserAgentsConcurrently_DoesNotCorruptSharedCache()
    {
        DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);

        var exceptions = new ConcurrentBag<Exception>();

        Parallel.For(0, 2000, i =>
        {
            try
            {
                var ua = UserAgents[i % UserAgents.Length];
                var dd = new DeviceDetector(ua);
                dd.Parse();
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        });

        exceptions.ShouldBeEmpty(string.Join(Environment.NewLine, exceptions.Select(e => e.ToString())));
    }

    // Directly hammers the cache implementation named in the issue's stack trace
    // (DictionaryCache.Contains/Fetch/Save) from many threads at once, including
    // repeated FlushAll() calls to maximize the chance of a torn read on the
    // backing collection if it were ever a plain (non-concurrent) Dictionary again.
    [Fact]
    public void DictionaryCache_ConcurrentReadWriteFlush_DoesNotThrow()
    {
        var cache = new DictionaryCache();
        var exceptions = new ConcurrentBag<Exception>();
        var keys = Enumerable.Range(0, 20).Select(i => $"key-{i}").ToArray();

        Parallel.For(0, 5000, i =>
        {
            try
            {
                var key = keys[i % keys.Length];
                switch (i % 4)
                {
                    case 0:
                        cache.Save(key, i);
                        break;
                    case 1:
                        cache.Fetch(key);
                        break;
                    case 2:
                        cache.Contains(key);
                        break;
                    case 3:
                        if (i % 200 == 0) cache.FlushAll();
                        else cache.Delete(key);
                        break;
                }
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        });

        exceptions.ShouldBeEmpty(string.Join(Environment.NewLine, exceptions.Select(e => e.ToString())));
    }
}
