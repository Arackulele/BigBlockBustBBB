using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RowClearingBoss : BossStage
{
    public override string name => "Row-dy Ronny ";

    public override string ScoreConditionDesc() => "Ronny only takes damage from Row clears.";

    public override string AttackDesc() => "A random column on the board becomes unbreakable.";

    public override double pointsmod => 1;
    
    public override Theme Theme { get => AssetLoader.Instance.DefaultStage_Theme; }

    public override bool ScoreCondition()
    {
        return ScoreManagement.Instance != null
            && ScoreManagement.Instance.CurrentScoreGainType == ScoreGainType.LineClear
            && ScoreManagement.Instance.CurrentClearCountRows > 0;
    }
}
