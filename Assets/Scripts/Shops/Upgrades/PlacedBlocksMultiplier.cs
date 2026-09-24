using System.Collections.Generic;
using UnityEngine;

public class PlacedBlocksMultiplier : Upgrade
{

    public override Rarity rarity { get => Rarity.Rare; }
    
    public override string name => "Suitcase";

    public override string description => "Each Block already placed on the board adds 5% to the multiplier";

    public override float GlobalPointsMultiplier()
    {
        int total = 0;
        foreach (bool b in GameBoard.instance.GridMap)
        {
            if (b) total++;
        }
        return 0.05f * total;
    }
}
