using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OceanStage : Stage
{
    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Blue Ocean";

    public override string description => "Blocks placed gain double Points.";
    
    public override double pointsmod => 1.2;

    public override Theme Theme { get => AssetLoader.Instance.OceanStage_Theme; }

    public override int BlockPlaceModifier(List<Vector2Int> placedPositions, UnplacedBlockScript block)
    {
        return 1;
    }
}
