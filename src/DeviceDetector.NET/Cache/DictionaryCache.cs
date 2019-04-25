using System.Collections.Concurrent;

namespace DeviceDetectorNET.Cache
{
    public class DictionaryCache : ICache
    {
        private static ConcurrentDictionary<string,object> _staticCache = new ConcurrentDictionary<string, object>();
        public bool Contains(string id)
        {
            return _staticCache !=null && _staticCache.Keys.Count > 0 && _staticCache.ContainsKey(id);
        }

        public bool Delete(string id)
        {
            if (Contains(id))
            {
                _staticCache.TryRemove(id, out _);
                return true;
            }
            return false;
        }

        public object Fetch(string id)
        {
            return Contains(id) ? _staticCache[id] : null;
        }

        public bool FlushAll()
        {
            _staticCache = new ConcurrentDictionary<string, object>();
            return true;
        }

        public bool Save(string id, object data, int lifeTime = 0)
        {
            if (Contains(id))
            {
                _staticCache[id] = data;
            }
            else
            {
                _staticCache.TryAdd(id, data);
            }
            return true;
        }
    }
}