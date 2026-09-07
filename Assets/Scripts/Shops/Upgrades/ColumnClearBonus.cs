using System.Collections.Generic;
using UnityEngine;

public class ColumnClearBonus : Upgrade
{

    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Birch Twig";

    public override string description => "Clearing a column gains an additional 100 points";

    public override float LineClearModifier(List<Vector2Int> ClearedBlocks)
    {
        int rf = ClearedBlocks[0].x;

        foreach (Vector2Int pos in ClearedBlocks)
        {
            if (pos.x != rf) return 0;
        }

        ScoreManagement.Instance.AddScore(100);
        return 0f;
    }
}
