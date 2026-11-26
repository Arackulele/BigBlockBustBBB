using System.Collections.Generic;
using UnityEngine;

public class RowFillConsumable : AffectBlocksConsumable
{
    public override Rarity rarity { get => Rarity.Rare; }

    public override string name => "Licorice Stick";

    public override string description() => "Use to clear a full row.";

    public override bool isDestroying => false;

    public override List<Vector2Int> Matrix
    {
        get
        {
            int width = GameBoard.instance.BoardSize().x;
            var matrix = new List<Vector2Int>(width);
            for (int y = -width; y < width; y++)
                matrix.Add(new Vector2Int(width, 0));
            return matrix;
        }
    }
    
}
