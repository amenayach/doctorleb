
namespace Health.Doctors.Caching
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Memory;
    using Health.Configuration;
    using System.Collections;

    /// <summary>
    /// Class to manage caching using In Memory Cache.
    /// </summary>
    public class InMemoryCacheManager : ICacheManager
    {
        /// <summary>
        /// Distributed cache object.
        /// </summary>
        private readonly IMemoryCache cache;

        /// <summary>
        /// cache settings.
        /// </summary>
        private readonly CacheConfig cacheSettings;

        public InMemoryCacheManager(IMemoryCache cache, CacheConfig cacheSettings)
        {
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
            this.cacheSettings = cacheSettings ?? throw new ArgumentNullException(nameof(cacheSettings));
        }

        /// <summary>
        /// Get item from cache or add it if don't exists.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">Key of item in cache.</param>
        /// <param name="getItem">Delegeat to get the item.</param>
        /// <returns>The item to get.</returns>
        public T GetOrAddCachedObject<T>(string key, Func<T> getItem)
        {
            if (this.cacheSettings.DisableCaching)
            {
                return getItem();
            }

            var obj = this.cache.Get<T>(key);

            if (obj != null)
            {
                return obj;
            }
            else
            {
                var item = getItem();

                if (item == null)
                {
                    return default(T);
                }

                this.SetItemInCache<T>(key, item);

                return item;
            }
        }

        /// <summary>
        /// Get item from cache or add it async if don't exists.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">Key of item in cache.</param>
        /// <param name="getItem">Delegeat to get the item.</param>
        /// <returns>The item to get.</returns>
        public async Task<T> GetOrAddCachedObjectAsync<T>(string key, Func<Task<T>> getItem, int itemExpiry = -1)
        {
            if (this.cacheSettings.DisableCaching)
            {
                return await getItem();
            }

            var obj = this.cache.Get<T>(key);

            if (obj != null)
            {
                return obj;
            }
            else
            {
                var item = await getItem();

                if (item == null)
                {
                    return default(T);
                }

                await SetItemInCacheAsync<T>(key, item, itemExpiry);

                return item;
            }
        }

        /// <summary>
        /// Refresh item in cache.
        /// </summary>
        /// <param name="key">key of the item in cache.</param>
        public void Refresh(string key)
        {
            //TODO
        }

        /// <summary>
        /// Refresh item in cache async.
        /// </summary>
        /// <param name="key">Key of the item in cache.</param>
        /// <returns></returns>
        public Task RefreshAsync(string key)
        {
            //TODO
            return null;
        }

        /// <summary>
        /// Remove item from cache.
        /// </summary>
        /// <param name="key">Key of the item in cache.</param>
        public void RemoveItemsFromCache(params string[] keys)
        {
            foreach (var key in keys)
            {
                this.cache.Remove(key);
            }
        }

        /// <summary>
        /// Remove item from cache async.
        /// </summary>
        /// <param name="key">Key of the item in cache.</param>
        /// <returns></returns>
        public async Task RemoveItemsFromCacheAsync(params string[] keys)
        {
            await Task.Factory.StartNew(() =>
             {
                 foreach (var key in keys)
                 {
                     this.cache.Remove(key);
                 }
             });
        }

        /// <summary>
        /// Update an item in cache async.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">Kye of the item in cache.</param>
        /// <param name="item">Item to update in cache.</param>D:\
        public async Task SetItemInCacheAsync<T>(string key, T item, int itemExpiry = -1)
        {
            if (item != null || ((item as IEnumerable)?.GetEnumerator().MoveNext() ?? false))
            {
                await Task.Factory.StartNew(() =>
                {
                    this.cache.Set<T>(key, item,
                        new MemoryCacheEntryOptions()
                        {
                            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(itemExpiry < 0 ? this.cacheSettings.CacheExpirationInMinutes : itemExpiry)
                        });
                });
            }
        }

        /// <summary>
        /// Update an item in cache.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">Kye of the item in cache.</param>
        /// <param name="item">Item to update in cache.</param>
        public void SetItemInCache<T>(string key, T item)
        {
            if (item != null || ((item as IEnumerable)?.GetEnumerator().MoveNext() ?? false))
            {
                this.cache.Set(key, item,
                new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(this.cacheSettings.CacheExpirationInMinutes)
                });
            }
        }

        /// <summary>
        /// Get item from cache async if exists.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">Key of the item in cache.</param>
        /// <returns>Item in cache.</returns>
        public async Task<T> GetItemFromCacheIfExistsAsync<T>(string key)
        {
            return await Task.Factory.StartNew(() =>
            {
                var obj = this.cache.Get<T>(key);

                if (obj != null)
                {
                    return obj;
                }

                return default(T);
            });
        }

        /// <summary>
        /// Get item from cache if exists.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">Key of the item in cache.</param>
        /// <returns>Item in cache.</returns>
        public T GetItemFromCacheIfExists<T>(string key)
        {
            var obj = this.cache.Get<T>(key);

            if (obj != null)
            {
                return obj;
            }

            return default(T);
        }

        /// <summary>
        /// Get item from cache async if exists.
        /// </summary>
        /// <param name="key">Key of the item in cache.</param>
        /// <returns>Item in cache.</returns>
        public async Task<object> GetItem(string key)
        {
            return await Task.Factory.StartNew(() => this.cache.Get(key));
        }
    }
}
