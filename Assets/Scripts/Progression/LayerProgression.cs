using System;
using System.Collections.Generic;
using UnityEngine;

public class LayerProgression : MonoBehaviour
{
    public static LayerProgression Instance;

    public List<EventType> CurrentLayer;

    private int CurrentPosition = 0;
    
    

    private void Start()
    {
        Instance = this;
        GenerateNewLayer(0);
    }

    public void GenerateNewLayer(int Depth)
    {
        int counter = 1;
        int max = 12;

        while (counter <= max)
        {
           //this is probably cleaner as a switch statement, actually maybe not i think the if else looks better
           if (counter == max - 1) CurrentLayer.Add(EventType.BossStage);
           else if (counter == max) CurrentLayer.Add(EventType.BonusStage);
           else if (counter == 2) CurrentLayer.Add(EventType.Shop);
           //this will be random event in the future
           else if (counter % 2 == 0) CurrentLayer.Add(EventType.Shop);
           else CurrentLayer.Add(EventType.RegularStage);
            counter++;
        }


    }

    public void NextEvent()
    {
        if (CurrentLayer.Count < CurrentPosition + 1)
        {

            CurrentPosition = 0;
            GenerateNewLayer(0);
        }
        
        SwitchEvent(CurrentLayer[CurrentPosition], CurrentLayer[CurrentPosition+1]);
        
    }

    private void SwitchEvent(EventType from, EventType to)
    {
        switch (from)
        { 
            case EventType.RegularStage:
                if (!GameManager.Instance.HasModifier<KeepCombo>()) ScoreManagement.Instance.Combo = 1;
                LevelProgression.instance.CurrentTurn = 0;
                LevelProgression.instance.CurrentStage++;
                GameManager.Instance.CurrentStage = LevelProgression.instance.SelectNewStage();
                ShopManager.instance.GoToShop();
                break;
            case  EventType.Shop:
                ShopManager.instance.ExitShop();
                break;
        }

        switch (to)
        {
            case EventType.RegularStage:
                ShopManager.instance.ExitShop();
                BlockPlacementArea.instance.FillBlocks(true);
                ScoreManagement.Instance.Score = 0;
                LevelProgression.instance.ScoreReq = GameManager.Instance.CurrentStage.ScoreReq();
                GameBoard.instance.EmptyBoard();
            
                //Since a stage should be added when the round starts, and isnt added through the normal upgrade process, we go through the trigger here
                GameManager.Instance.CurrentStage.OnAdded();
                break;
            case EventType.Shop :
                ShopManager.instance.GoToShop();
                break;
        }

        CurrentPosition++;
    }
    
}

public enum EventType
{
    Shop,
    
    RegularStage,
    BossStage,
    
    BonusStage
}