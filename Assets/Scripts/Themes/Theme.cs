using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public struct Theme
{

    public List<Sprite> BlockSprites;
    public List<Color>  BlockColors;

    public Material BGMaterial;
    public Color BordBackgroundColor;
    public Color UIBorderColor;


    public TMPro.VertexGradient ScoreTextColor;
    public Color TotalScoreTextColor;


}
