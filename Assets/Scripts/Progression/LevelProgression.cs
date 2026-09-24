using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgression : MonoBehaviour
{

    [SerializeField] private Scrollbar visual;

    [SerializeField] private TextMeshProUGUI TurnText;
    
    public static LevelProgression instance;

    [HideInInspector]
    public float ScoreReq = 500;
    
    [HideInInspector]
    public int CurrentStage = 1;
    
    [HideInInspector]
    public int CurrentTurn = 0;
    
    public List<Stage> StageIndex =
        new List<Stage>()
        {
            new DefaultStage(),
            new SandsStage(),
            new HillsStage(),
            new ValleyStage(),
            new OceanStage(),
            
            new WoodBlockStage()
        };

    private void Update()
    {
        visual.size = Math.Min(ScoreManagement.Instance.Score / ScoreReq, 1f);
        
        TurnText.text = "Turn " + CurrentTurn + "/" + GameManager.Instance.TurnLimit;

        if (ScoreReq <= ScoreManagement.Instance.Score && !ShopManager.instance.active) GameManager.Instance.WonStage();
        
        if (CurrentTurn > GameManager.Instance.TurnLimit) GameManager.Instance.EndRun();
    }

    private void Awake()
    {
        instance = this;
        
        foreach (Stage stage in StageIndex)
        {
            switch (stage.rarity)
            {
                case Rarity.Common: CommonStage.Add(stage); break;
                case Rarity.Rare: RareStage.Add(stage); break;
                case Rarity.Epic: EpicStage.Add(stage); break;
            }
        }
    }
    
    //ToDo: THis is also duplicate code from the shops, randomly selecting from rarity should just be its own helper tool for these types
    List<Stage> CommonStage = new List<Stage>();
    List<Stage> RareStage = new List<Stage>();
    List<Stage> EpicStage = new List<Stage>();

    public float RareChance = 30f;
    public float EpicChance = 3f;

    public Stage SelectNewStage()
    {
        Stage selected;

        if (UnityEngine.Random.Range(0f, 100f) <= RareChance) selected = RareStage.GetRandomItem();
        else if (UnityEngine.Random.Range(0f, 100f) <= EpicChance) selected = EpicStage.GetRandomItem();
        else selected = CommonStage.GetRandomItem();

        selected ??= StageIndex.GetRandomItem();

        //This is hacky and a little weird tbh
        return selected == null
            ? null
            : (Stage)Activator.CreateInstance(selected.GetType());
    }
    
    

}
