using System.Collections.Generic;
using UnityEngine;

public class GainTomatoMultReset : Upgrade
{

    public override Rarity rarity { get => Rarity.Rare; }
    
    public override string name => "Can of Soup";

    public override string description => "Whenever your mult is reset, gain a cherry tomato";

    public override void OnMultReset(int PreviousMult)
    {
        GameManager.Instance.AddConsumable(new GainMultConsumable());
    }
}
