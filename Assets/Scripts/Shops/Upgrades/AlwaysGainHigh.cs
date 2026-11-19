using System.Collections.Generic;
using UnityEngine;

public class AlwaysGainHigh : Upgrade
{

    public override Rarity rarity { get => Rarity.Rare; }
    
    public override string name => "Toy Monster Truck";

    public override string description => "Always gain a Block with 6 or more squares";

    public override void OnBlocksRefilled()
    {
        BlockPlacementArea placementArea = BlockPlacementArea.instance;
        //if (placementArea == null || placementArea.BlockPlacementAreas == null ||
        //    placementArea.BlockPlacementAreas.Count == 0)
        //    return;

        GameObject blockSlot = placementArea.BlockPlacementAreas[0];
        //if (blockSlot == null || blockSlot.transform.childCount == 0)
        //    return;

        UnplacedBlockScript block = blockSlot.transform.GetChild(0).GetComponent<UnplacedBlockScript>();
        if (block == null)
            return;

        List<List<Vector2Int>> lowShapes = new List<List<Vector2Int>>();
        foreach (List<Vector2Int> shape in BlockDictionary.BlockShapes)
        {
            if (shape != null && shape.Count >= 6)
                lowShapes.Add(shape);
        }

        if (lowShapes.Count == 0)
            return;

        OnActivate();
        block.Positions = new List<Vector2Int>(lowShapes.GetRandomItem());
    }
}
