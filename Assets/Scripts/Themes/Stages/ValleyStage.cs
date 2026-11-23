using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ValleyStage : Stage
{
    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Green Valley";

    public override string description() => "Every third turn, gain +0.5 mult.";

    public override double pointsmod => 1.125;

    public override Theme Theme { get => AssetLoader.Instance.ValleyStage_Theme; }

    public override float GlobalPointsMultiplier()
    {
        if (LevelProgression.instance.CurrentTurn % 3 == 0) return 0.5f;
        return 0;
    }
}
