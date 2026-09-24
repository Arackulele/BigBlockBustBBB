using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HillsStage : Stage
{
    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Grey Hills";

    public override string description => "Columns get +25% mult, Rows get -25% mult.";
    
    public override double pointsmod => 1;

    public override Theme Theme { get => AssetLoader.Instance.HillsStage_Theme; }
    
    public override float LineClearModifier(List<Vector2Int> ClearedBlocks)
    {
        int rf = ClearedBlocks[0].y;

        foreach (Vector2Int pos in ClearedBlocks)
        {
            if (pos.y != rf) return 0.25f;
        }

        return -0.25f;
    }
    
}
