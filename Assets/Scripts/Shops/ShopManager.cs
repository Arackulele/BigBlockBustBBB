using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ShopInventory shopInventory;
    
    public static ShopManager instance;

    public bool active = false;

    public GameObject ContinueButton;
    
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
        ContinueButton.SetActive(true);
        SetUpShop();
    }
    
    
    public void ExitShop()
    {
        active = false;
        ContinueButton.SetActive(false);
        GameBoard.instance.gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
