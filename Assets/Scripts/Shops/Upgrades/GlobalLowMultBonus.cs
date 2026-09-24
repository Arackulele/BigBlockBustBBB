using System.Collections.Generic;
using UnityEngine;

public class GlobalLowMultBonus : Upgrade
{

    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Speckled Rock";

    public override string description => "If your combo is less than 3, gain 2.5 mult";

    public override float GlobalPointsMultiplier()
    {
        if (ScoreManagement.Instance.Combo < 3) return 2.5f;
        return 0;
    }
}
