using System.Collections.Generic;
using UnityEngine;

public class GlobalLowMultBonus : Upgrade
{

    public override Rarity rarity { get => Rarity.Rare; }

    public override float GlobalPointsMultiplier()
    {
        if (ScoreManagement.Instance.Combo < 3) return 3;
        return 0;
    }
}
