using System.Collections.Generic;
using UnityEngine;

public class KeepCombo : Upgrade
{

    public override Rarity rarity { get => Rarity.Epic; }
    
    public override string name => "Insurance Policy";

    public override string description => "Whenever you clear a stage, your combo is kept.";

    public override int ComboGainMultiplier()
    {
        return 1;
    }
}
