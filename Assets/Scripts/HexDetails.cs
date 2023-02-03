using UnityEngine;

public class HexDetails
{
    public int GlobalIndex { get; set; }

    public HexType HexType { get; set; }

    public Vector2 WorldPosition { get; set; }

    public Vector2Int ChunkCell { get; set; }

    public Vector2Int Chunk { get; set; }
}
