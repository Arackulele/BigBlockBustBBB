using System.Collections.Generic;
using UnityEngine;

public class GainScoreBigBlocks : Upgrade
{

    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Empty Seashell";

    public override string description => "Whenever you place a block with 6 or more pieces, gain triple the points for it.";

    public override int BlockPlaceModifier(List<Vector2Int> placedPositions, UnplacedBlockScript block)
    {
        return placedPositions.Count * 2;
    }
}
