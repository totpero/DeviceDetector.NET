using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DeviceDetector.Net.CacheBuilder
{
    public class CacheBuilder
    {
        public IEnumerable<string> UserAgentStrings { get; }
        public bool SkipBotDetection { get; }
        public ILogger Logger { get; }

        public CacheBuilder(IEnumerable<string> userAgentStrings, bool skipBotDetection, ILogger logger)
        {
            UserAgentStrings = userAgentStrings;
            SkipBotDetection = skipBotDetection;
            Logger = logger;
        }

        public void BuildCache()
        {
            var userAgents = UserAgentStrings.ToList();
            Parallel.ForEach(userAgents, ParseUserAgentString);
        }

        private void ParseUserAgentString(string userAgent)
        {
            Logger?.LogInformation("Processing: {0}", userAgent);
            var detector = new DeviceDetectorNET.DeviceDetector(userAgent);
            detector.SkipBotDetection(SkipBotDetection);
            detector.Parse();
        }
    }
}