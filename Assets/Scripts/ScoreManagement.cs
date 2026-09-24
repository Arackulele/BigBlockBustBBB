using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManagement : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ScoreText;

    [SerializeField]
    private TextMeshProUGUI SpendingScoreText;

    [SerializeField]
    private TextMeshProUGUI CurrentMultPrefab;

    [SerializeField]
    private GameObject ScoreGiverPrefab;

    [SerializeField]
    private GameObject MultVisualPrefab;

    public static ScoreManagement Instance;

    public int Score;

    public double MoneyMult = 1;

    public double UnspentScore;

    public int Combo = 1;

    private void Update()
    {
        ScoreText.text = "" + Math.Round((double)Score, 1) + "/" + LevelProgression.instance.ScoreReq;
        SpendingScoreText.text = "" + Math.Round(UnspentScore, 2);
        CurrentMultPrefab.text = "" + Combo + "/" + Math.Round(GetMult(), 2);

    }

    private void Start()
    {
        Instance = this;
    }


    public double GetMult()
    {
        double mult = Combo * GameManager.Instance.GlobalMultiplier;
        foreach (GameModifier u in GameManager.Instance.GameModifiers())        {
            mult += u.GlobalPointsMultiplier();
        }
        return mult;
    }

    public void AddScore(int amount, Vector2 pos = new Vector2())
    {
        amount = (int)(amount * GetMult());
        PutScore(amount, pos);
    }
    
    public void AddRawScore(int amount, Vector2 pos = new Vector2())
    {
        PutScore(amount, pos);
    }

    public void ClearLine(int amount, List<Vector2Int> ClearedBlocks, Vector2 pos = new Vector2())
    {
        amount = (int)(amount * GetMult());
        foreach (GameModifier u in GameManager.Instance.GameModifiers())        {
            amount = (int)(amount * (u.LineClearModifier(ClearedBlocks) + 1));
        }
        
        IncrementMult(pos);
        GameBoard.instance.GettingMult = true;
        PutScoreAndMoney(amount, pos);

    }

    public void PlaceBlock(List<Vector2Int> PlacedPositions, UnplacedBlockScript Block, Vector2 pos = new Vector2())
    {
        int amount = 0;
        foreach (GameModifier u in GameManager.Instance.GameModifiers())        {
            Debug.Log($"Upgrade: {(u == null ? "NULL" : u.ToString())}");
            amount += u.BlockPlaceModifier(PlacedPositions, Block);
        }
        amount = (int)(amount * GetMult());
        if (amount > 0)
        {
            PutScore(amount, pos);
        }
    }

    public void IncrementMult(Vector2 pos = new Vector2())
    {
        GameObject g = Instantiate(MultVisualPrefab);
        g.transform.position = Vector2.Lerp(new Vector2(), pos, 0.5f);
        Combo++;
    }

    public void PutScore(int amount, Vector2 pos = new Vector2())
    {
        GameObject g = Instantiate(ScoreGiverPrefab);
        g.transform.position = pos;
        g.GetComponent<ScorePoint>().Amount = amount;
    }
    
    public void PutScoreAndMoney(int amount, Vector2 pos = new Vector2())
    {
        PutScore(amount, pos);
        ScoreManagement.Instance.UnspentScore += amount * (0.1 * ScoreManagement.Instance.MoneyMult);
    }

    public void DepleteMult()
    {
        foreach (GameModifier u in GameManager.Instance.GameModifiers())        {
            Debug.Log($"Upgrade: {(u == null ? "NULL" : u.ToString())}");
            u.OnMultReset(Combo);
        }
        Combo = 1;
    }

}
