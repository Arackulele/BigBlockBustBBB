using System.Collections.Generic;
using UnityEngine;

public class UnplacedBlockScript : DraggableObject
{
    public List<Vector2Int> Positions = new List<Vector2Int>();

    [SerializeField]
    private GameObject BlockPrefab;

    private GameObject ghostClone;

    public Color color;

    private readonly List<BlockScript> highlightedBlocks = new();

    private void Start()
    {
        color = ThemerScript.Instance.CurrentTheme.BlockColors.GetRandomItem();

        CreateBlocks();
        CreateGhost();
    }

    private void CreateBlocks()
    {
        foreach (Vector2Int pos in Positions)
        {
            GameObject posBlock = Instantiate(BlockPrefab, transform);

            posBlock.transform.localPosition = new Vector2(
                pos.x * GameBoard.instance.BoardScale,
                pos.y * GameBoard.instance.BoardScale
            );

            posBlock.GetComponent<SpriteRenderer>().material.color = color;
        }
    }

    private void CreateGhost()
    {
        ghostClone = Instantiate(gameObject, transform);

        // Remove the dragging script from the clone.
        Destroy(ghostClone.GetComponent<UnplacedBlockScript>());

        foreach (Transform child in ghostClone.transform)
        {
            SpriteRenderer sprite = child.GetComponent<SpriteRenderer>();

            if (sprite != null)
            {
                sprite.color = new Color(
                    color.r,
                    color.g,
                    color.b,
                    0.4f
                );
            }
        }

        ghostClone.transform.localScale *= 2;
        ghostClone.SetActive(false);
    }

    protected override void OnStartDragging()
    {
        transform.localScale = Vector3.one;
    }

    protected override void OnDragging()
    {
        CreateGhostBlocks();
    }

    protected override void OnCancelDragging()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        ClearHighlights();

        if (ghostClone != null)
            ghostClone.SetActive(false);
    }

    protected override void OnPlace(Vector2 validPosition)
    {
        Vector2Int save = GameBoard.instance.ClosestBlock(transform.position);

        foreach (Vector2Int pos in Positions)
        {
            int x = save.x + pos.x;
            int y = save.y + pos.y;

            GameBoard.instance.SetBlockAtPos(x, y, color);

            ScoreManagement.Instance.AddScore(
                1,
                GameBoard.instance.GetBlockAtPosition(x, y).transform.position
            );
        }

        GameBoard.instance.ClearBoard();

        ClearHighlights();

        BlockPlacementArea.Instance.StartCoroutine(
            BlockPlacementArea.Instance.GetNextBlockSpawns()
        );

        ScoreManagement.Instance.PlaceBlock(
            Positions,
            this,
            transform.position
        );

        Destroy(gameObject);
    }

    protected override bool TryGetValidDropPosition(out Vector2 validPosition)
    {
        validPosition = GetClosestValidPosition();

        return validPosition.x != -1;
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

        ghostClone.transform.position = new Vector3(
            closestPos.x,
            closestPos.y,
            1
        );

        ghostClone.SetActive(true);

        HighlightLinesThatWouldClear();
    }

    private Vector2 GetClosestValidPosition(float maxDistance = 1.6f)
    {
        Vector2Int attempted =
            GameBoard.instance.ClosestBlock(transform.position);

        foreach (Vector2Int pos in Positions)
        {
            int x = attempted.x + pos.x;
            int y = attempted.y + pos.y;

            // Outside board
            if (x < 0 ||
                x >= GameBoard.instance.BoardSize().x ||
                y < 0 ||
                y >= GameBoard.instance.BoardSize().y)
            {
                return new Vector2(-1, -1);
            }

            // Occupied
            if (GameBoard.instance.GridMap[x, y])
            {
                return new Vector2(-1, -1);
            }
        }

        // Too far away from the board position
        if (Vector2.Distance(
                transform.position,
                GameBoard.instance.GetRealPosition(attempted)
            ) > maxDistance)
        {
            return new Vector2(-1, -1);
        }

        return GameBoard.instance.GridObjects[
            attempted.x,
            attempted.y
        ].transform.position;
    }

    // ToDo: This should eventually share code with the actual board
    // line-clearing logic.
    private void HighlightLinesThatWouldClear()
    {
        Vector2Int attempted =
            GameBoard.instance.ClosestBlock(transform.position);

        bool[,] simulated =
            (bool[,])GameBoard.instance.GridMap.Clone();

        foreach (Vector2Int pos in Positions)
        {
            simulated[
                attempted.x + pos.x,
                attempted.y + pos.y
            ] = true;
        }

        // Check rows
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

        // Check columns
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
        BlockScript block =
            GameBoard.instance.GridObjects[x, y]
                .GetComponent<BlockScript>();

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