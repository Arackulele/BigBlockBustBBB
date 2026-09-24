using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.AllocatorManager;

public class BlockPlacementArea : MonoBehaviour
{
    [SerializeField]
    private BlockDatabase Database;

    [SerializeField]
    private GameObject PlaceBlockPrefab;

    [SerializeField]
    public List<GameObject> BlockPlacementAreas;
    
    public static BlockPlacementArea instance;

    void Start()
    {
        instance = this;
        BlockDictionary.Load(Database);
    }
    
    public void FillBlocks(bool refill = false)
    {
        for (int i = 0; i < BlockPlacementAreas.Count; i++)
        {
            if (BlockPlacementAreas[i].transform.childCount < 1 || refill)
            {
                FillBlock(i);
            }
        }

        foreach (GameModifier u in GameManager.Instance.GameModifiers())        {
            u.OnBlocksRefilled();
        }
    }

    public void FillBlock(int index)
    {
        GameObject block = BlockPlacementAreas[index];
        if (block.transform.childCount > 0) Destroy(block.transform.GetChild(0).gameObject);
            
                GameObject newblock = Instantiate(PlaceBlockPrefab, block.transform);
                UnplacedBlockScript newBlockScript = newblock.GetComponent<UnplacedBlockScript>();

                List<Vector2Int> shape = new List<Vector2Int>(
                    BlockDictionary.BlockShapes.GetRandomItem());

                RotateShapeRandom(shape);

                newBlockScript.Positions = shape;
            
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
            GameManager.Instance.Endturn();
        }
    }

    public System.Collections.IEnumerator GetNextBlockSpawns()
    {
        yield return new WaitForEndOfFrame();
        BlockPlacementArea.instance.CheckPlacements();
        if (GameManager.Instance.IsDead()) { GameManager.Instance.EndRun(); }
    }
}