using System.Collections.Generic;
using UnityEngine;

public class ScaleMultClears : Upgrade
{

    public override Rarity rarity { get => Rarity.Rare; }
    
    public override string name => "Ball of Tinfoil";
    
    public override string description() =>
        "Gain <color=red>" + Mult + "</color>, this increases permanently by 0.2 for each 3+ line clear.";

    private double Mult = 0.2;

    public override float LineClearModifier(List<Vector2Int> clearedBlocks, int clearCountRows, int clearCountCollumns, int clearCountMisc)
    {
        int clearCountTotal = clearCountRows + clearCountCollumns + clearCountMisc;
        if (clearCountTotal > 2)
        {
            Mult += 0.2;
            OnActivate();
        }
        return 0;
    }
    

    public override float GlobalPointsMultiplier()
    {
        return (float)Mult;
    }
}