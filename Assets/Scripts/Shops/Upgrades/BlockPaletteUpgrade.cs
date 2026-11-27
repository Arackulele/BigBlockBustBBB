using System.Collections.Generic;
using UnityEngine;

public abstract class BlockPaletteUpgrade : Upgrade
{
    protected virtual BlockDatabase ShapesToAdd => null;
    protected virtual BlockDatabase ShapesToRemove => null;

    public override List<List<Vector2Int>> ModifyBlockPalette(List<List<Vector2Int>> palette)
    {
        ApplyShapes(palette, ShapesToAdd, add: true);
        ApplyShapes(palette, ShapesToRemove, add: false);
        return palette;
    }

    private static void ApplyShapes(List<List<Vector2Int>> palette, BlockDatabase database, bool add)
    {
        if (palette == null || database == null) return;

        foreach (BlockShape entry in database.shapes)
        {
            if (entry?.cells == null || entry.cells.Count == 0) continue;

            bool exists = palette.Exists(shape => SameShapeUpToRotation(shape, entry.cells));
            if (add && !exists)
                palette.Add(new List<Vector2Int>(entry.cells));
            else if (!add)
                palette.RemoveAll(shape => SameShapeUpToRotation(shape, entry.cells));
        }
    }

    private static bool SameShapeUpToRotation(List<Vector2Int> shape, List<Vector2Int> pattern)
    {
        if (shape == null || pattern == null || shape.Count != pattern.Count || shape.Count == 0) return false;

        for (int rotation = 0; rotation < 4; rotation++)
        {
            foreach (Vector2Int shapeAnchor in shape)
            {
                Vector2Int patternAnchor = Rotate(pattern[0], rotation);
                Vector2Int offset = shapeAnchor - patternAnchor;
                bool matches = true;

                foreach (Vector2Int cell in pattern)
                {
                    if (!shape.Contains(Rotate(cell, rotation) + offset))
                    {
                        matches = false;
                        break;
                    }
                }

                if (matches) return true;
            }
        }

        return false;
    }

    private static Vector2Int Rotate(Vector2Int cell, int quarterTurns)
    {
        for (int i = 0; i < quarterTurns; i++)
            cell = new Vector2Int(-cell.y, cell.x);
        return cell;
    }
}
