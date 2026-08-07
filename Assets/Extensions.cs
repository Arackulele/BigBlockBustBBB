
using System.Collections.Generic;
using UnityEngine;


public static class Extensions
{


    public static T GetRandomItem<T>(this List<T> self)
    {
        if (self.Count > 0) return self[UnityEngine.Random.Range(0, self.Count)];
        else
        {
            return default(T);
        }
    }

    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public static Vector3 SnapPosition(Vector3 input, float factor = 1f)
    {

        float x = Mathf.Round(input.x / factor) * factor;
        float y = Mathf.Round(input.y / factor) * factor;
        float z = 0;

        return new Vector3(x, y, z);
    }

    public static Vector2 SnapPosition(Vector2 input, float factor = 1f)
    {

        float x = Mathf.Round(input.x / factor) * factor;
        float y = Mathf.Round(input.y / factor) * factor;

        return new Vector2(x, y);
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    public static Vector2 RandomPointInBounds2D(Bounds bounds)
    {
        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );
    }


}
