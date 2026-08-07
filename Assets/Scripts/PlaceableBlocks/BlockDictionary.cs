using System.Collections.Generic;
using UnityEngine;

public static class BlockDictionary
{
    public static List<List<Vector2Int>> BlockShapes = new();

    public static void Load(BlockDatabase database)
    {
        BlockShapes.Clear();

        foreach (var shape in database.shapes)
        {
            BlockShapes.Add(new List<Vector2Int>(shape.cells));
        }
    }
}