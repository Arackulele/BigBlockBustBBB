using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Unity.Collections.AllocatorManager;

public class GameBoard : MonoBehaviour
{

    public static GameBoard instance;


    public bool[,] GridMap = new bool[8, 8];
    public GameObject[,] GridObjects = new GameObject[8, 8];

    [SerializeField]
    private GameObject BoardSlot;
    [SerializeField]
    public float BoardScale;

    public bool GettingMult = false;

    void Awake()
    {
        instance = this;

        for (int i = 0; i < GridMap.GetLength(0); i++)
        {
            for (int c = 0; c < GridMap.GetLength(1); c++)
            {
                GameObject b = GameObject.Instantiate(BoardSlot, transform);

                float HalfScale = ( (GridMap.GetLength(0) - 1) * BoardScale ) * 0.5f;
                b.transform.position = new Vector3(i * BoardScale - HalfScale, c * BoardScale - HalfScale, 2);
                //Debug.Log("Generating Field at: " + i + ", " + c);
                GridObjects[i, c] = b;
                b.GetComponent<BlockScript>().GridPos = new Vector2Int(i, c);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2Int ClosestBlock(Vector2 position, bool OnlyEmpty = false)
    {
        GameObject closest = null;
        Vector2Int closestpos = new Vector2Int(-1, -1);
        for (int i = 0; i < GridMap.GetLength(0); i++)
        {
            for (int c = 0; c < GridMap.GetLength(1); c++)
            {
                GameObject g = GridObjects[i, c];
                if (closest == null || Vector2.Distance(position, g.transform.position) < Vector2.Distance(position, closest.transform.position))
                {
                    if (!OnlyEmpty || !GridMap[i, c]) {
                        closest = g;
                        closestpos = new Vector2Int(i, c);
                    }
                }
                Debug.Log("Closest pos is "  + closest.transform.position);
            }
        }

        return closestpos;
    }

    public GameObject ClosestBlockObj(Vector2 position)
    {
        Vector2Int temp = ClosestBlock(position);
        return GridObjects[temp.x, temp.y];
    }

    public GameObject GetBlockAtPosition(int x, int y)
    {
        return GridObjects[x, y];
    }
    
    public GameObject GetBlockAtPosition(Vector2Int position)
    {
        return GridObjects[position.x, position.y];
    }

    public Vector3 GetRealPosition(Vector2Int pos)
    {
        return GridObjects[pos.x, pos.y].transform.position;
    }

    public Vector2Int BoardSize()
    {
        return new Vector2Int(GridMap.GetLength(0), GridMap.GetLength(1));
    }

    public void SetBlockAtPos(int x, int y, Color c, Sprite s)
    {
        GridMap[x, y] = true;
        GridObjects[x, y].GetComponent<BlockScript>().Color = c;
        GridObjects[x, y].GetComponent<BlockScript>().Sprite = s;
        GridObjects[x, y].GetComponent<BlockScript>().Fill();
    }

    public void SetBlockSimple(Vector2Int pos)
    {
        Color c = ThemerScript.Instance.CurrentTheme.BlockColors.GetRandomItem();
        Sprite s = ThemerScript.Instance.CurrentTheme.BlockSprites.GetRandomItem();
        GameBoard.instance.SetBlockAtPos(pos.x, pos.y, c, s);
    }
    
    public void ClearBlockAtPos(int x, int y)
    {
        GridMap[x, y] = false;
        GridObjects[x, y].GetComponent<BlockScript>().Clear();
    }
    
    public void ClearBlockAtPos(Vector2Int pos)
    {
        ClearBlockAtPos(pos.x, pos.y);
    }

    public void ClearBoard()
    {
        bool[,] gridBeforeClear = (bool[,])GridMap.Clone();
        List<Vector2Int> clearableBlocks = GetClearableBlocks(gridBeforeClear, out int clearedRows);
        if (clearedRows > 0)
        {
            foreach (Vector2Int bl in clearableBlocks)
            {
            ClearBlockAtPos(bl.x, bl.y);
            }

            ScoreManagement.Instance.ClearLine(20 * (clearedRows * clearedRows), clearableBlocks);

        }

        foreach (GameModifier u in GameManager.Instance.GameModifiers())  {
            u.AfterBlocksPlaced();
        }
    }

    public List<Vector2Int> GetClearableBlocks(bool[,] gridMap, out int clearCount)
    {
        clearCount = 0;
        var clearableBlocks = new List<Vector2Int>();

        for (int y = 0; y < gridMap.GetLength(1); y++)
        {
            bool full = true;
            for (int x = 0; x < gridMap.GetLength(0); x++)
                if (!gridMap[x, y]) { full = false; break; }
            if (!full) continue;

            clearCount++;
            for (int x = 0; x < gridMap.GetLength(0); x++)
                AddUnique(clearableBlocks, new Vector2Int(x, y));
        }

        for (int x = 0; x < gridMap.GetLength(0); x++)
        {
            bool full = true;
            for (int y = 0; y < gridMap.GetLength(1); y++)
                if (!gridMap[x, y]) { full = false; break; }
            if (!full) continue;

            clearCount++;
            for (int y = 0; y < gridMap.GetLength(1); y++)
                AddUnique(clearableBlocks, new Vector2Int(x, y));
        }

        foreach (GameModifier modifier in GameManager.Instance.GameModifiers())
        {
            List<Vector2Int> additionalBlocks = modifier.GetAdditionalClearableBlocks(gridMap);
            foreach (Vector2Int pos in additionalBlocks)
                AddUnique(clearableBlocks, pos);
            if (additionalBlocks.Count > 1) clearCount++;
        }

        return clearableBlocks;
    }

    private static void AddUnique(List<Vector2Int> positions, Vector2Int position)
    {
        if (!positions.Contains(position)) positions.Add(position);
    }
    
    public void EmptyBoard()
    {
        for (int i = 0; i < GridMap.GetLength(0); i++)
        {
            for (int c = 0; c < GridMap.GetLength(1); c++)
            {
                ClearBlockAtPos(i, c);
            }
        }
    }

    public bool IsValidPosition(Vector2Int pos)
    {
        if (pos.x >= 0 && pos.x < GridMap.GetLength(0) &&
            pos.y >= 0 && pos.y < GridMap.GetLength(1) ) return true;
        else return false;
    }

    public Vector2Int GetRandomPosition(bool onlyempty = false)
    {
        if (!onlyempty) return new Vector2Int(Random.Range(0, GridMap.GetLength(0)), Random.Range(0, GridMap.GetLength(1)));

        List<Vector2Int> emptyPositions = new List<Vector2Int>();
        for (int x = 0; x < GridMap.GetLength(0); x++)
        {
            for (int y = 0; y < GridMap.GetLength(1); y++)
            {
                if (!GridMap[x, y])
                    emptyPositions.Add(new Vector2Int(x, y));
            }
        }

        if (emptyPositions.Count == 0)
            return new Vector2Int(-1, -1);

        return emptyPositions.GetRandomItem();
    }

}
