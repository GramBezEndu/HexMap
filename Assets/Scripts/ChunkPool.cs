using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkPool : MonoBehaviour
{
    private static ChunkPool instance;

    public static ChunkPool Instance => instance;

    private ChunkData[] chunks;

    private readonly int chunkCount = 64;

    private void Awake()
    {
        instance = this;
        chunks = new ChunkData[chunkCount];
        for (int i = 0; i < chunkCount; i++)
        {
            chunks[i] = new ChunkData();
        }
    }

    public ChunkData GetChunk()
    {
        for (int i = 0; i < chunkCount; i++)
        {
            if (!chunks[i].ChunkGO.activeInHierarchy)
            {
                return chunks[i];
            }
        }

        throw new InvalidOperationException("Chunk pool is empty");
    }
}
