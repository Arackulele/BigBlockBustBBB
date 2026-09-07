using System.Collections.Generic;
using UnityEngine;

public class ComboPayoutBonus : Upgrade
{

    public override Rarity rarity { get => Rarity.Epic; }
    
    public override string name => "Novelty Coin";

    public override string description => "When reaching a 25x Combo, gain 5000 Points";

    public override float LineClearModifier(List<Vector2Int> ClearedBlocks)
    {
        if (ScoreManagement.Instance.Combo == 25) ScoreManagement.Instance.AddRawScore(5000);
        return 0;
    }
}
