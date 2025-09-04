namespace Barin.Framework.Common.Helpers;

public static class DictionaryExtensions
{
    public static TValue GetSafeValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
    {
        TValue result = default(TValue);
        dictionary.TryGetValue(key, out result);
        return result;
    }

    /// <summary>
    /// Returns the default value of type U if the key does not exist in the dictionary
    /// </summary>
    public static U GetOrDefault<T, U>(this Dictionary<T, U> dic, T key)
    {
        if (dic.ContainsKey(key)) return dic[key];
        return default(U);
    }

    /// <summary>
    /// Returns an existing value U for key T, or creates a new instance of type U using the default constructor, 
    /// adds it to the dictionary and returns it.
    /// </summary>
    public static U GetOrInsertNew<T, U>(this Dictionary<T, U> dic, T key)
        where U : new()
    {
        if (dic.ContainsKey(key)) return dic[key];
        U newObj = new U();
        dic[key] = newObj;
        return newObj;
    }

    public static Dictionary<TKey, TElement> ToSafeDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
    {
        if (source == null)
            throw new ArgumentException("source");

        if (keySelector == null)
            throw new ArgumentException("keySelector");

        if (elementSelector == null)
            throw new ArgumentException("elementSelector");

        var dic = new Dictionary<TKey, TElement>(comparer);

        foreach (TSource element in source)
        {
            // مانع خطای وجود کلید تکراری می شود
            // System.ArgumentException: An item with the same key has already been added.
            if (!dic.ContainsKey(keySelector(element)))
                dic.Add(keySelector(element), elementSelector(element));
        }

        return dic;
    }
}
