using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UnplacedBlockScript : MonoBehaviour
{
    public List<Vector2Int> Positions = new List<Vector2Int>();

    private bool IsBeingPlaced = false;

    [SerializeField]
    private GameObject BlockPrefab;

    private Vector3 originalPosition;
    private GameObject ghostClone;

    public Color color;

    private readonly List<BlockScript> highlightedBlocks = new();

    private void Start()
    {
        originalPosition = transform.position;
        color = ThemerScript.Instance.CurrentTheme.BlockColors.GetRandomItem();

        foreach (Vector2Int pos in Positions)
        {
            GameObject PosBlock = GameObject.Instantiate(BlockPrefab, transform);
            PosBlock.transform.localPosition = new Vector2(pos.x*GameBoard.instance.BoardScale, pos.y * GameBoard.instance.BoardScale);
            PosBlock.GetComponent<SpriteRenderer>().material.color = color;
        }

        ghostClone = GameObject.Instantiate(gameObject, transform);
        Destroy(ghostClone.transform.GetComponent<UnplacedBlockScript>());
        foreach (Transform t in ghostClone.transform)
        {
            t.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0.4f);
        }
        ghostClone.transform.localScale *= 2;
        ghostClone.SetActive(false);
    }


    private void OnMouseOver()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            IsBeingPlaced = true;
        }
        else if (GetClosestValidPosition().x != -1)
        {
            Debug.Log("Tried to place block");
            Vector2Int Save = GameBoard.instance.ClosestBlock(transform.position);
            foreach (Vector2Int pos in Positions)
            {
                GameBoard.instance.SetBlockAtPos(Save.x + pos.x, Save.y + pos.y, color);
                ScoreManagement.Instance.AddScore(1, GameBoard.instance.GetBlockAtPosition(Save.x + pos.x, Save.y + pos.y).transform.position);
            }
            GameBoard.instance.ClearBoard();
            ClearHighlights();
            BlockPlacementArea.Instance.StartCoroutine(BlockPlacementArea.Instance.GetNextBlockSpawns());


            ScoreManagement.Instance.PlaceBlock(Positions, this, transform.position);
            Destroy(gameObject);
        }
        else
        {
            IsBeingPlaced = false;
        }


    }

    


    void Update()
    {

        if (IsBeingPlaced)
        {
            transform.localScale = new Vector3(1, 1, 1);
            
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(
               new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, -Camera.main.transform.position.z)
            );
            transform.position = mouseWorldPosition;
            CreateGhostBlocks();

        }
        else
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            transform.position = originalPosition;
            ghostClone.SetActive(false);
        }

        if (!Mouse.current.leftButton.isPressed) IsBeingPlaced = false;

    }
    private void CreateGhostBlocks()
    {
        ClearHighlights();

        Vector2 closestPos = GetClosestValidPosition();

        if (closestPos.x == -1)
        {
            ghostClone.SetActive(false);
            return;
        }

        ghostClone.transform.position = new Vector3(closestPos.x, closestPos.y, 1);
        ghostClone.SetActive(true);

        HighlightLinesThatWouldClear();
    }

    private Vector2 GetClosestValidPosition(float MaxDistance = 1.6f)
    {
        Vector2Int attempted = GameBoard.instance.ClosestBlock(transform.position);

        bool Possible = true;

        foreach (Vector2Int pos in Positions)
        {
            if (attempted.x + pos.x < 0 || attempted.x + pos.x > GameBoard.instance.BoardSize().x -1 ) Possible = false;
            else if (attempted.y + pos.y < 0 || attempted.y + pos.y > GameBoard.instance.BoardSize().y -1 || GameBoard.instance.GridMap[attempted.x + pos.x, attempted.y + pos.y]) Possible = false;
            if (!Possible) return new Vector2(-1, -1);
        }
        if (Vector2.Distance(transform.position, GameBoard.instance.GetRealPosition(attempted)) > MaxDistance) return new Vector2(-1, -1);
        return GameBoard.instance.GridObjects[attempted.x, attempted.y].transform.position;
    }




    //ToDo: This is ugly and should share code with the actual gameboard line clearing, but it works for now
    private void HighlightLinesThatWouldClear()
    {
        Vector2Int attempted = GameBoard.instance.ClosestBlock(transform.position);

        bool[,] simulated = (bool[,])GameBoard.instance.GridMap.Clone();

        foreach (Vector2Int pos in Positions)
        {
            simulated[attempted.x + pos.x, attempted.y + pos.y] = true;
        }

        for (int y = 0; y < GameBoard.instance.BoardSize().y; y++)
        {
            bool full = true;

            for (int x = 0; x < GameBoard.instance.BoardSize().x; x++)
            {
                if (!simulated[x, y])
                {
                    full = false;
                    break;
                }
            }

            if (full)
            {
                for (int x = 0; x < GameBoard.instance.BoardSize().x; x++)
                {
                    HighlightCell(x, y);
                }
            }
        }

        for (int x = 0; x < GameBoard.instance.BoardSize().x; x++)
        {
            bool full = true;

            for (int y = 0; y < GameBoard.instance.BoardSize().y; y++)
            {
                if (!simulated[x, y])
                {
                    full = false;
                    break;
                }
            }

            if (full)
            {
                for (int y = 0; y < GameBoard.instance.BoardSize().y; y++)
                {
                    HighlightCell(x, y);
                }
            }
        }
    }

    private void HighlightCell(int x, int y)
    {
        BlockScript block = GameBoard.instance.GridObjects[x, y].GetComponent<BlockScript>();

        if (!highlightedBlocks.Contains(block))
        {
            highlightedBlocks.Add(block);
            block.highlight();
        }
    }

    private void ClearHighlights()
    {
        foreach (BlockScript block in highlightedBlocks)
        {
            if (block != null)
                block.dehighlight();
        }

        highlightedBlocks.Clear();
    }

}
