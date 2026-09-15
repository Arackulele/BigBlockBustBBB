using System;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableShop : ConsumableInventory
{
    //ToDo: This class is ugly and it and upgrade Shop share a ton of duplicate code, this needs another layer of abstraction
    public List<Consumable> ConsumableIndex =
        new List<Consumable>()
        {
            new GainScoreConsumable()
        };
    
    public List<Consumable> CommonConsumables = new List<Consumable>();
    public List<Consumable> RareConsumables = new List<Consumable>();
    public List<Consumable> EpicConsumables = new List<Consumable>();

    public float RareChance = 30f;
    public float EpicChance = 3f;
    
    public List<Consumable> Consumables = new List<Consumable>();


    
    protected override void PopulatebyList()
    {
        
        for(int c = ShopManager.instance.ShopAmount; c > 0; c--)
        {
            Consumable potential = SelectConsumable(Consumables);
            if (potential != null )Consumables.Add(potential);
        }
        
        Populate(Consumables, true, visual => visual.GetComponent<UpgradeVisual>().IsShop = true);
    }

    private Consumable SelectConsumable(List<Consumable> excluded)
    {
        Consumable Selected = null;
        bool Completed = false;
        int max = 0;
        
        while (!Completed && max < 10)
        {
            if (UnityEngine.Random.Range(0f, 100f) <= RareChance) Selected = RareConsumables.GetRandomItem();
            else if (UnityEngine.Random.Range(0f, 100f) <= EpicChance) Selected = EpicConsumables.GetRandomItem();
            else Selected = CommonConsumables.GetRandomItem();

            if (!excluded.Contains(Selected)) Completed = true;
            else Selected = null; 
            max++;
        }
        return Selected;
    }

    private void Awake()
    {
        foreach (Consumable consumable in ConsumableIndex)
        {
            switch (consumable.rarity)
            {
                case Rarity.Common: CommonConsumables.Add(consumable); break;
                case Rarity.Rare: RareConsumables.Add(consumable); break;
                case Rarity.Epic: EpicConsumables.Add(consumable); break;
            }
            
            
        }
        
    }
}
