using System.Collections.Generic;
using UnityEngine;

public class RowFillConsumable : Consumable
{
    public override Rarity rarity { get => Rarity.Rare; }

    public override ConsumableVisual.PlacementType Placement { get => ConsumableVisual.PlacementType.OnBoard; }


    public override string name => "Licorice Stick";

    public override string description() => "Use to clear a full row.";

    private Color color;
    public Sprite sprite;

    public override void OnDrop(Vector2 pos)
    {
        Vector2Int origin = GameBoard.instance.ClosestBlock(pos, true);

        //ToDO: I should just make a unified placeblock method for colors and stuff
        color = ThemerScript.Instance.CurrentTheme.BlockColors.GetRandomItem();
        sprite = ThemerScript.Instance.CurrentTheme.BlockSprites.GetRandomItem();


        for(int i = 0; i < GameBoard.instance.BoardSize().x; i++) {
        
            if (GameBoard.instance.GridMap[i, origin.y] == false)GameBoard.instance.SetBlockAtPos(i, origin.y, color, sprite);
        }
        GameBoard.instance.ClearBoard();
        UsedUp();
    }
    
}
