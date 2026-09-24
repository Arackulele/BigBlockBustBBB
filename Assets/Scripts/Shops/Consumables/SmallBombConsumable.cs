using System.Collections.Generic;
using UnityEngine;

public class SmallBombConsumable : Consumable
{
    public override Rarity rarity { get => Rarity.Common; }

    public override ConsumableVisual.PlacementType Placement { get => ConsumableVisual.PlacementType.OnBoard; }


    public override string name => "Gummy Bear";

    public override string description => "Place on a Tile on the Board to remove all placed Blocks in a cross area.";
    

    private static List<Vector2Int> Matrix = new List<Vector2Int>()
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0),
    };

    public override void OnDrop(Vector2 pos)
    {
        //ToDo: This destruction code should probably be centralized somewhere so any upgrade can use it and just feed a matrix
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
