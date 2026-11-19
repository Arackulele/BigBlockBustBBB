using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Stage : GameModifier
{

    public virtual Theme Theme { get; }
    
   
    public virtual Rarity rarity { get; }
    
    public virtual double pointsmod { get; }

    public virtual int ScoreReq()
    {
        int x = LevelProgression.instance.CurrentStage;
        double req = (float)((500 + 10 * (x + 2) * x) * GameManager.Instance.CurrentStage.pointsmod);
        req = Mathf.RoundToInt(LevelProgression.instance.ScoreReq);
        return (int)req;
    }

}
