using System;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : ShopItem
{
    //ToDo:Gain Points on condition set
    //Get specific Block Types in Block roll

    public override float baseprice => 5;

    public override GameObject createVisual(Transform par)
    {
        
        GameObject g = GameObject.Instantiate(AssetLoader.Instance.ConsumableTemplate, par);
        if (sprite != null) g.GetComponent<SpriteRenderer>().sprite = sprite;
        SpriteRenderer s = g.transform.GetChild(0).GetComponent<SpriteRenderer>();

        switch (rarity)
        {
            case Rarity.Common: s.color = new Color(157f / 255, 236f / 255, 156f / 255); break;
            case Rarity.Rare:   s.color = new Color(135f / 255, 201f / 255, 238f / 255); break;
            case Rarity.Epic:   s.color = new Color(201f / 255, 134f / 255, 238f / 255); break;
        }

        g.GetComponent<ConsumableVisual>().Consumable = this;

        return g;
    }
}
