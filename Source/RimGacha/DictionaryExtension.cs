using System.Collections.Generic;

namespace RimGacha;

public static class DictionaryExtension
{
    public static void EnsureKey<TK, TV>(this Dictionary<TK, TV> dict, TK key, TV fallbackVal)
    {
        dict.TryAdd(key, fallbackVal);
    }
}