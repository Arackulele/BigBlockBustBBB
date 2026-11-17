using System.Collections.Generic;
using UnityEngine;

public class GainDiscard : Upgrade
{

    public override Rarity rarity { get => Rarity.Epic; }
    
    public override string name => "Herb Planter";

    public override string description => "Each turn, gain a mint twig if you aren't already holding one.";

    public override void OnTurnPassed(int turn)
    {
        if (!GameManager.Instance.HasConsumable<BlockDiscardConsumable>()) GameManager.Instance.AddConsumable(new BlockDiscardConsumable());
        OnActivate();
    }
}
