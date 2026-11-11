using System.Collections.Generic;
using UnityEngine;

public class ScaleMultClears : Upgrade
{

    public override Rarity rarity { get => Rarity.Rare; }
    
    public override string name => "Ball of Tinfoil";

    //ToDo: Dynamic description
    public override string description => "Gain 0.2 mult, this increases permanently by 0.2 for each 3+ line clear.";

    private double Mult = 0.2f;

    public override float LineClearModifier(List<Vector2Int> clearedBlocks)
    {
        if (clearedBlocks.Count > 20) Mult += 0.2f;
        return 0;
    }

    public override float GlobalPointsMultiplier()
    {
        return (float)Mult;
    }
}