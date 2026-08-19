using System;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : Purchasable
{
    //ToDo:Gain Points on condition set
    //Get specific Block Types in Block roll

    [SerializeField]
    private Sprite sprite;

    public virtual Rarity rarity { get; }
    
    public virtual string name { get; }
    
    public virtual string description { get; }

    public override double price()
    {
        double Price = 30 + UnityEngine.Random.Range(2, 10);
        
        switch (rarity)
        {
            case Rarity.Common: Price *= 1; break;
            case Rarity.Rare:   Price *= 2;  break;
            case Rarity.Epic:   Price *= 5;  break;
        }
        
        
        return Price;
    }

    

    public override GameObject createVisual(Transform par)
    {
        
        GameObject g = GameObject.Instantiate(AssetLoader.Instance.UpgradeTemplate, par);
        if (sprite != null) g.GetComponent<SpriteRenderer>().sprite = sprite;
        SpriteRenderer s = g.transform.GetChild(0).GetComponent<SpriteRenderer>();

        switch (rarity)
        {
            case Rarity.Common: s.color = new Color(157f / 255, 236f / 255, 156f / 255); break;
            case Rarity.Rare:   s.color = new Color(135f / 255, 201f / 255, 238f / 255); break;
            case Rarity.Epic:   s.color = new Color(201f / 255, 134f / 255, 238f / 255); break;
        }

        g.GetComponent<UpgradeVisual>().upgrade = this;

        return g;
    }

    public virtual float LineClearModifier(List<Vector2Int> ClearedBlocks)
    {
        return 0;
    }

    public virtual int BlockPlaceModifier(List<Vector2Int> PlacedPositions, UnplacedBlockScript Block)
    {
        return 0;
    }

    public virtual float ShopPriceModifier(Purchasable Item)
    {
        return 0;
    }

    public virtual float ShopBarModifier()
    {
        return 0;
    }

    public virtual int ComboGainMultiplier()
    {
        return 0;
    }

    public virtual float GlobalPointsMultiplier()
    {
        return 0;
    }

}

public enum Rarity
{
    Common,
    Rare,
    Epic
}

