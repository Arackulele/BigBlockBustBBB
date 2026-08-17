using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ShopInventory shopInventory;
    
    public static ShopManager instance;

    public bool active = false;

    private void Awake()
    {
        instance = this;
    }

    public void SetUpShop()
    {
        shopInventory.Upgrades = new List<Upgrade>() { new RowClearBonus(), new GlobalLowMultBonus(), new MoneyMultBonus() };
        shopInventory.UpdateArea();
    }

    public void GoToShop()
    {
        active = true;
        GameBoard.instance.gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
        SetUpShop();

    }
}
