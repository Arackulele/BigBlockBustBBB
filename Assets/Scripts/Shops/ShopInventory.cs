using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopInventory : UpgradeInventory
{
    public List<Upgrade> UpgradeIndex =
        new List<Upgrade>()
        {
            new AlwaysGainLow(),
            new ColumnClearBonus(),
            new ComboPayoutBonus(),
            new GlobalLowMultBonus(),
            new MoneyMultBonus(),
            new RowClearBonus(),
            new LowMultMoney()
        };
    
    public List<Upgrade> CommonUpgrades = new List<Upgrade>();
    public List<Upgrade> RareUpgrades = new List<Upgrade>();
    public List<Upgrade> EpicUpgrades = new List<Upgrade>();

    public float RareChance = 15f;
    public float EpicChance = 1f;
    
    public List<Upgrade> Upgrades = new List<Upgrade>();


    
    protected override void PopulatebyList()
    {
        
        for(int c = ShopManager.instance.ShopAmount; c > 0; c--)
        {
            Upgrade potential = SelectUpgrade(Upgrades);
            if (potential != null )Upgrades.Add(potential);
        }
        
        Debug.Log("Amount of shop upgrades rendering:" +  Upgrades.Count);
        Populate(Upgrades, true);
    }

    private Upgrade SelectUpgrade(List<Upgrade> excluded)
    {
        Upgrade Selected = null;
        bool Completed = false;
        int max = 0;
        
        while (!Completed && max < 10)
        {
            if (UnityEngine.Random.Range(0f, 100f) <= RareChance) Selected = RareUpgrades.GetRandomItem();
            else if (UnityEngine.Random.Range(0f, 100f) <= EpicChance) Selected = EpicUpgrades.GetRandomItem();
            else Selected = CommonUpgrades.GetRandomItem();

            if (!excluded.Contains(Selected) && !GameManager.Instance.Perks.Contains(Selected) && Selected != null) Completed = true;
            else Selected = null; 
            max++;
        }
        return Selected;
    }

    private void Awake()
    {
        foreach (Upgrade upgrade in UpgradeIndex)
        {
            switch (upgrade.rarity)
            {
                case Rarity.Common: CommonUpgrades.Add(upgrade); break;
                case Rarity.Rare: RareUpgrades.Add(upgrade); break;
                case Rarity.Epic: EpicUpgrades.Add(upgrade); break;
            }
            
            
        }
        
    }
}
