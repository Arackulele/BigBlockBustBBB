using System.Collections.Generic;
using UnityEngine;

public abstract class AffectBlocksConsumable : Consumable
{
    public override ConsumableVisual.PlacementType Placement { get => ConsumableVisual.PlacementType.OnBoard; }

    public virtual List<Vector2Int> Matrix => null;

    public virtual bool isDestroying => false;

    public override void OnDrop(Vector2 pos)
    {
        Vector2Int origin = GameBoard.instance.ClosestBlock(pos);

        foreach (Vector2Int matrixpos in Matrix)
        {
            Vector2Int finalpos = origin + matrixpos;
            if ( GameBoard.instance.IsValidPosition(finalpos) )
            {
                if (isDestroying) GameBoard.instance.ClearBlockAtPos(origin + matrixpos);
                else if (!GameBoard.instance.GridMap[finalpos.x, finalpos.y])GameBoard.instance.SetBlockSimple(finalpos);
            }
        }
        GameBoard.instance.ClearBoard();
        UsedUp();
    }
}
