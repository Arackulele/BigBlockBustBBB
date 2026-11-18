using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HillsStage : Stage
{
    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Grey Hills";

    public override string description => "Columns get +0.5 mult, Rows get -0.5 mult.";
    
    public override double pointsmod => 1;

    public override Theme Theme { get => AssetLoader.Instance.HillsStage_Theme; }
    
    public override float LineClearModifier(List<Vector2Int> clearedBlocks, int clearCountRows, int clearCountCollumns, int clearCountMisc)
    {
        float total = 0;
        total += clearCountCollumns * 0.5f;
        total -= clearCountRows * 0.5f;

        return total;
    }
    
}
