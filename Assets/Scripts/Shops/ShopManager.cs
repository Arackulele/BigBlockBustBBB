using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ShopInventory shopInventory;
    
    public static ShopManager instance;

    private void Start()
    {
        instance = this;

        SetUpShop();
    }

    public void SetUpShop()
    {
        shopInventory.Upgrades = new List<Upgrade>() { new GlobalLowMultBonus(), new MoneyMultBonus(), new RowClearBonus() };
        shopInventory.UpdateArea();
    }
}
