using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MountainStage : Stage
{
    public override Rarity rarity { get => Rarity.Rare; }
    
    public override string name => "Rocky Mountains";

    public override string description => "A cross in the middle is filled with unclearable blocks.";
    
    public override double pointsmod => 1.125;

    public override Theme Theme { get => AssetLoader.Instance.MountainStage_Theme; }
    
    private List<Vector2Int> points = new List<Vector2Int>()
    {
        new Vector2Int(3, 3),
        new Vector2Int(3, 4),
        new Vector2Int(4, 4),
        new Vector2Int(4, 3),
        
        
        new Vector2Int(3, 2),
        new Vector2Int(2, 3),
        
        new Vector2Int(3, 5),
        new Vector2Int(5, 3),
        
        new Vector2Int(4, 2),
        new Vector2Int(2, 4),
        
        new Vector2Int(4, 5),
        new Vector2Int(5, 4),

        
    };

    public override void OnAdded()
    {
        foreach (Vector2Int point in points)
        {
            GameBoard.instance.SetBlockUnclearable(point);
        }
        
    }
}
