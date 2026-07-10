using BitFaster.Caching.Lru;

namespace DeviceDetectorNET.Cache.BitFaster
{
    /// <summary>
    /// ICache backed by BitFaster.Caching's ConcurrentLru. Unlike DictionaryCache, entries are
    /// bounded and least-recently-used ones are evicted once <paramref name="capacity"/> is reached,
    /// which caps memory use under high User-Agent cardinality.
    /// </summary>
    public class BitFasterConcurrentLruCache : ICache
    {
        private readonly ConcurrentLru<string, object> _cache;

        public BitFasterConcurrentLruCache(int capacity = 10_000)
        {
            _cache = new ConcurrentLru<string, object>(capacity);
        }

        public object Fetch(string id)
        {
            _cache.TryGet(id, out var value);
            return value;
        }

        public bool Contains(string id)
        {
            return _cache.TryGet(id, out _);
        }

        public bool Save(string id, object data, int lifeTime = 0)
        {
            _cache.AddOrUpdate(id, data);
            return true;
        }

        public bool Delete(string id)
        {
            return _cache.TryRemove(id);
        }

        public bool FlushAll()
        {
            _cache.Clear();
            return true;
        }
    }
}
