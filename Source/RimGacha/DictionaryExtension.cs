using System.Collections.Generic;

namespace RimGacha;

public static class DictionaryExtension
{
    public static void EnsureKey<K, V>(this Dictionary<K, V> dict, K key, V fallbackVal)
    {
        if (!dict.ContainsKey(key))
        {
            dict.Add(key, fallbackVal);
        }
    }
}