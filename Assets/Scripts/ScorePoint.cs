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
        transform.localScale *= Math.Min(1 + (Amount * 0.01f), 4);
        goalPos = ScoreManagement.Instance.transform.position;
        originalPos = transform.position;
        
        if (Amount > ScoreManagement.Instance.GetMult() + 1) {
            Camera.main.GetComponent<Screenshake>().shakeDuration += Math.Min(1f, Mathf.Sqrt(Amount) * 0.01f);
            Camera.main.GetComponent<Screenshake>().shakeAmount = Math.Min(1f, Mathf.Sqrt(Amount) * 0.013f + 0.08f );
        }

        //transform.GetComponent<TextMeshPro>().colorGradient = ThemerScript.Instance.CurrentTheme.ScoreTextColor;
    }

    void Update()
    {
        LifeTime += Time.deltaTime + Math.Max(0, LifeTime * 0.07f);
        if (LifeTime > 0) transform.position = Vector2.Lerp(originalPos, goalPos, LifeTime);

        if (LifeTime > 1)
        {
            ScoreManagement.Instance.Score += Amount;
            Destroy(gameObject);
        }
    }
}
