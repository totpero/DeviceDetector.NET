using System;
using NLog;

namespace DeviceDetectorNET
{
    /// <summary>
    /// Global DeviceDetector settings
    /// </summary>
    public class DeviceDetectorSettings
    {
        static DeviceDetectorSettings()
        {
            RegexesDirectory = string.Empty;
            ParseCacheDBFilename = null;
            ParseCacheDBDirectory = string.Empty;
            ParseCacheDBExpiration = TimeSpan.Zero;
            LRUCacheMaxSize = 10_000;
            LRUCacheMaxDuration = TimeSpan.FromMinutes(10);
            LRUCacheCleanPercentage = 30;
        }

        /// <summary>
        /// Default yaml regexes path
        /// Default is <see cref="string.Empty"/>
        /// Exemple: C:\YamlRegexsFiles\
        /// </summary>
        public static string RegexesDirectory { get; set; }

        /// <summary>
        /// Default Parse cache database filename. Defaults to DeviceDetectorNET.db
        /// </summary>
        public static string ParseCacheDBFilename { get; set; }

        /// <summary>
        /// Default Parse cache database directory. Defaults to string.Empty, which is the application's directory
        /// </summary>
        public static string ParseCacheDBDirectory { get; set; }

        /// <summary>
        /// To improve performance, we can cache the parsed DeviceDetector results.
        /// ParseCacheDBExpiration defaults to TimeSpan.Zero, which disables the cache
        /// One might consider setting this to a value like TimeSpan.FromDays(365) to improve performance
        /// </summary>
        public static TimeSpan ParseCacheDBExpiration { get; set; }

        /// <summary>
        /// Default maximum size for least recently used (LRU) in-memory cache database (defaults to 10,000 records)
        /// </summary>
        public static int LRUCacheMaxSize { get; set; }

        /// <summary>
        /// When the LRU cache is full, purge this percentage of the cache (defaults to 30%)
        /// </summary>
        public static int LRUCacheCleanPercentage { get; set; }

        /// <summary>
        /// Default maximum cache duration for LRU in-memory cache database (default to 10 minutes)
        /// </summary>
        public static TimeSpan LRUCacheMaxDuration { get; set; }

        public static ILogger Logger { get; set; }
    }
}
