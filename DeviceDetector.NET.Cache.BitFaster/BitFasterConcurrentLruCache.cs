using BitFaster.Caching.Lru;

namespace DeviceDetectorNET.Cache.BitFaster
{
    public class BitFasterConcurrentLruCache : ICache
    {
        private static ConcurrentLru<string, object> _concurrentLru = new ConcurrentLru<string, object>(int.MaxValue);

        public object Fetch(string id)
        {
            _concurrentLru.TryGet(id, out var value);
            return value;
        }

        public bool Contains(string id)
        {
            return _concurrentLru != null && _concurrentLru.Keys.Count > 0 && _concurrentLru.Keys.Contains(id);
        }

        public bool Save(string id, object data, int lifeTime = 0)
        {
            _concurrentLru.AddOrUpdate(id, data);
            return true;
        }

        public bool Delete(string id)
        {
            return _concurrentLru.TryRemove(id);
        }

        public bool FlushAll()
        {
            _concurrentLru = new ConcurrentLru<string, object>(int.MaxValue);
            return true;
        }
    }
}