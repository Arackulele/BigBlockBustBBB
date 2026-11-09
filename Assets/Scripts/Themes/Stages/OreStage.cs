using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OreStage : Stage
{
    public override Rarity rarity { get => Rarity.Rare; }
    
    public override string name => "Metalworks";

    public override string description => "When a line is cleared, bronze blocks add -0.5 to the modifier, gold blocks add 0.5.";
    
    public override double pointsmod => 1;

    public override Theme Theme { get => AssetLoader.Instance.OreStage_Theme; }
    
    public override float LineClearModifier(List<Vector2Int> ClearedBlocks)
    {
        Color bronze = Theme.BlockColors[0];
        Color gold = Theme.BlockColors[2];

        float finalmod = 0;

        foreach (Vector2Int blockpos in ClearedBlocks)
        {
            Color Match = GameBoard.instance.GetBlockAtPosition(blockpos).GetComponent<BlockScript>().Color;
            if (Match == bronze) finalmod -= 0.5f;
            else if (Match == gold) finalmod += 0.5f;
        }

        return finalmod;
    }
    
}
