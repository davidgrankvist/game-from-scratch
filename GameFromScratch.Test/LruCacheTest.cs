using GameFromScratch.App.Platform.Common.Text;

namespace GameFromScratch.Test
{
    [TestClass]
    public class LruCacheTest
    {

        private static LruCache<int, int> CreateCache(int capacity)
        {
            return new LruCache<int, int>(capacity);
        }

        [TestMethod]
        public void TestDoesNotExceedCapacity()
        {
            var capacity = 3;
            var cache = CreateCache(capacity);

            for (var i = 0; i < 2 * capacity; i++)
            {
                cache.Add(i, i);
            }

            Assert.AreEqual(capacity, cache.Count);
        }

        [TestMethod]
        public void TestContainsAddedValues()
        {
            var capacity = 3;
            var cache = CreateCache(capacity);

            for (var i = 0; i < capacity; i++)
            {
                cache.Add(i, i);
            }

            for (var i = 0; i < capacity; i++)
            {
                var exists = cache.TryGetValue(i, out var value);
                Assert.IsTrue(exists);
                Assert.AreEqual(i, value);
            }
        }

        [TestMethod]
        public void TestLeastRecentlyUsedValuesAreReplaced()
        {
            var capacity = 10; // needs to be even for checks below
            var cache = CreateCache(capacity);

            // fill the cache
            for (var i = 0; i < capacity; i++)
            {
                cache.Add(i, i);
            }

            // overflow with half of capacity
            for (var i = capacity; i < capacity + capacity / 2; i++)
            {
                cache.Add(i, i);
            }

            // the first half of the initial elements should be removed
            for (var i = 0; i < capacity / 2; i++)
            {
                var exists = cache.TryGetValue(i, out var _);
                Assert.IsFalse(exists);
            }

            // the second half of the initial elements plus the new elements should remain
            for (var i = capacity / 2; i < capacity + capacity / 2; i++)
            {
                var exists = cache.TryGetValue(i, out var value);
                Assert.IsTrue(exists);
                Assert.AreEqual(i, value);
            }
        }
    }
}