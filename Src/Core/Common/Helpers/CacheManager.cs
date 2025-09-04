using System.Runtime.Caching;

namespace Barin.Framework.Common.Helpers;

public static class CacheManager
{
    static readonly ObjectCache cache = MemoryCache.Default;
    static readonly object padlock = new object();

    public static void ResetCache(string keyName)
    {
        lock (padlock)
        {
            cache.Remove(keyName);
        }
    }

    public static object GetItem(string keyName, Func<object> action, DateTimeOffset offset)
    {
        lock (padlock)
        {
            var content = cache[keyName];

            if (content == null)
            {
                content = action();
                cache.Add(keyName, content, offset == default ? DateTimeOffset.MaxValue : offset);
            }

            return content;
        }
    }

    public static object GetItem(string keyName, Func<object> action)
    {
        return GetItem(keyName, action, default);
    }
}
