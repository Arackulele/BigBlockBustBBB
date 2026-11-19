using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUIScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ScoreText;

    [SerializeField]
    private TextMeshProUGUI SpendingScoreText;

    [SerializeField]
    private TextMeshProUGUI CurrentMultPrefab;


    private void Update()
    {
        ScoreText.text = "" + Math.Round((double)ScoreManagement.Instance.Score, 1) + "/" + LevelProgression.instance.ScoreReq;
        SpendingScoreText.text = "" + Math.Round(ScoreManagement.Instance.UnspentScore, 2);
        CurrentMultPrefab.text = "" + ScoreManagement.Instance.Combo + "/" + Math.Round(ScoreManagement.Instance.GetMult(), 2);

    }


}
