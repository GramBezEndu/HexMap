using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public int PositionX { get; private set; }

    public int PositionY { get; private set; }

    public HexType[] HexTypes { get; private set; }

    public Chunk(HexType[] hexTypes, int positionX, int positionY)
    {
        HexTypes = hexTypes;
        PositionX = positionX;
        PositionY = positionY;
    }
}
