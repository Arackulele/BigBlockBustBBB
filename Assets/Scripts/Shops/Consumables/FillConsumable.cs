using System.Collections.Generic;
using UnityEngine;

public class FillConsumable : Consumable
{
    public override Rarity rarity { get => Rarity.Rare; }

    public override ConsumableVisual.PlacementType Placement { get => ConsumableVisual.PlacementType.OnBoard; }


    public override string name => "Hot Soup";

    public override string description => "Fill an area of Tiles, with a maximum of 12 in total.";
    
    private static List<Vector2Int> Matrix = new List<Vector2Int>()
    {
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0),
    };

    private Color color;
    public Sprite sprite;

    private int Counter = 12;
    

    public override void OnDrop(Vector2 pos)
    {
        Vector2Int origin = GameBoard.instance.ClosestBlock(pos, true);

        //ToDO: I should just make a unified placeblock method for colors and stuff
        color = ThemerScript.Instance.CurrentTheme.BlockColors.GetRandomItem();
        sprite = ThemerScript.Instance.CurrentTheme.BlockSprites.GetRandomItem();


        GameBoard.instance.SetBlockAtPos(origin.x, origin.y, color, sprite);
        CheckAdjacent(origin);

        GameBoard.instance.ClearBoard();
        UsedUp();
    }

    private void CheckAdjacent(Vector2Int pos)
    {
        foreach (Vector2Int matrixpos in Matrix)
        {
            if (Counter < 1) break;
            Vector2Int finalpos = pos + matrixpos;
            if (GameBoard.instance.IsValidPosition(finalpos) && !GameBoard.instance.GridMap[finalpos.x, finalpos.y])
            {
                Counter--;
                GameBoard.instance.SetBlockAtPos(finalpos.x, finalpos.y, color, sprite);
                CheckAdjacent(finalpos);
            }
        }
    }
}
