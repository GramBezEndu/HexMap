using UnityEngine;

public class HexDetails
{
    public int GlobalIndex { get; set; }

    public CellType HexType { get; set; }

    public Vector2 WorldPosition { get; set; }

    public Vector2Int ChunkCell { get; set; }

    public Vector2Int Chunk { get; set; }
}
