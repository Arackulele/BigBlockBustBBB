using System.Collections.Generic;
using UnityEngine;

public class LowMultMoney : Upgrade
{

    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Lucky Penny";

    public override string description => "At 0 combo, clearing a line rewards 5 coins";

    public override float LineClearModifier(List<Vector2Int> ClearedBlocks)
    {

        if (ScoreManagement.Instance.Combo == 1) ScoreManagement.Instance.UnspentScore += 5;
        return 0f;
    }
}
