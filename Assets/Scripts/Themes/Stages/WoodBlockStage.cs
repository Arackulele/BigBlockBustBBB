using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WoodBlockStage : Stage
{
    public override Rarity rarity { get => Rarity.Rare; }
    
    public override string name => "Wooden Blocks";

    public override string description => "A full 4x4 Area of Blocks is also cleared.";
    
    public override double pointsmod => 1.4;

    public override Theme Theme { get => AssetLoader.Instance.WoodenStage_Theme; }

    public override void AfterBlocksPlaced()
    {
        
    }

    public override List<Vector2Int> GetAdditionalClearableBlocks(bool[,] gridMap)
    {
        List<Vector2Int> clearableBlocks = new List<Vector2Int>();
        
        // Check for 4x4 areas
        for (int x = 0; x <= gridMap.GetLength(0) - 4; x++)
        {
            for (int y = 0; y <= gridMap.GetLength(1) - 4; y++)
            {
                bool isFull = true;
                for (int dx = 0; dx < 4 && isFull; dx++)
                {
                    for (int dy = 0; dy < 4 && isFull; dy++)
                    {
                        if (!gridMap[x + dx, y + dy])
                        {
                            isFull = false;
                        }
                    }
                }

                if (isFull)
                {
                    Debug.Log("Found 4x4 area at: " + x + ", " + y);
                    for (int dx = 0; dx < 4; dx++)
                    {
                        for (int dy = 0; dy < 4; dy++)
                        {
                            clearableBlocks.Add(new Vector2Int(x + dx, y + dy));
                        }
                    }
                }
            }
        }

        return clearableBlocks;
    }
}
