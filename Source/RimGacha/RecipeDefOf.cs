using System;
using RimWorld;
using Verse;

namespace RimGacha
{
	// Token: 0x02000002 RID: 2
	[DefOf]
	public static class RecipeDefOf
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		static RecipeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RecipeDefOf));
		}

		// Token: 0x04000001 RID: 1
		public static RecipeDef Make_Gachapon;
	}
}
