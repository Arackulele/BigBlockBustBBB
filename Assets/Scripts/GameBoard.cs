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

    public Vector2Int ClosestBlock(Vector2 position)
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
                closest = g;
                closestpos = new Vector2Int(i, c);
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

    public Vector3 GetRealPosition(Vector2Int pos)
    {
        return GridObjects[pos.x, pos.y].transform.position;
    }

    public Vector2Int BoardSize()
    {
        return new Vector2Int(GridMap.GetLength(0), GridMap.GetLength(1));
    }

    public void SetBlockAtPos(int x, int y, Color c)
    {
        GridMap[x, y] = true;
        GridObjects[x, y].GetComponent<BlockScript>().Color = c;
        GridObjects[x, y].GetComponent<BlockScript>().Fill();
    }
    public void ClearBlockAtPos(int x, int y)
    {
        GridMap[x, y] = false;
        GridObjects[x, y].GetComponent<BlockScript>().Clear();
    }

    public void ClearBoard()
    {
        int clearedRows = 0;
        Vector2 ComboPosition = new Vector2();
        List<Vector2Int> ClearableBlocks = new List<Vector2Int>();
        for (int i = 0; i < GridMap.GetLength(0); i++)
        {
            bool foundGap = false;
            List<Vector2Int> BlocksInRow = new List<Vector2Int>();
            for (int c = 0; c < GridMap.GetLength(1); c++)
            {
                if (GridMap[i, c] == false) foundGap = true;
                BlocksInRow.Add(new Vector2Int(i, c));
            }
            if (!foundGap)
            {
                clearedRows++;
                Debug.Log("Attempting to clear Row");
                foreach (Vector2Int bl in BlocksInRow)
                {
                    ClearableBlocks.Add(bl);
                }
                ComboPosition = GetRealPosition(BlocksInRow.GetRandomItem());
            }
        }

        //clear Rows
        for (int i = 0; i < GridMap.GetLength(1); i++)
        {
            bool foundGap = false;
            List<Vector2Int> BlocksInRow = new List<Vector2Int>();
            for (int c = 0; c < GridMap.GetLength(0); c++)
            {
                if (GridMap[c, i] == false) foundGap = true;
                BlocksInRow.Add(new Vector2Int(c, i));
            }
            if (!foundGap)
            {
                clearedRows++;
                Debug.Log("Attempting to clear Row");
                foreach (Vector2Int bl in BlocksInRow)
                {
                    ClearableBlocks.Add(bl);
                }
            }
            if (Random.Range(0, 10) > 4) ComboPosition = GetRealPosition(BlocksInRow.GetRandomItem());
        }

        if (clearedRows > 0)
        {
            foreach (Vector2Int bl in ClearableBlocks)
            { 
            ClearBlockAtPos(bl.x, bl.y);
            }

            ScoreManagement.Instance.ClearLine(20 * (clearedRows * clearedRows), ClearableBlocks);
            ScoreManagement.Instance.IncrementMult(ComboPosition);
            GettingMult = true;
        }
    }
    
    public void EmptyBoard()
    {
        foreach (GameObject g in GridObjects)
        {
            g.GetComponent<BlockScript>().Clear();
        }
    }

}