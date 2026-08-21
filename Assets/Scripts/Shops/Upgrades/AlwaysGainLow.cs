using System.Collections.Generic;
using UnityEngine;

public class AlwaysGainLow : Upgrade
{

    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Tiny Toy Soldier";

    public override string description => "Always gain a Block with 3 or less squares";

    public override void OnBlocksRefilled()
    {
        bool check = true;

        while (check)
        {
            UnplacedBlockScript up = BlockPlacementArea.Instance.BlockPlacementAreas[0].transform.GetChild(0).GetComponent<UnplacedBlockScript>();

            if (up.Positions.Count > 3)
            {
                BlockPlacementArea.Instance.FillBlock(0);
            }
            else check = false;
        }
    }
}
