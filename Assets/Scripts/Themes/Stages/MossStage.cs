using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MossStage : Stage
{
    public override Rarity rarity { get => Rarity.Rare; }
    
    public override string name => "Moss Grotto";

    public override string description => "Whenever you place a block, a random tile on the board will grow moss.";
    
    public override double pointsmod => 1;

    public override Theme Theme { get => AssetLoader.Instance.MossStage_Theme; }

    public override int BlockPlaceModifier(List<Vector2Int> placedPositions, UnplacedBlockScript block)
    {
        Vector2Int pos = GameBoard.instance.GetRandomPosition(true);
        if (pos != null)GameBoard.instance.SetBlockSimple(pos);
        
        GameBoard.instance.ClearBoard();
        return 0;
    }
}
