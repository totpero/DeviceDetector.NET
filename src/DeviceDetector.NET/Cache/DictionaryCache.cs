using System.Collections.Generic;

namespace DeviceDetector.NET.Cache
{
    public class DictionaryCache : ICache
    {
        private static Dictionary<string,object> _staticCache = new Dictionary<string, object>();
        public bool Contains(string id)
        {
            return _staticCache !=null && _staticCache.Keys.Count > 0 && _staticCache.ContainsKey(id);
        }

        public bool Delete(string id)
        {
            _staticCache.Remove(id);
            return true;
        }

        public object Fetch(string id)
        {
            return Contains(id) ? _staticCache[id] : null;
        }

        public bool FlushAll()
        {
            _staticCache = new Dictionary<string, object>();
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
                _staticCache.Add(id, data);
            }
            return true;
        }
    }
}