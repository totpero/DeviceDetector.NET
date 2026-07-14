using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DeviceDetector.Net.CacheBuilder
{
    public class CacheBuilder(IEnumerable<string> userAgentStrings, bool skipBotDetection, ILogger logger)
    {
        public IEnumerable<string> UserAgentStrings { get; } = userAgentStrings;
        public bool SkipBotDetection { get; } = skipBotDetection;
        public ILogger Logger { get; } = logger;

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