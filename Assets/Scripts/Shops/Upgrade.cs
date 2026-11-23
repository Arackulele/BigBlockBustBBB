using System;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

public class Upgrade : ShopItem
{

    public override float baseprice => 26;
    
    private UpgradeVisual Visual;

    public override GameObject createVisual(Transform par)
    {
        if (sprite == null)
            sprite = FindUpgradeSprite(name);

        GameObject g = GameObject.Instantiate(AssetLoader.Instance.UpgradeTemplate, par);
        if (sprite != null) g.GetComponent<SpriteRenderer>().sprite = sprite;
        SpriteRenderer s = g.transform.GetChild(0).GetComponent<SpriteRenderer>();

        Visual = g.GetComponent<UpgradeVisual>();

        switch (rarity)
        {
            case Rarity.Common: s.color = new Color(157f / 255, 236f / 255, 156f / 255); break;
            case Rarity.Rare:   s.color = new Color(135f / 255, 201f / 255, 238f / 255); break;
            case Rarity.Epic:   s.color = new Color(201f / 255, 134f / 255, 238f / 255); break;
        }

        Visual.upgrade = this;

        return g;
    }

    private static Sprite FindUpgradeSprite(string displayName)
    {
    #if UNITY_EDITOR
        if (string.IsNullOrWhiteSpace(displayName)) return null;

        string normalizedName = NormalizeSpriteName(displayName);
        // Search all assets in the folder and load them as Sprite. FindAssets("t:Sprite")
        // can omit texture assets depending on their importer state.
        string[] assetGuids = AssetDatabase.FindAssets(string.Empty, new[] { "Assets/Sprites/UpgradeSprites" });
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
        return string.Concat(value.Where(character => char.IsLetterOrDigit(character))).ToLowerInvariant();
    }

    public void OnActivate()
    {
        if (Visual != null)Visual.StartCoroutine(Visual.OnActivate());
    }

    
    public void Remove()
    {
        OnRemoved();
        GameManager.Instance.StartCoroutine(WaitThenRemove());
    }

    private IEnumerator WaitThenRemove()
    {
        yield return new WaitForEndOfFrame();
        GameManager.Instance.Perks.Remove(this);
        ShopManager.instance.shopInventory.Upgrades.Add(this);
        yield return new WaitForEndOfFrame();
        GameObject.Destroy(Visual.gameObject);
    }

    public override float Cost()
    {
        float finalprice = baseprice + UnityEngine.Random.Range(1, 10);
        
        switch (rarity)
        {
            case Rarity.Common: finalprice *= 1;  break;
            case Rarity.Rare:   finalprice *= 3;  break;
            case Rarity.Epic:   finalprice *= 8;  break;
        }
        return finalprice;
    }
}
