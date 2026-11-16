using System.Collections.Generic;
using UnityEngine;

public class SafetyNetScore : Upgrade
{

    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Safety Net";

    public override string description => "If you reach the last turn and haven't reached the target score, gain 10% of the score as points.";

    public override void OnTurnPassed(int turn)
    {
        if (turn == GameManager.Instance.TurnLimit - 1)
        {
            ScoreManagement.Instance.AddScore((int)(LevelProgression.instance.ScoreReq * 0.1f));
            
        }
    }
    
}
