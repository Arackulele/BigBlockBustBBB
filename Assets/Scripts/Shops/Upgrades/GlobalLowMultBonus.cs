using System.Collections.Generic;
using UnityEngine;

public class GlobalLowMultBonus : Upgrade
{

    public override Rarity rarity { get => Rarity.Rare; }
    
    public override string name => "Speckled Rock";

    public override string description => "If your combo is less than 3, gain 3 mult";

    public override float GlobalPointsMultiplier()
    {
        if (ScoreManagement.Instance.Combo < 3) return 3;
        return 0;
    }
}
