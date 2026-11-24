using System.Collections.Generic;
using UnityEngine;

public class ColumnFillConsumable : Consumable
{
    public override Rarity rarity { get => Rarity.Rare; }

    public override ConsumableVisual.PlacementType Placement { get => ConsumableVisual.PlacementType.OnBoard; }


    public override string name => "Licorice Stick";

    public override string description() => "Use to clear a full column.";

    private Color color;
    public Sprite sprite;
    
    public override void OnDrop(Vector2 pos)
    {
        Vector2Int origin = GameBoard.instance.ClosestBlock(pos, true);

        //ToDO: I should just make a unified placeblock method for colors and stuff
        color = ThemerScript.Instance.CurrentTheme.BlockColors.GetRandomItem();
        sprite = ThemerScript.Instance.CurrentTheme.BlockSprites.GetRandomItem();


        for(int i = 0; i < GameBoard.instance.BoardSize().y; i++) {
        
            if (GameBoard.instance.GridMap[origin.x, i] == false)GameBoard.instance.SetBlockAtPos(origin.x, i, color, sprite);
        }
        GameBoard.instance.ClearBoard();
        UsedUp();
    }
    
}
