using System.Collections.Generic;
using UnityEngine;

public class MoneyMultBonus : Upgrade
{

    public override Rarity rarity { get => Rarity.Epic; }
    
    public override string name => "Piggybank";

    public override string description => "Gain 5% of your current Money as mult";

    public override float GlobalPointsMultiplier()
    {
        if (ScoreManagement.Instance.UnspentScore > 50) return (float)(ScoreManagement.Instance.UnspentScore * 0.05f);
        return 0;
    }
}
