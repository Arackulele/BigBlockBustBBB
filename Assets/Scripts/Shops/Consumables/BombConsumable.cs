using System.Collections.Generic;
using UnityEngine;

public class BombConsumable : AffectBlocksConsumable
{
    public override Rarity rarity { get => Rarity.Rare; }
    
    public override string name => "Hard Candy";

    public override string description() => "Place on a Tile on the Board to remove all placed Blocks in a 3x3 Area.";

    public override bool isDestroying => true;

    public override List<Vector2Int> Matrix => new List<Vector2Int>()
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
    
}
