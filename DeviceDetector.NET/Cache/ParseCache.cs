using LiteDB;
using Newtonsoft.Json;
using System;
using System.IO;
using Microsoft.Extensions.Logging;
namespace DeviceDetectorNET.Cache
{
    internal class ParseCache
    {
        private ParseCache()
        {
        }

        private static readonly Lazy<ParseCache> LazyCache = new Lazy<ParseCache>(InitializeCache);
        private LiteDatabase _cacheDatabase;

        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = null,
            TypeNameHandling = TypeNameHandling.Auto
        };

        private static ParseCache InitializeCache()
        {
            var cache = new ParseCache();
            var dir = DeviceDetectorSettings.ParseCacheDBDirectory ?? string.Empty;

            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                try
                {
                    Directory.CreateDirectory(dir);
                }
                catch (Exception exception)
                {
                    Logger?.LogWarning("Unable to create directory {0} due to {1}", dir, exception);
                    // for now, swallow this error so we do not accidentally impact an unknown use case
                    ;
                    //throw new DirectoryNotFoundException($"Directory {dir} was not found and could not create it");
                }
            }

            var filename = DeviceDetectorSettings.ParseCacheDBFilename ?? "DeviceDetectorNET.db";
            var path = Path.Combine(dir, filename);
            var connectionString = new ConnectionString($"filename={path}");

            cache._cacheDatabase = new LiteDatabase(connectionString);
            cache.ParsedDataCollection = cache._cacheDatabase.GetCollection<CachedDataHolder>();
            cache.EmptyExpired();

            return cache;
        }

        public static ILogger Logger { get; set; } = DeviceDetectorSettings.Logger;
        public static ParseCache Instance => LazyCache.Value;

        private LiteCollection<CachedDataHolder> ParsedDataCollection { get; set; }

        public DeviceDetectorCachedData FindById(string key)
        {
            var cachedData = ParsedDataCollection.FindById(key);
            if (IsExpired(cachedData))
            {
                ParsedDataCollection.Delete(key);
                return null;
            }

            if (string.IsNullOrEmpty(cachedData?.Json)) return null;

            var data = JsonConvert.DeserializeObject<DeviceDetectorCachedData>(cachedData.Json, JsonSettings);
            return data;
        }

        public void Upsert(string key, DeviceDetectorCachedData data)
        {
            var cachedData = new CachedDataHolder()
            {
                Id = key,
                Json = JsonConvert.SerializeObject(data, JsonSettings),
                ExpirationDate = DateTime.UtcNow.Add(DeviceDetectorSettings.ParseCacheDBExpiration)
            };

            ParsedDataCollection.Upsert(cachedData);
        }

        private void EmptyExpired()
        {
            ParsedDataCollection.Delete(b => b.ExpirationDate < DateTime.UtcNow);
        }

        private static bool IsExpired(CachedDataHolder data)
        {
            if (data == null)
                return false;
            return DateTime.UtcNow > data.ExpirationDate.ToUniversalTime();
        }

    }
}
