using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShopItemInventory : MonoBehaviour
{
    [SerializeField] private Transform ItemHolder;
    [SerializeField] private Bounds LayoutBounds = new Bounds(new Vector3(0f, 0f, -2f), new Vector3(3f, 3f, 0f));
    [SerializeField] private ShopInventoryLayoutDirection LayoutDirection = ShopInventoryLayoutDirection.Vertical;
    [SerializeField, Min(1)] private int ItemsPerLine = 10;
    [SerializeField, Min(0f)] private Vector2 Padding = new Vector2(0.3f, 0.3f);

    protected abstract List<GameObject> ItemVisuals { get; }

    public void UpdateArea()
    {
        List<GameObject> visuals = ItemVisuals;
        foreach (GameObject visual in visuals)
        {
            if (visual != null)
                Destroy(visual);
        }

        visuals.Clear();
        PopulatebyList();
    }

    protected abstract void PopulatebyList();

    protected void Populate<T>(List<T> items, bool shop, Action<GameObject> markAsShopVisual)
        where T : ShopItem
    {
        if (items == null)
            return;

        int count = items.Count;

        for (int i = 0; i < count; i++)
        {
            T item = items[i];
            if (item == null)
                continue;

            GameObject visual = item.createVisual(VisualParent);
            if (visual == null)
                continue;

            ItemVisuals.Add(visual);

            if (shop)
                markAsShopVisual(visual);

            PositionVisual(visual.transform, GetItemPosition(i, count));
        }
    }

    private Transform VisualParent => ItemHolder != null ? ItemHolder : transform;

    private void PositionVisual(Transform visualRoot, Vector3 layoutPosition)
    {
        Transform visualCenter = visualRoot.childCount > 0
            ? visualRoot.GetChild(0)
            : visualRoot;

        visualRoot.localPosition = layoutPosition;

        Vector3 centerPosition = VisualParent.InverseTransformPoint(visualCenter.position);
        visualRoot.localPosition += layoutPosition - centerPosition;
    }

    private Vector3 GetItemPosition(int index, int total)
    {
        int itemsInLine = Mathf.Min(Mathf.Max(1, ItemsPerLine), total);
        int row;
        int column;
        int rowCount;
        int columnCount;

        if (LayoutDirection == ShopInventoryLayoutDirection.Vertical)
        {
            rowCount = itemsInLine;
            columnCount = Mathf.CeilToInt(total / (float)rowCount);
            row = index % rowCount;
            column = index / rowCount;
        }
        else
        {
            columnCount = itemsInLine;
            rowCount = Mathf.CeilToInt(total / (float)columnCount);
            row = index / columnCount;
            column = index % columnCount;
        }

        float x = GetAxisPosition(column, columnCount, LayoutBounds.min.x, LayoutBounds.max.x, Padding.x);
        float y = GetAxisPosition(row, rowCount, LayoutBounds.min.y, LayoutBounds.max.y, Padding.y, true);

        return new Vector3(x, y, LayoutBounds.center.z);
    }

    private static float GetAxisPosition(int index, int count, float min, float max, float padding, bool descending = false)
    {
        float size = Mathf.Max(0f, max - min);
        float edgePadding = Mathf.Min(padding, size * 0.5f);
        float contentMin = min + edgePadding;
        float contentSize = Mathf.Max(0f, size - edgePadding * 2f);
        float gap = count > 1
            ? Mathf.Min(padding, contentSize / (count - 1))
            : 0f;
        float cellSize = count > 0
            ? Mathf.Max(0f, (contentSize - gap * (count - 1)) / count)
            : 0f;
        float position = contentMin + cellSize * 0.5f + index * (cellSize + gap);

        return descending ? min + max - position : position;
    }

    private void OnDrawGizmosSelected()
    {
        Matrix4x4 previousMatrix = Gizmos.matrix;
        Color previousColor = Gizmos.color;

        Gizmos.matrix = VisualParent.localToWorldMatrix;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(LayoutBounds.center, LayoutBounds.size);

        Gizmos.matrix = previousMatrix;
        Gizmos.color = previousColor;
    }
}

public enum ShopInventoryLayoutDirection
{
    Vertical,
    Horizontal
}
