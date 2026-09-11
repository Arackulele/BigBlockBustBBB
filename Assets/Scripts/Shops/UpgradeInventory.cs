using System.Collections.Generic;
using UnityEngine;

public class UpgradeInventory : ShopItemInventory
{
    public List<GameObject> UpgradeVisuals = new();

    protected override List<GameObject> ItemVisuals => UpgradeVisuals;

    protected override void PopulatebyList()
    {
        Populate(GameManager.Instance.Perks, false);
    }

    protected void Populate(List<Upgrade> upgrades, bool Shop)
    {
        Populate(upgrades, Shop, visual => visual.GetComponent<UpgradeVisual>().IsShop = true);
    }
}
