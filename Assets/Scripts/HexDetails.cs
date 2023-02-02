using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexDetails
{
    public HexType HexType { get; set; }

    public Vector2 WorldPosition { get; set; }

    public Vector2Int LocalPosition { get; set; }

    public Vector2Int Chunk { get; set; }
}
