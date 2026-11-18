using System.Collections.Generic;
using UnityEngine;

public class ColumnClearBonus : Upgrade
{

    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Olive Branch";

    public override string description => "Clearing a column gains an additional 100 points";

    public override float LineClearModifier(List<Vector2Int> clearedBlocks, int clearCountRows, int clearCountCollumns, int clearCountMisc)
    {
        if (clearCountCollumns > 0)
        {
        OnActivate();
        ScoreManagement.Instance.AddScore(100 * clearCountCollumns);
        }
        return 0f;
    }
}
