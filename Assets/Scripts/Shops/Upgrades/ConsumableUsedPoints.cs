using System.Collections.Generic;
using UnityEngine;

public class ConsumableUsedPoints : Upgrade
{

    public override Rarity rarity { get => Rarity.Rare; }
    
    public override string name => "Recycling Bin";

    public override string description() => "Whenever a consumable is used up, gain 10 Points";

    public override void OnConsumableUsed()
    {
        OnActivate();
        ScoreManagement.Instance.AddScore(10);
    }
}
