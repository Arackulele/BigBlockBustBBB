using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SandsStage : Stage
{
    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Yellow Sands";

    public override string description => "Rows get +25% mult, Columns get -25% mult.";
    
    public override double pointsmod => 1;

    public override Theme Theme { get => AssetLoader.Instance.SandStage_Theme; }
    
    public override float LineClearModifier(List<Vector2Int> ClearedBlocks)
    {
        int rf = ClearedBlocks[0].y;

        foreach (Vector2Int pos in ClearedBlocks)
        {
            if (pos.y != rf) return -0.25f;
        }

        return 0.25f;
    }
    
}
