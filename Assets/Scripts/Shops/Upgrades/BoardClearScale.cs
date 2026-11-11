using System.Collections.Generic;
using UnityEngine;

public class BoardClearScale : Upgrade
{

    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Sweeping Broom";

    public override string description => "Gain 500 points when clearing a full board, this goes up by 500 every time the bonus is obtained.";

    private int Score = 500;

    public override void AfterBlocksPlaced()
    {
        foreach (var b in GameBoard.instance.GridMap)
        {
            if (b) return;
        }
        ScoreManagement.Instance.AddRawScore(Score);
        Score += 500;
    }
    
}