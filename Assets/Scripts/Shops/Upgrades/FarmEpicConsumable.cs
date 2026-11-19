using System.Collections.Generic;
using UnityEngine;

public class FarmEpicConsumable : Upgrade
{

    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Cracked Geode";

    public override string description => "After gaining <color=blue>" + Score + "</color> score, remove this upgrade and gain a random epic consumable.";

    private int Score = 5000;

    public override void OnScoreGained(int amount)
    {
        if (amount <= 0 || !GameManager.Instance.Perks.Contains(this))
            return;

        Score -= amount;

        if (Score <= 0)
        {
            OnActivate();
            GameManager.Instance.AddConsumable(ShopManager.instance.consumableShop.GetConsumableOfRarity(Rarity.Epic));
            Remove();

        }
    }
}
