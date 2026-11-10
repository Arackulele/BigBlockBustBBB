using System.Collections.Generic;
using UnityEngine;

public class GainScoreRisk : Upgrade
{

    public override Rarity rarity { get => Rarity.Rare; }
    
    public override string name => "Toy Skateboard";

    public override string description => "Mult rises twice as fast, but you loose 100*mult points when breaking combo.";

    public override void OnMultReset(int PreviousMult)
    {
        ScoreManagement.Instance.AddScoreNegative(100 * PreviousMult);
    }

    public override int ComboGainMultiplier()
    {
        return 1;
    }
}
