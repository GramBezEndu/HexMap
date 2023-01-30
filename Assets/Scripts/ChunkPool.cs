using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkPool : MonoBehaviour
{
    [SerializeField]
    private Texture texture;

    [SerializeField]
    private Material sharedMaterial;

    private static ChunkPool instance;

    public static ChunkPool Instance => instance;

    public HexSharedInfo HexSharedInfo { get; private set; }

    private ChunkData[] chunks;

    private readonly int chunkCount = 64;

    public Texture Texture => texture;

    public Material SharedMaterial => sharedMaterial;

    private void Awake()
    {
        instance = this;
        HexSharedInfo = new HexSharedInfo();
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
