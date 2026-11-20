using System.Collections.Generic;
using UnityEngine;

public class ColumnPointScale : Upgrade
{

    public override Rarity rarity { get => Rarity.Rare; }
    
    public override string name => "Olive Sapling";

    public override string description() => "Gain <color=blue>" + Score +
                                            "</color> score every turn, this increases by 2 for every column cleared.";

    private int Score = 2;

    public override void OnTurnPassed(int turn)
    {
        ScoreManagement.Instance.AddScore(Score);
        OnActivate();
    }

    public override float LineClearModifier(List<Vector2Int> clearedBlocks, int clearCountRows, int clearCountCollumns, int clearCountMisc)
    {
        if (clearCountCollumns > 0)
        {
            Score += 2 * clearCountCollumns;
            OnActivate();
        }
        return 0;
    }
}