using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DefaultStage : Stage
{
    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Blockland";

    public override string description => "No modifiers.";

    public override double pointsmod => 1;


    public override Theme Theme { get => AssetLoader.Instance.DefaultStage_Theme; }
}
