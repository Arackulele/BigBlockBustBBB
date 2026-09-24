using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThemerScript : MonoBehaviour
{
    public static ThemerScript Instance;

    public Theme CurrentTheme;
    public List<SpriteRenderer> UIBorder = new List<SpriteRenderer>();
    public SpriteRenderer Background;
    public List<SpriteRenderer> UIBackground = new List<SpriteRenderer>();

    [SerializeField] private TextMeshProUGUI ThemeText;
    [SerializeField] private TextMeshProUGUI ThemeDesc;


    public TextMeshProUGUI TotalScoreText;


    private void Awake()
    {
        Instance = this;
        //We want to set the current theme once as a start so all the default colors dont have to be manually set in the editor
        //ChangeTheme(CurrentTheme);
    }

    public void ChangeTheme(Theme theme)
    {
        CurrentTheme = theme;

        foreach (GameObject t in GameBoard.instance.GridObjects)
        {
             t.GetComponent<SpriteRenderer>().color = CurrentTheme.BordBackgroundColor;
        }
        
        Background.material = CurrentTheme.BGMaterial;

        TotalScoreText.color = CurrentTheme.TotalScoreTextColor;
        
        ChangeSegment(UIBackground, CurrentTheme.BordBackgroundColor);
        
        ChangeSegment(UIBorder, CurrentTheme.UIBorderColor);

        ThemeText.text = GameManager.Instance.CurrentStage.name;
        ThemeDesc.text = GameManager.Instance.CurrentStage.description + " Required Points: x" + GameManager.Instance.CurrentStage.pointsmod;


    }

    private void ChangeSegment(List<SpriteRenderer> segments, Color color)
    {
        foreach (SpriteRenderer segment in segments)
        {
            segment.color = color;
        }
    }
    

}
