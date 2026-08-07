using UnityEngine;

public class AssetLoader : MonoBehaviour
{

    public static AssetLoader Instance;

    public GameObject UpgradeTemplate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }


}
