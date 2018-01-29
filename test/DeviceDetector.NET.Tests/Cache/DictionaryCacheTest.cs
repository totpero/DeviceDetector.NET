using FluentAssertions;
using DeviceDetectorNET.Cache;
using Xunit;

namespace DeviceDetectorNET.Tests.Cache
{
    [Trait("Category", "DictionaryCache")]
    public class DictionaryCacheTest
    {
        [Fact]
        public void Initialize()
        {
            var cache = new DictionaryCache();
            cache.FlushAll();
        }

        [Fact]
        public void TestSetNotPresent()
        {
            var cache = new DictionaryCache();
            var result = cache.Fetch("NotExistingKey");
            result.Should().BeNull();
        }

        [Fact]
        public void TestSetAndGet()
        {
            var cache = new DictionaryCache();
            // add entry
            cache.Save("key", "value");
            cache.Fetch("key").ShouldBeEquivalentTo("value");

            // change entry
            cache.Save("key", "value2");
            cache.Fetch("key").ShouldBeEquivalentTo("value2");

            // remove entry
            cache.Delete("key");
            cache.Fetch("key").Should().BeNull();

            // flush all entries
            cache.Save("key", "value2");
            cache.Save("key3", "value2");
            cache.FlushAll();
            cache.Fetch("key").Should().BeNull();
            cache.Fetch("key3").Should().BeNull();

        }
    }
}
