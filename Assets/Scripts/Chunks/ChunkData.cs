using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkData
{
    public GameObject ChunkGO { get; private set; }

    public MeshFilter MeshFilter { get; private set; }

    public MeshRenderer MeshRenderer { get; private set; }

    public HexInfo[] Hexes { get; private set; }

    public ChunkData()
    {
        ChunkGO = new GameObject("Unused");
        MeshFilter = ChunkGO.AddComponent<MeshFilter>();
        MeshRenderer = ChunkGO.AddComponent<MeshRenderer>();
        MeshRenderer.material = ChunkPool.Instance.SharedMaterial;
        MeshRenderer.material.mainTexture = ChunkPool.Instance.Texture;
        ChunkGO.SetActive(false);

        int chunkLength = HexSharedInfo.ChunkLength;
        int cellCount = chunkLength * chunkLength;
        Hexes = new HexInfo[cellCount];

        for (int i = 0; i < Hexes.Length; i++)
        {
            int row = i / chunkLength;
            int column = i % chunkLength;
            bool isRowEven = (row % 2 == 0) ? true : false;
            float offset = 0f;
            if (isRowEven)
            {
                offset = ChunkPool.Instance.HexSharedInfo.HexSize.x / 2f;
            }

            Hexes[i] = new HexInfo()
            {
                LocalPosition = new Vector3(
                    offset + ChunkPool.Instance.HexSharedInfo.HexSize.x * column,
                    (ChunkPool.Instance.HexSharedInfo.HexSize.y - ChunkPool.Instance.HexSharedInfo.HeightAdjustment) * row,
                    0f),
            };

            Hexes[i].InitializeMesh();
        }
    }
}
