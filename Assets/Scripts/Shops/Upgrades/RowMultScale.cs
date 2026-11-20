using System.Collections.Generic;
using UnityEngine;

public class RowMultScale : Upgrade
{

    public override Rarity rarity { get => Rarity.Rare; }
    
    public override string name => "Silver Ladle";

    public override string description() =>
        "Gain <color=red>" + Mult + "</color> mult, this increases by 0.05 for every row cleared.";

    private double Mult = 0.05;

    public override float LineClearModifier(List<Vector2Int> clearedBlocks, int clearCountRows, int clearCountCollumns, int clearCountMisc)
    {
  
        if (clearCountRows > 0)
        {
        Mult += 0.05 * clearCountRows;
        OnActivate();
        }
        return 0f;
    }

    public override float GlobalPointsMultiplier()
    {
        return (float)Mult;
    }
}
