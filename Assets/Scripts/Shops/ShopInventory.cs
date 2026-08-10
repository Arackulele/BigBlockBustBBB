using System.Collections.Generic;
using UnityEngine;

public class ShopInventory : UpgradeInventory
{
    public List<Upgrade> Upgrades;
    
    protected new void PopulatebyList()
    {
        Populate(Upgrades);
    }
    
}
