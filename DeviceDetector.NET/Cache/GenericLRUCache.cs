using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace DeviceDetectorNET.Cache
{
    /// <summary>
    /// See https://gist.github.com/eladmarg/8d4a7f7a43a36c35d488e99475a101d9
    /// </summary>
    /// <typeparam name="TV"></typeparam>
    /// <typeparam name="TU"></typeparam>
    public class GenericLRUCache<TV, TU> where TU : class
        {
            private readonly int CacheMaxSize;
            private readonly int CleanSize;
            private readonly TimeSpan MaxDuration;

            private readonly ConcurrentDictionary<TV, CacheDataObject<TU>> _cache = new ConcurrentDictionary<TV, CacheDataObject<TU>>();

            public GenericLRUCache(int maxSize = 50000, int cleanPercentage = 30, TimeSpan maxDuration = default(TimeSpan))
            {
                CacheMaxSize = maxSize;
                CleanSize = (int)Math.Max(CacheMaxSize * (1.0 * cleanPercentage / 100), 1000);
                if (maxDuration == default(TimeSpan))
                {
                    MaxDuration = TimeSpan.FromDays(1);
                }
                else
                {
                    MaxDuration = maxDuration;
                }
            }

            private static readonly object _lockObject = new object();
            private static bool IsCleaning = false;

            public bool AddToCache(TV cacheKey, TU value)
            {
                var cachedResult = new CacheDataObject<TU>
                {
                    Usage = 1,
                    Value = value,
                    Timestamp = DateTime.UtcNow
                };

                _cache.AddOrUpdate(cacheKey, cachedResult, (_, __) => cachedResult);
                if (_cache.Count > 0 && _cache.Count > CacheMaxSize && !IsCleaning)
                {

                    lock (_lockObject)
                    {
                        if (IsCleaning)
                            return true;

                        try
                        {
                            IsCleaning = true;
                            var cacheArray = _cache.ToArray();
                            if (cacheArray.Length > 0)
                            {
                                var itemsToSkip = CacheMaxSize - CleanSize;
                                if (itemsToSkip > 10)
                                {
                                    var items = cacheArray.OrderByDescending(x => x.Value.Usage)
                                          .ThenBy(x => x.Value.Timestamp)
                                          .Skip(itemsToSkip);

                                    if (items.Any())
                                    {
                                        foreach (var source in items)
                                        {
                                            if (source.Key == null || cacheKey == null)
                                                continue;

                                            if (EqualityComparer<TV>.Default.Equals(source.Key, cacheKey))
                                                continue; // we don't want to remove the one we just added

                                            CacheDataObject<TU> ignored;
                                            _cache.TryRemove(source.Key, out ignored);
                                        }
                                    }
                                }


                            }

                        }
                        finally
                        {
                            IsCleaning = false;
                        }
                    }

                }
                return true;
            }

            public TU GetFromCache(TV cacheKey, bool increment = true)
            {
                CacheDataObject<TU> value;
                if (_cache.TryGetValue(cacheKey, out value) && (MaxDuration == TimeSpan.MaxValue || (DateTime.UtcNow - value.Timestamp) <= MaxDuration))
                {
                    if (increment)
                    {
                        Interlocked.Increment(ref value.Usage);
                    }
                    return value.Value;
                }
                return null;
            }

            public TU GetOrAdd(TV cacheKey, Func<TU> aquire, bool increment = true)
            {
                CacheDataObject<TU> value;
                if (_cache.TryGetValue(cacheKey, out value) && (MaxDuration == TimeSpan.MaxValue || (DateTime.UtcNow - value.Timestamp) <= MaxDuration))
                {
                    if (increment)
                    {
                        Interlocked.Increment(ref value.Usage);
                    }
                    return value.Value;
                }
                TU val = aquire();
                if (val != default(TU))
                {
                    AddToCache(cacheKey, val);
                }

                return val;
            }

            public bool TryGetValue(TV cacheKey, out TU val, bool increment = true)
            {
                CacheDataObject<TU> value;
                if (_cache.TryGetValue(cacheKey, out value) && (MaxDuration == TimeSpan.MaxValue || (DateTime.UtcNow - value.Timestamp) <= MaxDuration))
                {
                    if (increment)
                    {
                        Interlocked.Increment(ref value.Usage);
                    }
                    val = value.Value;
                    return true;
                }
                val = default(TU);
                return false;
            }

            public void Remove(TV cacheKey)
            {
                CacheDataObject<TU> value;
                _cache.TryRemove(cacheKey, out value);
            }

            public bool IsExistInCache(TV cacheKey)
            {
                return (_cache.ContainsKey(cacheKey));
            }


            private class CacheDataObject<T> where T : class
            {
                public DateTime Timestamp;
                public int Usage;
                public T Value;
            }
        }
    }
