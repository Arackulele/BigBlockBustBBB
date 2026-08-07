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
        ScoreText.text = "" + Score;
        SpendingScoreText.text = "" + UnspentScore.ToString();
        CurrentMultPrefab.text = "" + Combo.ToString() + "/" + GetMult().ToString();

    }

    private void Start()
    {
        Instance = this;
    }

    public float GetMult()
    {
        float mult = Combo;
        foreach (Upgrade u in GameManager.Instance.Perks)
        {
            mult += u.GlobalPointsMultiplier();
        }
        return mult;
    }

    public void AddScore(int amount, Vector2 pos = new Vector2())
    {
        amount = (int)(amount * GetMult());
        PutScore(amount, pos);
    }

    public void ClearLine(int amount, List<Vector2Int> ClearedBlocks, Vector2 pos = new Vector2())
    {
        amount = (int)(amount * GetMult());
        foreach (Upgrade u in GameManager.Instance.Perks)
        {
            amount = (int)(amount * (u.LineClearModifier(ClearedBlocks) + 1));
        }
        PutScore(amount, pos);

    }

    public void PlaceBlock(List<Vector2Int> PlacedPositions, UnplacedBlockScript Block, Vector2 pos = new Vector2())
    {
        int amount = 0;
        foreach (Upgrade u in GameManager.Instance.Perks)
        {
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

    public void DepleteMult()
    {
        Combo = 1;
    }

}
