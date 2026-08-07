using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeInventory : MonoBehaviour
{
    public List<GameObject> UpgradeVisuals = new();

    [SerializeField] private GameObject UpperBound;
    [SerializeField] private GameObject LowerBound;

    [SerializeField] private float Padding = 0.3f;

    public static UpgradeInventory Instance;

    private void Start()
    {
        Instance = this;
    }

    public void UpdateArea()
    {
        foreach (GameObject obj in UpgradeVisuals)
        {
            if (obj != null)
                Destroy(obj);
        }

        UpgradeVisuals.Clear();

        float top = UpperBound.transform.localPosition.y;
        float bottom = LowerBound.transform.localPosition.y;

        int count = GameManager.Instance.Perks.Count;

        for (int i = 0; i < count; i++)
        {
            Upgrade upgrade = GameManager.Instance.Perks[i];

            GameObject visual = upgrade.createVisual(transform);
            UpgradeVisuals.Add(visual);

            visual.transform.localPosition =
                GetUpgradePosition(i, count, top, bottom, Padding);

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