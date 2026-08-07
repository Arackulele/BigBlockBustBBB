using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockPlacementArea : MonoBehaviour
{
    [SerializeField]
    private BlockDatabase Database;

    [SerializeField]
    private GameObject PlaceBlockPrefab;

    [SerializeField]
    public List<GameObject> BlockPlacementAreas;

    public static BlockPlacementArea Instance;

    void Start()
    {
        Instance = this;
        BlockDictionary.Load(Database);
    }



    public void FillBlocks()
    {
        foreach (GameObject block in BlockPlacementAreas)
        {
            if (block.transform.childCount < 1)
            {
                GameObject newblock = Instantiate(PlaceBlockPrefab, block.transform);
                UnplacedBlockScript newBlockScript = newblock.GetComponent<UnplacedBlockScript>();

                List<Vector2Int> shape = new List<Vector2Int>(
                    BlockDictionary.BlockShapes.GetRandomItem());

                RotateShapeRandom(shape);

                newBlockScript.Positions = shape;
            }
        }
    }

    private void RotateShapeRandom(List<Vector2Int> shape)
    {
        int rotations = Random.Range(0, 4);

        for (int r = 0; r < rotations; r++)
        {
            for (int i = 0; i < shape.Count; i++)
            {
                Vector2Int p = shape[i];
                shape[i] = new Vector2Int(p.y, -p.x);
            }
        }
    }

    public void CheckPlacements()
    {
        // TODO: Call this only after a block has been placed.
        bool HaveToRefill = true;

        foreach (GameObject block in BlockPlacementAreas)
        {
            if (block.transform.childCount > 0)
            {
                HaveToRefill = false;
                break;
            }
        }

        if (HaveToRefill)
        {
            if (!GameBoard.instance.GettingMult) ScoreManagement.Instance.DepleteMult();
            else GameBoard.instance.GettingMult = false;
            FillBlocks();
        }
    }

    public System.Collections.IEnumerator GetNextBlockSpawns()
    {
        yield return new WaitForEndOfFrame();
        BlockPlacementArea.Instance.CheckPlacements();
        if (GameManager.Instance.IsDead()) { GameManager.Instance.EndRun(); }
    }
}