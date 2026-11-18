using System.Collections.Generic;
using UnityEngine;

public class ComboPayoutBonus : Upgrade
{

    public override Rarity rarity { get => Rarity.Epic; }
    
    public override string name => "Novelty Coin";

    public override string description => "When reaching a 10x Combo, gain 4000 Points";

    public override int ComboGainMultiplier()
    {
        if (ScoreManagement.Instance.Combo == 10)
        {
            ScoreManagement.Instance.AddRawScore(4000);
            OnActivate();
        }
        return 0;
    }
}
