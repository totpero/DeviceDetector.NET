using Shouldly;
using DeviceDetectorNET.Cache.BitFaster;
using Xunit;

namespace DeviceDetectorNET.Tests.Cache
{
    [Trait("Category", "BitFasterConcurrentLruCache")]
    public class BitFasterConcurrentLruCacheTest
    {
        [Fact]
        public void Initialize()
        {
            var cache = new BitFasterConcurrentLruCache();
            cache.FlushAll();
        }

        [Fact]
        public void TestSetNotPresent()
        {
            var cache = new BitFasterConcurrentLruCache();
            var result = cache.Fetch("NotExistingKey");
            result.ShouldBeNull();
        }

        [Fact]
        public void TestSetAndGet()
        {
            var cache = new BitFasterConcurrentLruCache();
            // add entry
            cache.Save("key", "value");
            cache.Fetch("key").ShouldBe("value");
            cache.Contains("key").ShouldBeTrue();

            // change entry
            cache.Save("key", "value2");
            cache.Fetch("key").ShouldBe("value2");

            // remove entry
            cache.Delete("key");
            cache.Fetch("key").ShouldBeNull();
            cache.Contains("key").ShouldBeFalse();

            // flush all entries
            cache.Save("key", "value2");
            cache.Save("key3", "value2");
            cache.FlushAll();
            cache.Fetch("key").ShouldBeNull();
            cache.Fetch("key3").ShouldBeNull();
        }

        [Fact]
        public void TestEvictsOnceCapacityIsExceeded()
        {
            // ConcurrentLru approximates LRU via internal hot/warm/cold queues rather than
            // strict recency order, so assert only that bounding actually happens, not which
            // specific key survives.
            var cache = new BitFasterConcurrentLruCache(capacity: 3);

            for (var i = 0; i < 20; i++)
            {
                cache.Save($"key{i}", $"value{i}");
            }

            var survivors = 0;
            for (var i = 0; i < 20; i++)
            {
                if (cache.Contains($"key{i}"))
                {
                    survivors++;
                }
            }

            survivors.ShouldBeLessThan(20);
        }
    }
}
