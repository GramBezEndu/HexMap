using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunksRenderer : MonoBehaviour
{
    [SerializeField]
    private MapGenerator mapGenerator;

    [SerializeField]
    private GameObject hex;

    private readonly int padding = 10;

    // Start is called before the first frame update
    void Start()
    {
        RenderChunk(mapGenerator.TestChunk);
    }

    private void RenderChunk(Chunk chunk)
    {
        SpriteRenderer hexRenderer = hex.GetComponent<SpriteRenderer>();
        Vector2 hexSize = hexRenderer.size;
        Debug.Log("hexSize: " + hexSize);
        float heightAdjustment = (float)(Math.Sqrt(Math.Pow(hexSize.y / 2f, 2f) - Math.Pow(hexSize.x / 2f, 2f)));
        for (int i = 0; i < chunk.HexTypes.Length; i ++)
        {
            int row = i / 10;
            int column = i % 10;
            bool isRowEven = (row % 2 == 0) ? true : false;
            float offset = 0f;
            if (isRowEven)
            {
                offset = hexSize.x / 2f;
            }

            GameObject hexCell = Instantiate(hex, new Vector3(offset + (column * hexSize.x), row * (hexSize.y - heightAdjustment), 0), Quaternion.identity);
            hexCell.GetComponent<SpriteRenderer>().color = GetColor(chunk.HexTypes[i]);
        }
    }

    private Color GetColor(HexType type)
    {
        switch (type)
        {
            case HexType.Blue:
                return Color.blue;
            case HexType.Green:
                return Color.green;
            case HexType.Yellow:
                return Color.yellow;
            default:
                return Color.gray;
        }
    }
}
