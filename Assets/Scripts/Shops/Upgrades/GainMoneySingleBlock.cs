using System.Collections.Generic;
using UnityEngine;

public class GainMoneySingleBlock : Upgrade
{

    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Magnifying Glass";

    public override string description() => "Whenever you place a single piece block, gain 4 Money and 8 extra points.";

    public override int BlockPlaceModifier(List<Vector2Int> placedPositions, UnplacedBlockScript block)
    {

        if (placedPositions.Count == 1)
        {
            OnActivate();
            ScoreManagement.Instance.UnspentScore += 4;
            return 8;
        }
        else return 0;
    }
}
