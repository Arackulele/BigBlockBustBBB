using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressSlider : MonoBehaviour
{

    [SerializeField]
    private Scrollbar visual;

    public float ScoreReq = 1000;

    private void Update()
    {
        visual.size = Math.Min(ScoreManagement.Instance.Score / ScoreReq, 1f);

        if (ScoreReq <= ScoreManagement.Instance.Score && !ShopManager.instance.active) ShopManager.instance.GoToShop(); 
    }

}
