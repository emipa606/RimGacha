﻿using Verse;

namespace RimGacha
{
    // Token: 0x02000013 RID: 19
    public class Comp_AnimalDesign : ThingComp
    {
        // Token: 0x04000007 RID: 7
        private PawnKindDef animalInt;

        // Token: 0x06000024 RID: 36 RVA: 0x00002C7C File Offset: 0x00000E7C
        public Comp_AnimalDesign()
        {
            animalInt = Find.World.GetComponent<WorldComp_BiomeManager>().GetRandomGlobalAnimalWeighted();
        }

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000023 RID: 35 RVA: 0x00002C64 File Offset: 0x00000E64
        public PawnKindDef Animal => animalInt;

        // Token: 0x06000025 RID: 37 RVA: 0x00002C9B File Offset: 0x00000E9B
        public void SetAnimalDesign(PawnKindDef animal)
        {
            animalInt = animal;
        }

        // Token: 0x06000026 RID: 38 RVA: 0x00002CA5 File Offset: 0x00000EA5
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Defs.Look(ref animalInt, "animalDesign");
        }

        // Token: 0x06000027 RID: 39 RVA: 0x00002CC0 File Offset: 0x00000EC0
        public override bool AllowStackWith(Thing other)
        {
            return other.TryGetAnimalDesign(out var pawnKindDef) && animalInt == pawnKindDef;
        }

        // Token: 0x06000028 RID: 40 RVA: 0x00002CE8 File Offset: 0x00000EE8
        public override void PostSplitOff(Thing piece)
        {
            base.PostSplitOff(piece);
            piece.TryGetComp<Comp_AnimalDesign>().animalInt = animalInt;
        }

        // Token: 0x06000029 RID: 41 RVA: 0x00002D04 File Offset: 0x00000F04
        public override string TransformLabel(string label)
        {
            var array = label.Split(' ');
            return string.Concat(array[0], " ", animalInt.label, " ", string.Join(" ", array, 1, array.Length - 1));
        }

        // Token: 0x0600002A RID: 42 RVA: 0x00002D6B File Offset: 0x00000F6B
    }
}