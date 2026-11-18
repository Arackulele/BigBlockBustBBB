using System.Collections.Generic;
using UnityEngine;

public class RowClearBonus : Upgrade
{

    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Silver spoon";

    public override string description => "Clearing a row has +0.5 mult";

    public override float LineClearModifier(List<Vector2Int> clearedBlocks, int clearCountRows, int clearCountCollumns, int clearCountMisc)
    {
        OnActivate();
        if (clearCountRows > 0) return 0.5f * clearCountRows;
        return 0;
    }
}
