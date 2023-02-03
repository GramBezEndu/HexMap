using System;
using UnityEngine;

public class ChunkPool : MonoBehaviour
{
    private const int chunkCount = 64;

    private static ChunkPool instance;

    [SerializeField]
    private Texture texture;

    [SerializeField]
    private Material sharedMaterial;

    private ChunkData[] chunks;

    public static ChunkPool Instance => instance;

    public HexSharedInfo HexSharedInfo { get; private set; }

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
            chunks[i].ChunkGO.transform.parent = transform.gameObject.transform;
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
