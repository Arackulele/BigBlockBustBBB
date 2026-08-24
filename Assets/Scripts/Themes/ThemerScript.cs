using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThemerScript : MonoBehaviour
{
    public static ThemerScript Instance;

    public Theme CurrentTheme;
    public List<SpriteRenderer> UIBorder = new List<SpriteRenderer>();
    public List<SpriteRenderer> Background = new List<SpriteRenderer>();
    public List<SpriteRenderer> UIBackground = new List<SpriteRenderer>();
    public List<SpriteRenderer> UIPanel = new List<SpriteRenderer>();


    public TextMeshProUGUI TotalScoreText;


    private void Start()
    {
        Instance = this;
        //We want to set the current theme once as a start so all the default colors dont have to be manually set in the editor
        ChangeTheme(CurrentTheme);
    }

    public void ChangeTheme(Theme theme)
    {
        CurrentTheme = theme;

        foreach (GameObject t in GameBoard.instance.GridObjects)
        {
             t.GetComponent<SpriteRenderer>().color = CurrentTheme.BordBackgroundColor;
        }
        
        ChangeSegment(Background, CurrentTheme.BackgroundColor);
        TotalScoreText.color = CurrentTheme.TotalScoreTextColor;

        ChangeSegment(UIBorder, CurrentTheme.BordBorderColor);
        
        ChangeSegment(UIBackground, CurrentTheme.BordBackgroundColor);
        
        ChangeSegment(UIPanel, CurrentTheme.UIPanelColor);

        
    }

    private void ChangeSegment(List<SpriteRenderer> segments, Color color)
    {
        foreach (SpriteRenderer segment in segments)
        {
            segment.color = color;
        }
    }
    

}
