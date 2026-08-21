using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField]
    private GameObject EndScreen;

    public List<Upgrade> Perks = new List<Upgrade>();
    
    public int MaxPerks = 4;

    public UpgradeInventory upgradeArea;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        StartCoroutine(StartGame());
    }

    public IEnumerator StartGame()
    {

        yield return new WaitForSeconds(0.2f);
        BlockPlacementArea.Instance.CheckPlacements();

        AddUpgrade(new AlwaysGainLow());
        //AddUpgrade(new GlobalLowMultBonus());
        //AddUpgrade(new MoneyMultBonus());

        upgradeArea.UpdateArea();

    }

    public bool AddUpgrade(Upgrade upgrade)
    {
        if (!Perks.Contains(upgrade) &&  Perks.Count < MaxPerks)
        {
        Perks.Add(upgrade);
        //ToDo: When an upgrade is added, schedule to update area at the end of the frame, insttead of updating every time one is added
        upgradeArea.UpdateArea();
        return true;
        }
        return false;
    }

    public bool IsDead()
    {
        foreach (GameObject block in BlockPlacementArea.Instance.BlockPlacementAreas)
        {
            if (block.transform.childCount > 0)
            {
                UnplacedBlockScript up = block.transform.GetChild(0).GetComponent<UnplacedBlockScript>();

                foreach (GameObject space in GameBoard.instance.GridObjects)
                {
                    if (IsPlaceAbleAtPosition(up, space.GetComponent<BlockScript>().GridPos)) { return false; }
                }
            }
        }
        return true;
    }

    private bool IsPlaceAbleAtPosition(UnplacedBlockScript rf, Vector2Int attempted)
    {
        bool Possible = true;

        foreach (Vector2Int pos in rf.Positions)
        {
            if (attempted.x + pos.x < 0 || attempted.x + pos.x > GameBoard.instance.BoardSize().x - 1) Possible = false;
            else if (attempted.y + pos.y < 0 || attempted.y + pos.y > GameBoard.instance.BoardSize().y - 1 || GameBoard.instance.GridMap[attempted.x + pos.x, attempted.y + pos.y]) Possible = false;
            if (!Possible) return false;
        }
        return Possible;
    }

    public void EndRun()
    {
        EndScreen.SetActive(true);
    }

    public void StartNew()
    {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }
    
    public void ProgressLevel()
    {
        ShopManager.instance.ExitShop();
        ScoreManagement.Instance.Score = 0;
        LevelProgressSlider.instance.ScoreReq *= 1.2f;
        GameBoard.instance.EmptyBoard();
    }
}
