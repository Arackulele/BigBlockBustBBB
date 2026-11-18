using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StageReroller : MonoBehaviour
{
    public static StageReroller instance;

    [SerializeField] private TextMeshProUGUI rerollButtonText;

    private void Start()
    {
        instance = this;
        UpdateRerollButtonText();

    }
    
    public int RerollPrice = 0;

    public void Reroll()
    {
        if (ScoreManagement.Instance.SpendMoney(RerollPrice))
        {
            GameManager.Instance.CurrentStage = LevelProgression.instance.SelectNewStage(false);
            if (RerollPrice == 0) RerollPrice = 6;
            else RerollPrice *= 2;
            UpdateRerollButtonText();
        }
    }

    public void GoBack()
    {
        if (LevelProgression.instance.previousStage != null)
        {
            Stage previousStage = (Stage)Activator.CreateInstance(LevelProgression.instance.previousStage);
            GameManager.Instance.CurrentStage = previousStage;
            LevelProgression.instance.previousStage = null;
        }
    }

    public void UpdateRerollButtonText()
    {
        if (rerollButtonText != null)
            rerollButtonText.text = $"Reroll ({RerollPrice})";
    }
        
}
