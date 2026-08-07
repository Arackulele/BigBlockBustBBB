using System;
using TMPro;
using UnityEngine;

public class ScorePoint : MonoBehaviour
{
    public int Amount = 1;

    private Vector2 originalPos;
    private Vector2 goalPos;

    private float LifeTime = -0.24f;

    void Start()
    {
        


        LifeTime += UnityEngine.Random.Range(0.01f, 0.06f);
        transform.GetComponent<TextMeshPro>().text = "+" + Amount;
        transform.localScale *= Math.Min(1 + (Amount * 0.05f), 4);
        goalPos = ScoreManagement.Instance.transform.position;
        originalPos = transform.position;

        transform.GetComponent<TextMeshPro>().colorGradient = ThemerScript.Instance.CurrentTheme.ScoreTextColor;
    }

    void Update()
    {
        LifeTime += Time.deltaTime + Math.Max(0, LifeTime * 0.07f);
        if (LifeTime > 0) transform.position = Vector2.Lerp(originalPos, goalPos, LifeTime);

        if (LifeTime > 1)
        {
            ScoreManagement.Instance.Score += Amount;
            ScoreManagement.Instance.UnspentScore += Amount * (0.1 * ScoreManagement.Instance.MoneyMult);
            Destroy(gameObject);
        }
    }
}
