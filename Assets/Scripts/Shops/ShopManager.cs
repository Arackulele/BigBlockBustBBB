using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ShopInventory shopInventory;
    
    public static ShopManager instance;

    public bool active = false;

    public GameObject ContinueButton;

    public int ShopAmount = 3;
    
    private void Awake()
    {
        instance = this;
    }

    public void SetUpShop()
    {
        shopInventory.UpdateArea();
    }

    public void GoToShop()
    {
        shopInventory.Upgrades.Clear();
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
