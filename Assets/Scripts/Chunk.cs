using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    private int positionX;

    private int positionY;

    public HexType[] HexTypes { get; private set; }

    public Chunk(HexType[] hexTypes)
    {
        HexTypes = hexTypes;
    }
}
