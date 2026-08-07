using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Blockshapes/Block Database")]
public class BlockDatabase : ScriptableObject
{
    public List<BlockShape> shapes = new();
}

[System.Serializable]
public class BlockShape
{
    public string name = "New Shape";
    public List<Vector2Int> cells = new();
}
