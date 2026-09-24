using System.Collections.Generic;
using UnityEngine;

public class TurnPassMoney : Upgrade
{

    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Postage Stamp";

    public override string description => "Gain 2 Money at the end of every Turn";

    public override void OnTurnPassed(int Turn)
    {
        ScoreManagement.Instance.UnspentScore += 2;
    }
}