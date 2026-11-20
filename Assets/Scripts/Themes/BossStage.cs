using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class BossStage : Stage
{
    public virtual int AttackCooldown() => 1;
    
    public virtual void Attack() {}

    public virtual bool ScoreCondition() => true;

    public virtual string ScoreConditionDesc() => "";
    
    public virtual string AttackDesc() => "";



    public override Rarity rarity => Rarity.Common;



    private int CurrentCooldown;

    public override int ScoreReq()
    {
        int x = LevelProgression.instance.CurrentLayer;
        int y = 0;
        switch (x)
        {
            default: case 1:
                y = 1000;
                break;
            case 2:
                y = 3000;
                break;
            case 3:
                y = 11000;
                break;
        }
        
        double req = (1000 + y) * pointsmod;
        return Mathf.RoundToInt((float)req);
    }

    public override void OnAdded()
    {
        CurrentCooldown = AttackCooldown();
    }

    public override void OnTurnPassed(int turn)
    {
        CurrentCooldown--;

        if (AttackCooldown() <= 0)
        {
            Attack();
            CurrentCooldown = AttackCooldown();
        }
    }

    public override int ScoreGained(int amount)
    {
        if (ScoreCondition()) return amount;
        else return 0;
    }

    public override String description()
    {
        String compound = ScoreConditionDesc();

        //this is way more code than is necessary to accomplish this, ill just use rich text formatting next time
        compound += " " + AttackDesc();
        compound += " Attacks every ";
        if (AttackCooldown() == 1) compound += " turn";
        else
        {
            compound += "" + AttackCooldown() +  " turns.";
        }

        return compound;
    }
}
