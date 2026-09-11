using System.Collections.Generic;
using UnityEngine;

public class ConsumableInventory : ShopItemInventory
{
    public List<GameObject> ConsumableVisuals = new();

    protected override List<GameObject> ItemVisuals => ConsumableVisuals;

    protected override void PopulatebyList()
    {
        Populate(
            GameManager.Instance.Consumables,
            false,
            visual => visual.GetComponent<ConsumableVisual>().IsShop = true);
    }
}
