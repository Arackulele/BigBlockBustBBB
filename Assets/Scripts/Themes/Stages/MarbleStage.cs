using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MarbleStage : Stage
{
    public override Rarity rarity { get => Rarity.Rare; }
    
    public override string name => "Polished Marble";

    public override string description() => "Clearing a line on the edge of the board gives double the points.";

    public override double pointsmod => 1.2;

    public override Theme Theme { get => AssetLoader.Instance.MarbleStage_Theme; }

    public override float LineClearModifier(List<Vector2Int> clearedBlocks, int clearCountRows, int clearCountCollumns, int clearCountMisc)
    {
        bool condition = true;
        foreach (Vector2Int block in clearedBlocks)
        {
            if (block.x == 0 || block.x == GameBoard.instance.BoardSize().x ||
                block.y == 0 || block.y == GameBoard.instance.BoardSize().y ) { }
            else condition = false;
        }
        
        //this is like xmult in balatro
        if (condition) return (float)ScoreManagement.Instance.GetMult();
        return 0;
    }
}
