using System.Collections.Generic;
using UnityEngine;

public class UpgradeSlotCostMoney : Upgrade
{

    public override Rarity rarity { get => Rarity.Rare; }
    
    public override string name => "Parking Ticket";

    public override string description() => "Gain 2 Upgrade Slots, but pay 20 Money at the end of every stage. If you can't pay the price, this upgrade is removed.";

    public override void OnAdded()
    {
        GameManager.Instance.ChangeMaxPerks(2);
    }

    public override void OnRemoved()
    {
        GameManager.Instance.ChangeMaxPerks(-2);
    }

    public override void OnStageCleared()
    {
        if (ScoreManagement.Instance.SpendMoney(20)) { }
        else Remove();
    }
}
