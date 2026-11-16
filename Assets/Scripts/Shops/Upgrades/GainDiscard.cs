using System.Collections.Generic;
using UnityEngine;

public class GainDiscard : Upgrade
{

    public override Rarity rarity { get => Rarity.Epic; }
    
    public override string name => "Herb Planter";

    public override string description => "Each turn, gain a mint twig if you aren't already holding one.";

    public override float LineClearModifier(List<Vector2Int> ClearedBlocks)
    {
        if (ScoreManagement.Instance.Combo == 25) ScoreManagement.Instance.AddRawScore(5000);
        return 0;
    }
}
