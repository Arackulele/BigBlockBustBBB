using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ForestStage : Stage
{
    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Orange Forest";

    public override string description() => "Clearing a line with all 4 different colors awards +2 mult.";

    public override double pointsmod => 1.225;

    public override Theme Theme { get => AssetLoader.Instance.ForestStage_Theme; }

    public override float LineClearModifier(List<Vector2Int> clearedBlocks, int clearCountRows, int clearCountCollumns, int clearCountMisc)
    {
        List<Color> colors = new List<Color>();
        
        foreach (Vector2Int i in clearedBlocks)
        {
            colors.Add(GameBoard.instance.GetBlockAtPosition(i).GetComponent<BlockScript>().Color);
        }

        foreach (Color c in ThemerScript.Instance.CurrentTheme.BlockColors)
        {
            if (!colors.Contains(c)) return 0f;
        }

        return 2f;
    }
}
