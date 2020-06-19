using System;
using Verse;

namespace RimGacha
{
	// Token: 0x02000007 RID: 7
	public static class RacePropertiesExtension
	{
		// Token: 0x0600000A RID: 10 RVA: 0x0000260C File Offset: 0x0000080C
		public static bool IsAnimal(this RaceProperties race)
		{
			return race.thinkTreeMain == ThinkTreeDefOf.Animal;
		}
	}
}
