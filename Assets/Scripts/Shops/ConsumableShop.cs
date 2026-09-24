using System;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableShop : ConsumableInventory
{
    //ToDo: This class is ugly and it and upgrade Shop share a ton of duplicate code, this needs another layer of abstraction
    public List<Consumable> ConsumableIndex =
        new List<Consumable>()
        {
            new GainScoreConsumable(),
            new GainMultConsumable(),
            new BlockRerollConsumable(),
            new BombConsumable(),
            new FillConsumable(),
            new PermaMultConsumable(),
            new UpgradeSlotIncreaseConsumable(),
            new EatMultConsumable(),
            new TurnAddConsumable(),
            new TurnLimitIncreaseConsumable(),
            new SmallBombConsumable()
        };
    
    List<Consumable> CommonConsumables = new List<Consumable>();
    List<Consumable> RareConsumables = new List<Consumable>();
    List<Consumable> EpicConsumables = new List<Consumable>();

    public float RareChance = 30f;
    public float EpicChance = 3f;
    
    public List<Consumable> Consumables = new List<Consumable>();


    
    protected override void PopulatebyList()
    {
        
        for(int c = ShopManager.instance.ConsumableAmount; c > 0; c--)
        {
            Consumable potential = SelectConsumable();
            if (potential != null )Consumables.Add(potential);
        }
        
        Populate(Consumables, true, visual => visual.GetComponent<ConsumableVisual>().IsShop = true);
    }

    public Consumable SelectConsumable()
    {
        Consumable selected;

        if (UnityEngine.Random.Range(0f, 100f) <= RareChance) selected = RareConsumables.GetRandomItem();
        else if (UnityEngine.Random.Range(0f, 100f) <= EpicChance) selected = EpicConsumables.GetRandomItem();
        else selected = CommonConsumables.GetRandomItem();

        //This is hacky and a little weird tbh
        return selected == null
            ? null
            : (Consumable)Activator.CreateInstance(selected.GetType());
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
