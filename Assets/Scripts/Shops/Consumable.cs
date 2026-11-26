using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

public class Consumable : ShopItem
{
    public override float baseprice => 7;
    
    public virtual ConsumableVisual.PlacementType Placement => ConsumableVisual.PlacementType.Anywhere;

    private ConsumableVisual Visual;

    public override GameObject createVisual(Transform par)
    {
        if (sprite == null)
            sprite = FindConsumableSprite(name);

        GameObject g = GameObject.Instantiate(AssetLoader.Instance.ConsumableTemplate, par);
        if (sprite != null) g.GetComponent<SpriteRenderer>().sprite = sprite;
        SpriteRenderer s = g.transform.GetChild(0).GetComponent<SpriteRenderer>();
        Visual = g.GetComponent<ConsumableVisual>();

        //ToDo: Seperate consumable rarities maybe
        switch (rarity)
        {
            case Rarity.Common: s.color = new Color(157f / 255, 236f / 255, 156f / 255); break;
            case Rarity.Rare:   s.color = new Color(135f / 255, 201f / 255, 238f / 255); break;
            case Rarity.Epic:   s.color = new Color(201f / 255, 134f / 255, 238f / 255); break;
        }

        Visual.GetComponent<ConsumableVisual>().Consumable = this;
        Visual.GetComponent<ConsumableVisual>().type = Placement;


        return g;
    }

    private static Sprite FindConsumableSprite(string displayName)
    {
#if UNITY_EDITOR
        if (string.IsNullOrWhiteSpace(displayName)) return null;

        string normalizedName = NormalizeSpriteName(displayName);
        string[] assetGuids = AssetDatabase.FindAssets(string.Empty, new[] { "Assets/Sprites/ConsumableSprites" });
        foreach (string guid in assetGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (NormalizeSpriteName(Path.GetFileNameWithoutExtension(path)) != normalizedName)
                continue;

            Sprite foundSprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (foundSprite != null) return foundSprite;
        }
#endif
        return null;
    }

    private static string NormalizeSpriteName(string value)
    {
        return string.Concat(value.Where(char.IsLetterOrDigit)).ToLowerInvariant();
    }

    public virtual void OnDrop(Vector2 pos)
    {
        
    }
    
    public virtual bool CanUse()
    {
        return true;
    }
    
    //ToDo: Maybe consumables should always delete after being dropped, leaving it up to the consumable
    //has pros ( reusable consumables on a cooldown, limited uses ) and cons ( repeat code for different
    //consumable types, no standartization for the pros)
    public void UsedUp()
    {
        foreach (GameModifier u in GameManager.Instance.GameModifiers())        {
            u.OnConsumableUsed();
        }
        
        GameManager.Instance.Consumables.Remove(this);
        GameObject.Destroy(Visual.gameObject);
    }
    
    public override float Cost()
    {
        float finalprice = baseprice + UnityEngine.Random.Range(0, 2);
        
        switch (rarity)
        {
            case Rarity.Common: finalprice *= 1;  break;
            case Rarity.Rare:   finalprice *= 2;  break;
            case Rarity.Epic:   finalprice *= 8;  break;
        }
        return finalprice;
    }
    
}
