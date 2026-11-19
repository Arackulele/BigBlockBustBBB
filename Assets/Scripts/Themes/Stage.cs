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
        double req = (500 + 10 * (x + 2) * x) * pointsmod;
        return Mathf.RoundToInt((float)req);
    }

}
