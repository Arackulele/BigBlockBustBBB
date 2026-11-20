using System.Collections.Generic;
using UnityEngine;

public class FarmEpicUpgrade : Upgrade
{

    public override Rarity rarity { get => Rarity.Rare; }
    
    public override string name => "Amethyst Geode";

    public override string description() => "After reaching a 10x combo, replace this upgrade with a random epic upgrade.";

    private int Score = 5000;

    public override int ComboGainMultiplier()
    {
        if (ScoreManagement.Instance.Combo > 9)
        {
            OnActivate();
            GameManager.Instance.ChangeMaxPerks(1);
            GameManager.Instance.AddUpgrade(ShopManager.instance.shopInventory.GetUpgradeOfRarity(Rarity.Rare));
            Remove();
            GameManager.Instance.ChangeMaxPerks(-1);
        }
        return 0;
    }
}