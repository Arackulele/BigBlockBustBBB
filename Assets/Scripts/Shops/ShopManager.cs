using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public UpgradeShop shopInventory;
    
    public ConsumableShop consumableShop;

    
    public static ShopManager instance;

    public bool active = false;

    public GameObject ContinueButton;

    public int ShopAmount = 3;
    
    public int ConsumableAmount = 4;

    [SerializeField] private GameObject ToActivate;
    [SerializeField] private GameObject ToDeactivate;


    
    private void Awake()
    {
        instance = this;
    }

    public void SetUpShop()
    {
        shopInventory.UpdateArea();
        consumableShop.UpdateArea();
    }

    public void GoToShop()
    {
        ThemerScript.Instance.ChangeTheme(AssetLoader.Instance.Shop_Theme);
        shopInventory.Upgrades.Clear();
        consumableShop.Consumables.Clear();
        active = true;
        GameBoard.instance.gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
        ContinueButton.SetActive(true);
        foreach (GameModifier u in GameManager.Instance.GameModifiers())        {
            u.OnStageCleared();
        }
        
        ToDeactivate.SetActive(false);
        ToActivate.SetActive(true);

        StageReroller.instance.RerollPrice = 0;
        StageReroller.instance.UpdateRerollButtonText();
        
        SetUpShop();
    }



    public void ExitShop()
    {
        active = false;
        ContinueButton.SetActive(false);
        GameBoard.instance.gameObject.SetActive(true);
        if (GameManager.Instance.CurrentStage != null)
            ThemerScript.Instance.ChangeTheme(GameManager.Instance.CurrentStage.Theme);
        transform.GetChild(0).gameObject.SetActive(false);
        
        ToDeactivate.SetActive(true);
        ToActivate.SetActive(false);
    }
}
