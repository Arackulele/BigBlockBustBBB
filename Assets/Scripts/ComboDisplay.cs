using System;
using TMPro;
using UnityEngine;

public class ComboDisplay : MonoBehaviour
{

    //ToDo: This could be like an inheritance thing with the score pointer system
    public int Mult;

    private Vector2 originalPos;
    private Vector2 goalPos;

    private float LifeTime = -0.24f;

    void Start()
    {
        //Counter should start at x1 always, not x2
        Mult = ScoreManagement.Instance.Combo -1;
        LifeTime += UnityEngine.Random.Range(0.01f, 0.06f);
        transform.GetComponent<TextMeshPro>().text = "X" + Mult + " Combo";
        transform.localScale *= Math.Min(1.3f + (Mult * 0.1f), 4);
        goalPos = new Vector2(transform.position.x, transform.position.y + 2);
        originalPos = transform.position;
    }

    void Update()
    {
        LifeTime += Time.deltaTime + Math.Max(0, LifeTime * 0.07f);
        if (LifeTime > 0) transform.position = Vector2.Lerp(originalPos, goalPos, LifeTime);
        TextMeshPro TM = transform.GetComponent<TextMeshPro>();
        TM.color = new Color(TM.color.r, TM.color.g, TM.color.b, Mathf.Lerp(1.2f, 0, LifeTime));

        if (LifeTime > 1)
        {
            Destroy(gameObject);
        }
    }
}
