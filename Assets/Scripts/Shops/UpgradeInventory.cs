using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeInventory : MonoBehaviour
{
    public List<GameObject> UpgradeVisuals = new();

    [SerializeField] private GameObject UpperBound;
    [SerializeField] private GameObject LowerBound;
    [SerializeField] private GameObject UpgradeHolder;


    [SerializeField] private float Padding = 0.3f;

    public void UpdateArea()
    {
        foreach (GameObject obj in UpgradeVisuals)
        {
            if (obj != null)
                Destroy(obj);
        }
        UpgradeVisuals.Clear();
        PopulatebyList();

    }

    protected virtual void PopulatebyList()
    {
        Populate(GameManager.Instance.Perks);
    }

    protected void Populate(List<Upgrade> upgrades)
    {
        
        int count = upgrades.Count;

        for (int i = 0; i < count; i++)
        {
            Upgrade upgrade = upgrades[i];

            GameObject visual = upgrade.createVisual(UpgradeHolder.transform);
            UpgradeVisuals.Add(visual);

            visual.transform.localPosition =
                GetUpgradePosition(i, count, UpperBound.transform.localPosition.y, LowerBound.transform.localPosition.y, Padding);

        }
        
    }

    private Vector3 GetUpgradePosition(int index, int total, float upperEdge, float lowerEdge, float padding)
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