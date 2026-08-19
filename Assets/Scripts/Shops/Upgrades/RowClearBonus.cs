using System.Collections.Generic;
using UnityEngine;

public class RowClearBonus : Upgrade
{

    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Olive Branch";

    public override string description => "Clearing a row is worth 50% more";

    public override float LineClearModifier(List<Vector2Int> ClearedBlocks)
    {
        int rf = ClearedBlocks[0].y;

        foreach (Vector2Int pos in ClearedBlocks)
        {
            if (pos.y != rf) return 0;
        }

        return 0.5f;
    }
}
