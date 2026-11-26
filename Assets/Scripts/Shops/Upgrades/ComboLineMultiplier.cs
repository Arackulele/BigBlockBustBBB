using System.Collections.Generic;
using UnityEngine;

public class ComboLineMultiplier : Upgrade
{

    public override Rarity rarity { get => Rarity.Epic; }
    
    public override string name => "Bottle Rocket";

    public override string description() => "You gain combo equal to the amount of lines cleared.";

    public override float LineClearModifier(List<Vector2Int> clearedBlocks, int clearCountRows, int clearCountCollumns, int clearCountMisc)
    {
        ScoreManagement.Instance.Combo += (clearCountMisc + clearCountCollumns + clearCountRows) - 1;
        return 0f;
    }
}
