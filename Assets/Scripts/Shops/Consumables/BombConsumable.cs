using System.Collections.Generic;
using UnityEngine;

public class BombConsumable : Consumable
{
    public override Rarity rarity { get => Rarity.Rare; }

    public override ConsumableVisual.PlacementType Placement { get => ConsumableVisual.PlacementType.OnBoard; }
    
    public override string name => "Hard Candy";

    public override string description => "Place on a Tile on the Board to remove all placed Blocks in a 3x3 Area.";
    

    private static List<Vector2Int> Matrix = new List<Vector2Int>()
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(0, -1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, -1),
        
    };

    public override void OnDrop(Vector2 pos)
    {
        Vector2Int origin = GameBoard.instance.ClosestBlock(pos);

        foreach (Vector2Int matrixpos in Matrix)
        {
            Vector2Int finalpos = origin + matrixpos;
            if ( GameBoard.instance.IsValidPosition(finalpos) )
            GameBoard.instance.ClearBlockAtPos(origin + matrixpos);
        }
        UsedUp();
    }
}
