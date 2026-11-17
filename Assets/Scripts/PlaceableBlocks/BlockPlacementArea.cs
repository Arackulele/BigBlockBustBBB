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

    public void ChangeDatabase(BlockDatabase database)
    {
        BlockDictionary.Load(database);
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

    public bool DiscardBlockAt(Vector2 position)
    {
        int closestIndex = GetBlockSlotAt(position);
        if (closestIndex < 0) return false;

        Transform block = BlockPlacementAreas[closestIndex].transform.GetChild(0);
        Destroy(block.gameObject);
        StartCoroutine(GetNextBlockSpawns());
        return true;
    }

    public bool HasBlockAt(Vector2 position)
    {
        return GetBlockSlotAt(position) >= 0;
    }

    private int GetBlockSlotAt(Vector2 position)
    {
        int closestIndex = -1;
        float closestDistance = float.PositiveInfinity;

        for (int i = 0; i < BlockPlacementAreas.Count; i++)
        {
            GameObject slot = BlockPlacementAreas[i];
            if (slot == null || slot.transform.childCount == 0) continue;

            Transform block = slot.transform.GetChild(0);
            var renderers = block.GetComponentsInChildren<Renderer>();
            foreach (Renderer blockRenderer in renderers)
            {
                // Dragged objects are raised above the board in Z. Compare only
                // the screen plane so the drag offset cannot prevent a hit.
                Bounds bounds = blockRenderer.bounds;
                if (position.x >= bounds.min.x && position.x <= bounds.max.x &&
                    position.y >= bounds.min.y && position.y <= bounds.max.y) return i;
            }

            float distance = ((Vector2)slot.transform.position - position).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        if (closestIndex < 0) return -1;
        Transform closestBlock = BlockPlacementAreas[closestIndex].transform.GetChild(0);
        foreach (Renderer blockRenderer in closestBlock.GetComponentsInChildren<Renderer>())
        {
            Bounds bounds = blockRenderer.bounds;
            if (position.x >= bounds.min.x && position.x <= bounds.max.x &&
                position.y >= bounds.min.y && position.y <= bounds.max.y) return closestIndex;
        }

        return -1;
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
