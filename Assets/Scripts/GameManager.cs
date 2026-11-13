using System;
using System.Collections;
using System.Collections.Generic;
using EasyTransition;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public Stage CurrentStage = new DefaultStage();

    [SerializeField]
    private GameObject EndScreen;

    [SerializeField] 
    private TransitionSettings transitiontype;

    public List<Upgrade> Perks = new List<Upgrade>();
    
    public List<Consumable> Consumables = new List<Consumable>();

    
    [SerializeField, Min(0)] private int maxPerks = 4;
    public int MaxPerks => maxPerks;
    
    [SerializeField, Min(0)] private int maxConsumables = 10;
    public int MaxConsumables => maxConsumables;
    
    public double GlobalMultiplier = 1.0;

    public int TurnLimit = 10;


    public UpgradeInventory upgradeArea;
    public ConsumableInventory consumableArea;
    
    public ObjectSwitcher areaswitcher;

    private bool stageTransitionInProgress;
    




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        transitiontype.transitionSpeed = 1.8f;
        StartCoroutine(StartGame());
    }

    private void Start()
    {
    }

    public List<GameModifier> GameModifiers()
    {
        List<GameModifier> FinalList = new List<GameModifier>();
        FinalList.AddRange(Perks);
        if (CurrentStage != null) FinalList.Add(CurrentStage);
        return FinalList;
    }

    public bool HasModifier<T>() where T : GameModifier
    {
        //Thanks stackoverflow
        return Perks.Exists(perk => perk is T);
    }

    public IEnumerator StartGame()
    {

        yield return new WaitForSeconds(0.2f);
        ChangeStage(new DefaultStage());
        BlockPlacementArea.instance.CheckPlacements();

        //AddUpgrade(new AlwaysGainLow());
        //AddUpgrade(new GlobalLowMultBonus());
        //AddUpgrade(new MoneyMultBonus());
        //AddConsumable(new GainScoreConsumable());
        //AddConsumable(new BombConsumable());
        //AddConsumable(new FillConsumable());


        upgradeArea.UpdateArea();
        consumableArea?.UpdateArea();

    }

    public bool AddUpgrade(Upgrade upgrade)
    {
        if (!Perks.Contains(upgrade) &&  Perks.Count < MaxPerks)
        {
        Perks.Add(upgrade);
        if (ShopManager.instance.shopInventory.UpgradeIndex.Contains(upgrade)) ShopManager.instance.shopInventory.UpgradeIndex.Remove(upgrade);
        //ToDo: When an upgrade is added, schedule to update area at the end of the frame, instead of updating every time one is added
        upgradeArea.UpdateArea();
        upgrade.OnAdded();
        return true;
        }
        return false;
    }

    public bool AddConsumable(Consumable consumable)
    {
        if (Consumables.Count >= MaxConsumables)
            return false;

        Consumables.Add(consumable);
        consumableArea.UpdateArea();
        return true;
    }

    public void ChangeMaxPerks(int amount)
    {
        maxPerks = Mathf.Max(0, maxPerks + amount);

        bool removedPerks = false;
        while (Perks.Count > maxPerks)
        {
            int lastIndex = Perks.Count - 1;
            Upgrade removedPerk = Perks[lastIndex];
            Perks.RemoveAt(lastIndex);
            removedPerk.OnRemoved();
            removedPerks = true;
        }

        if (removedPerks)
            upgradeArea?.UpdateArea();
    }

    public void ChangeMaxConsumables(int amount)
    {
        maxConsumables = Mathf.Max(0, maxConsumables + amount);

        bool removedConsumables = false;
        while (Consumables.Count > maxConsumables)
        {
            Consumables.RemoveAt(Consumables.Count - 1);
            removedConsumables = true;
        }

        if (removedConsumables)
            consumableArea?.UpdateArea();
    }

    public bool IsDead()
    {
        foreach (GameObject block in BlockPlacementArea.instance.BlockPlacementAreas)
        {
            if (block.transform.childCount > 0)
            {
                UnplacedBlockScript up = block.transform.GetChild(0).GetComponent<UnplacedBlockScript>();

                foreach (GameObject space in GameBoard.instance.GridObjects)
                {
                    if (IsPlaceAbleAtPosition(up, space.GetComponent<BlockScript>().GridPos)) { return false; }
                }
            }
        }
        return true;
    }

    private bool IsPlaceAbleAtPosition(UnplacedBlockScript rf, Vector2Int attempted)
    {
        bool Possible = true;

        foreach (Vector2Int pos in rf.Positions)
        {
            if (attempted.x + pos.x < 0 || attempted.x + pos.x > GameBoard.instance.BoardSize().x - 1) Possible = false;
            else if (attempted.y + pos.y < 0 || attempted.y + pos.y > GameBoard.instance.BoardSize().y - 1 || GameBoard.instance.GridMap[attempted.x + pos.x, attempted.y + pos.y]) Possible = false;
            if (!Possible) return false;
        }
        return Possible;
    }

    public void EndRun()
    {
        EndScreen.SetActive(true);
    }
    
    public void WonStage()
    {
        if (stageTransitionInProgress) return;

        //ToDo: This would be nicer to keep in the upgrade but it'd take more code
        stageTransitionInProgress = true;
        TransitionToNextState(() =>
        {
            if (!HasModifier<KeepCombo>()) ScoreManagement.Instance.Combo = 1;
            LevelProgression.instance.CurrentTurn = 0;
            LevelProgression.instance.CurrentStage++;
            ShopManager.instance.GoToShop();
        });
    }
    
    
    
    public void Endturn()
    {
        foreach (GameModifier u in GameManager.Instance.GameModifiers())        {
            u.OnTurnPassed(LevelProgression.instance.CurrentTurn);
        }
        
        if (!GameBoard.instance.GettingMult) ScoreManagement.Instance.DepleteMult();
        else GameBoard.instance.GettingMult = false;
        BlockPlacementArea.instance.FillBlocks();
        LevelProgression.instance.CurrentTurn++;
    }

    public void StartNew()
    {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }
    
    public void ProgressLevel()
    {
        if (stageTransitionInProgress) return;

        stageTransitionInProgress = true;
        TransitionToNextState(() =>
        {
            ShopManager.instance.ExitShop();
            ChangeStage();
            BlockPlacementArea.instance.FillBlocks(true);
            ScoreManagement.Instance.Score = 0;
            int x = LevelProgression.instance.CurrentStage;
            LevelProgression.instance.ScoreReq = (float)((500 + 10 * (x + 2) * x) * CurrentStage.pointsmod);
            LevelProgression.instance.ScoreReq = Mathf.RoundToInt(LevelProgression.instance.ScoreReq);
            GameBoard.instance.EmptyBoard();
        });
    }

    private void TransitionToNextState(Action cutPointAction)
    {
        TransitionManager transitionManager = TransitionManager.Instance();
        if (transitionManager == null || transitiontype == null)
        {
            stageTransitionInProgress = false;
            return;
        }

        UnityEngine.Events.UnityAction onCutPoint = null;
        onCutPoint = () =>
        {
            transitionManager.onTransitionCutPointReached -= onCutPoint;
            cutPointAction();
        };
        transitionManager.onTransitionCutPointReached += onCutPoint;
        transitionManager.onTransitionEnd += ResetStageTransition;
        transitionManager.Transition(transitiontype, 0);
    }

    private void ResetStageTransition()
    {
        TransitionManager.Instance().onTransitionEnd -= ResetStageTransition;
        stageTransitionInProgress = false;
    }

    public void ChangeStage()
    {
        CurrentStage = LevelProgression.instance.SelectNewStage();
        ThemerScript.Instance.ChangeTheme(CurrentStage.Theme);
    }
    
    public void ChangeStage(Stage stage)
    {
        CurrentStage = stage;
        ThemerScript.Instance.ChangeTheme(CurrentStage.Theme);
    }
}
