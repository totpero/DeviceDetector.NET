using System;

namespace DeviceDetectorNET.Cache
{
    public static class LRUCachedDeviceDetector
    {
        private static readonly DictionaryCache deviceCache;
        private static readonly GenericLRUCache<string, DeviceDetector> lruDeviceDetector;

        static LRUCachedDeviceDetector()
        {
            deviceCache = new DictionaryCache();

            lruDeviceDetector = new GenericLRUCache<string, DeviceDetector>(maxSize: DeviceDetectorSettings.LRUCacheMaxSize,
                cleanPercentage: DeviceDetectorSettings.LRUCacheCleanPercentage,
                maxDuration: DeviceDetectorSettings.LRUCacheMaxDuration);
        }

        /// <summary>
        /// LRU cached version of GetDeviceDetector
        /// </summary>
        /// <param name="userAgent">used as the key for the cache lookup.
        /// Note that this does not handle situations where SetVersionTruncation is used because Parse will operate differently based on Version Truncation, which
        /// is a static variable.</param>
        /// <returns></returns>
        public static DeviceDetector GetDeviceDetector(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
                return null;

            if (!lruDeviceDetector.TryGetValue(userAgent, out var dd))
            {
                dd = new DeviceDetector(userAgent);
                dd.SetCache(deviceCache);
                dd.Parse();
                lruDeviceDetector.AddToCache(userAgent, dd);
            }
            return dd;
        }
    }
}
