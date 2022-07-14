using Verse;

namespace RimGacha;

public class Comp_AnimalDesign : ThingComp
{
    private PawnKindDef animalInt;

    public Comp_AnimalDesign()
    {
        animalInt = Find.World.GetComponent<WorldComp_BiomeManager>().GetRandomGlobalAnimalWeighted();
    }

    public PawnKindDef Animal => animalInt;

    public void SetAnimalDesign(PawnKindDef animal)
    {
        animalInt = animal;
    }

    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Defs.Look(ref animalInt, "animalDesign");
    }

    public override bool AllowStackWith(Thing other)
    {
        return other.TryGetAnimalDesign(out var pawnKindDef) && animalInt == pawnKindDef;
    }

    public override void PostSplitOff(Thing piece)
    {
        base.PostSplitOff(piece);
        piece.TryGetComp<Comp_AnimalDesign>().animalInt = animalInt;
    }

    public override string TransformLabel(string label)
    {
        var array = label.Split(' ');
        return string.Concat(array[0], " ", animalInt.label, " ", string.Join(" ", array, 1, array.Length - 1));
    }
}