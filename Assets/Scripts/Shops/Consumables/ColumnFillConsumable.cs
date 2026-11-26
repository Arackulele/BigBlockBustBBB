using System.Collections.Generic;
using UnityEngine;

public class ColumnFillConsumable : AffectBlocksConsumable
{
    public override Rarity rarity { get => Rarity.Rare; }
    
    public override string name => "Pretzel Stick";

    public override string description() => "Use to clear a full column.";

    public override bool isDestroying => false;


    public override List<Vector2Int> Matrix
    {
        get
        {
            int height = GameBoard.instance.BoardSize().y;
            var matrix = new List<Vector2Int>(height);
            for (int y = -height; y < height; y++)
                matrix.Add(new Vector2Int(0, y));
            return matrix;
        }
    }
}
