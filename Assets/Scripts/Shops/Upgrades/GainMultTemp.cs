using System.Collections.Generic;
using UnityEngine;

public class GainMultTemp : Upgrade
{

    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Ice Cube";

    public override string description => "Gain 4 mult, this decreases by 0.2 mult after every stage.";

    private double Mult = 4;

    public override void OnStageCleared()
    {
        if (Mult > 0) Mult -= 0.2;
        if (Mult < 0) Mult = 0;
    }
    
    public override float GlobalPointsMultiplier()
    {
        return (float)Mult;
    }
}