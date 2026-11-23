using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GardensStage : Stage
{
    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Pink Gardens";

    public override string description() => "Clearing a line with only same color blocks awards +2 mult.";

    public override double pointsmod => 1.175;

    public override Theme Theme { get => AssetLoader.Instance.GardensStage_Theme; }

    public override float LineClearModifier(List<Vector2Int> clearedBlocks, int clearCountRows, int clearCountCollumns, int clearCountMisc)
    {
        //ToDo: Why the fuck havent i just implemented a method to get the blockscript directly in game manager
        Color c = GameBoard.instance.GetBlockAtPosition(clearedBlocks[0]).GetComponent<BlockScript>().Color;

        foreach (Vector2Int i in clearedBlocks)
        {
            Color comp = GameBoard.instance.GetBlockAtPosition(i).GetComponent<BlockScript>().Color;

            if (c != comp) return 0;
        }

        return 2f;
    }
}
