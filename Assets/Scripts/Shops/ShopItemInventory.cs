using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShopItemInventory : MonoBehaviour
{
    [SerializeField] private GameObject UpperBound;
    [SerializeField] private GameObject LowerBound;
    [SerializeField] private GameObject UpgradeHolder;
    [SerializeField] private float Padding = 0.3f;

    protected abstract List<GameObject> ItemVisuals { get; }

    public void UpdateArea()
    {
        foreach (GameObject obj in ItemVisuals)
        {
            if (obj != null)
                Destroy(obj);
        }

        ItemVisuals.Clear();
        PopulatebyList();
    }

    protected abstract void PopulatebyList();

    protected void Populate<T>(List<T> items, bool shop, Action<GameObject> markAsShopVisual)
        where T : ShopItem
    {
        int count = items.Count;

        for (int i = 0; i < count; i++)
        {
            GameObject visual = items[i].createVisual(UpgradeHolder.transform);
            ItemVisuals.Add(visual);

            if (shop)
                markAsShopVisual(visual);

            if (visual != null)
                visual.transform.localPosition =
                    GetItemPosition(i, count, UpperBound.transform.localPosition.y, LowerBound.transform.localPosition.y, Padding);
        }
    }

    private Vector3 GetItemPosition(int index, int total, float upperEdge, float lowerEdge, float padding)
    {
        if (total <= 1)
            return new Vector3(UpperBound.transform.localPosition.x, (upperEdge + lowerEdge) * 0.5f, -2f);

        float usableHeight = (upperEdge - lowerEdge) - padding * (total - 1);
        float slotHeight = usableHeight / total;

        float y = upperEdge - slotHeight * 0.5f;
        y -= index * (slotHeight + padding);

        return new Vector3(UpperBound.transform.localPosition.x, y, -2f);
    }
}
