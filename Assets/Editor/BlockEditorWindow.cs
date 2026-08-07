using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class BlockEditorWindow : EditorWindow
{
    private BlockDatabase database;

    private const int GridSize = 6;
    private bool[,] grid = new bool[GridSize, GridSize];

    private int selectedShape = -1;

    [MenuItem("Tools/Block Editor")]
    static void Open()
    {
        GetWindow<BlockEditorWindow>("Block Editor");
    }

    void OnGUI()
    {
        database = (BlockDatabase)EditorGUILayout.ObjectField(
            "Database",
            database,
            typeof(BlockDatabase),
            false);

        if (database == null)
        {
            EditorGUILayout.HelpBox("Assign a Block Database asset.", MessageType.Info);

            if (GUILayout.Button("Create New Database"))
            {
                database = CreateInstance<BlockDatabase>();

                AssetDatabase.CreateAsset(database, "Assets/BlockDatabase.asset");
                AssetDatabase.SaveAssets();
            }

            return;
        }

        GUILayout.Space(10);

        DrawGrid();

        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Clear"))
            ClearGrid();

        if (GUILayout.Button("Save As New Shape"))
            SaveCurrentShape();

        if (selectedShape >= 0)
        {
            if (GUILayout.Button("Overwrite Selected"))
                OverwriteShape();
        }

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(20);

        DrawShapeList();
    }

    void DrawGrid()
    {
        for (int y = GridSize - 1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();

            for (int x = 0; x < GridSize; x++)
            {
                Color old = GUI.backgroundColor;
                GUI.backgroundColor = grid[x, y] ? Color.green : Color.gray;

                if (GUILayout.Button("", GUILayout.Width(35), GUILayout.Height(35)))
                {
                    grid[x, y] = !grid[x, y];
                }

                GUI.backgroundColor = old;
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    void ClearGrid()
    {
        grid = new bool[GridSize, GridSize];
    }

    List<Vector2Int> GetCurrentCells()
    {
        List<Vector2Int> cells = new();

        int originX = GridSize / 2;
        int originY = GridSize / 2;

        // Read cells from editor grid.
        for (int x = 0; x < GridSize; x++)
        {
            for (int y = 0; y < GridSize; y++)
            {
                if (grid[x, y])
                    cells.Add(new Vector2Int(x - originX, y - originY));
            }
        }

        if (cells.Count == 0)
            return cells;

        // Calculate centroid.
        Vector2 centroid = Vector2.zero;
        foreach (var cell in cells)
            centroid += (Vector2)cell;

        centroid /= cells.Count;

        // Find occupied block nearest the centroid.
        Vector2Int newOrigin = cells[0];
        float bestDistance = float.MaxValue;

        foreach (var cell in cells)
        {
            float distance = ((Vector2)cell - centroid).sqrMagnitude;

            if (distance < bestDistance)
            {
                bestDistance = distance;
                newOrigin = cell;
            }
        }

        // Shift so that block becomes (0,0).
        for (int i = 0; i < cells.Count; i++)
            cells[i] -= newOrigin;

        return cells;
    }

    void SaveCurrentShape()
    {
        BlockShape shape = new();

        shape.name = "Shape " + database.shapes.Count;
        shape.cells = GetCurrentCells();

        database.shapes.Add(shape);

        EditorUtility.SetDirty(database);
        AssetDatabase.SaveAssets();
    }

    void OverwriteShape()
    {
        database.shapes[selectedShape].cells = GetCurrentCells();

        EditorUtility.SetDirty(database);
        AssetDatabase.SaveAssets();
    }

    void LoadShape(BlockShape shape)
    {
        ClearGrid();

        if (shape.cells.Count == 0)
            return;

        int minX = int.MaxValue;
        int maxX = int.MinValue;
        int minY = int.MaxValue;
        int maxY = int.MinValue;

        foreach (var cell in shape.cells)
        {
            minX = Mathf.Min(minX, cell.x);
            maxX = Mathf.Max(maxX, cell.x);
            minY = Mathf.Min(minY, cell.y);
            maxY = Mathf.Max(maxY, cell.y);
        }

        int width = maxX - minX + 1;
        int height = maxY - minY + 1;

        int offsetX = (GridSize - width) / 2 - minX;
        int offsetY = (GridSize - height) / 2 - minY;

        foreach (var cell in shape.cells)
        {
            int x = cell.x + offsetX;
            int y = cell.y + offsetY;

            if (x >= 0 && x < GridSize &&
                y >= 0 && y < GridSize)
            {
                grid[x, y] = true;
            }
        }
    }

    void DrawShapeList()
    {
        GUILayout.Label("Saved Shapes", EditorStyles.boldLabel);

        for (int i = 0; i < database.shapes.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            database.shapes[i].name =
                EditorGUILayout.TextField(database.shapes[i].name);

            if (GUILayout.Button("Load"))
            {
                selectedShape = i;
                LoadShape(database.shapes[i]);
            }

            if (GUILayout.Button("Delete"))
            {
                database.shapes.RemoveAt(i);

                EditorUtility.SetDirty(database);
                AssetDatabase.SaveAssets();

                break;
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}