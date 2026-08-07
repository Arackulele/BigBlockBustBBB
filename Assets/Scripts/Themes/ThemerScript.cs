using TMPro;
using UnityEngine;

public class ThemerScript : MonoBehaviour
{
    public static ThemerScript Instance;

    public Theme CurrentTheme;
    public SpriteRenderer BordBorder;
    public SpriteRenderer UpgradeArea;
    public SpriteRenderer Background;
    public SpriteRenderer UpgradeAreaBackground;

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
        Background.color = CurrentTheme.BackgroundColor;
        TotalScoreText.color = CurrentTheme.TotalScoreTextColor;

        BordBorder.color = CurrentTheme.BordBorderColor;

        UpgradeArea.color = CurrentTheme.BordBorderColor;
        UpgradeAreaBackground.color = CurrentTheme.BordBackgroundColor;


    }

}
