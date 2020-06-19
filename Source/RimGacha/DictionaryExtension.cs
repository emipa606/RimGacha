using System;
using System.Collections.Generic;

namespace RimGacha
{
	// Token: 0x02000006 RID: 6
	public static class DictionaryExtension
	{
		// Token: 0x06000009 RID: 9 RVA: 0x000025E4 File Offset: 0x000007E4
		public static void EnsureKey<K, V>(this Dictionary<K, V> dict, K key, V fallbackVal)
		{
			bool flag = !dict.ContainsKey(key);
			if (flag)
			{
				dict.Add(key, fallbackVal);
			}
		}
	}
}
