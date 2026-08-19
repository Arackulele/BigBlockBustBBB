using System.Collections.Generic;
using UnityEngine;

public class ShopInventory : UpgradeInventory
{
    public List<Upgrade> Upgrades;
    
    protected override void PopulatebyList()
    {
        Debug.Log("Amount of shop upgrades rendering:" +  Upgrades.Count);
        Populate(Upgrades, true);
    }
    
}
