using System.Collections.Generic;
using UnityEngine;

public class IncreaseConsumables : Upgrade
{

    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Fruit Basket";

    public override string description => "Increase Consumable Slots by 3";

    public override void OnAdded()
    {
        GameManager.Instance.ChangeMaxConsumables(3);
    }

    public override void OnRemoved()
    {
        GameManager.Instance.ChangeMaxConsumables(-3);
    }
}