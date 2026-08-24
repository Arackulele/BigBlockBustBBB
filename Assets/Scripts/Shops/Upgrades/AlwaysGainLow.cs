using System.Collections.Generic;
using UnityEngine;

public class AlwaysGainLow : Upgrade
{

    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Tiny Toy Soldier";

    public override string description => "Always gain a Block with 3 or less squares";

    public override void OnBlocksRefilled()
    {
        BlockPlacementArea placementArea = BlockPlacementArea.Instance;
        if (placementArea == null || placementArea.BlockPlacementAreas == null ||
            placementArea.BlockPlacementAreas.Count == 0)
            return;

        GameObject blockSlot = placementArea.BlockPlacementAreas[0];
        if (blockSlot == null || blockSlot.transform.childCount == 0)
            return;

        UnplacedBlockScript block = blockSlot.transform.GetChild(0).GetComponent<UnplacedBlockScript>();
        if (block == null)
            return;

        List<List<Vector2Int>> lowShapes = new List<List<Vector2Int>>();
        foreach (List<Vector2Int> shape in BlockDictionary.BlockShapes)
        {
            if (shape != null && shape.Count <= 3)
                lowShapes.Add(shape);
        }

        if (lowShapes.Count == 0)
            return;

        block.Positions = new List<Vector2Int>(lowShapes[Random.Range(0, lowShapes.Count)]);
    }
}
