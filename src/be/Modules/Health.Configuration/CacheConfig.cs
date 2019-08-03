namespace Health.Configuration
{
    public class CacheConfig
    {
        public int CacheExpirationInMinutes
        {
            get;
            set;
        }

        public int ShortCacheExpirationInMinutes
        {
            get;
            set;
        }

        public bool DisableCaching
        {
            get;
            set;
        }
    }
}
