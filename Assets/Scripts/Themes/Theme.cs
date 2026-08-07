using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public struct Theme
{

    public List<Sprite> BlockSprites;
    public List<Color>  BlockColors;

    public Color BackgroundColor;
    public Color BordBackgroundColor;
    public Color BordBorderColor;

    public TMPro.VertexGradient ScoreTextColor;
    public Color TotalScoreTextColor;


}
