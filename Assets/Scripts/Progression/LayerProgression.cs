using System;
using System.Collections.Generic;
using UnityEngine;

public class LayerProgression : MonoBehaviour
{
    public static LayerProgression Instance;

    public List<EventType> CurrentLayer;

    private void Start()
    {
        Instance = this;
    }

    public void GenerateNewLayer(int Depth)
    {
        int counter = 1;
        int max = 8;

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
    
    
    
}

public enum EventType
{
    Shop,
    
    RegularStage,
    BossStage,
    
    BonusStage
}