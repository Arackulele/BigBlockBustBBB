using UnityEngine;

public class AssetLoader : MonoBehaviour
{

    public static AssetLoader Instance;

    public GameObject UpgradeTemplate;
    
    public GameObject ConsumableTemplate;
    
    
    
    //Themes
    public Theme Shop_Theme;

    
    public Theme DefaultStage_Theme;
    public Theme SandStage_Theme;
    public Theme HillsStage_Theme;
    public Theme ValleyStage_Theme;
    public Theme OceanStage_Theme;
    public Theme GardensStage_Theme;
    public Theme ForestStage_Theme;
    
    public Theme WoodenStage_Theme;
    public Theme OreStage_Theme;
    public Theme MossStage_Theme;
    public Theme MountainStage_Theme;
    
    public BlockDatabase DefaultBlockPalette;
    public BlockDatabase TetrominoBlockPalette;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
    }

}
