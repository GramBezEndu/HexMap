using System;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    private const int renderDistance = 2;

    [SerializeField]
    private new Camera camera;

    [SerializeField]
    private MapGenerator mapGenerator;

    private ChunkLoader chunkLoader = new ChunkLoader();

    private List<ChunkInfo> loadedChunks = new List<ChunkInfo>();

    private int currentChunkX;

    private int currentChunkY;

    public int CurrentChunkX
    {
        get => currentChunkX;
        private set
        {
            if (value != currentChunkX)
            {
                currentChunkX = value;
                LoadChunksAroundCamera();
                UnloadChunks();
            }
        }
    }

    public int CurrentChunkY
    {
        get => currentChunkY;
        private set
        {
            if (value != currentChunkY)
            {
                currentChunkY = value;
                LoadChunksAroundCamera();
                UnloadChunks();
            }
        }
    }

    private void Start()
    {
        CurrentChunkX = (int)(camera.transform.position.x / HexSharedInfo.Instance.ChunkSize.x);
        CurrentChunkY = (int)(camera.transform.position.y / HexSharedInfo.Instance.ChunkSize.y);

        LoadChunksAroundCamera();
    }

    private void Update()
    {
        CurrentChunkX = (int)(camera.transform.position.x / HexSharedInfo.Instance.ChunkSize.x);
        CurrentChunkY = (int)(camera.transform.position.y / HexSharedInfo.Instance.ChunkSize.y);
    }

    private void UnloadChunks()
    {
        int unloadDistance = renderDistance + 1;
        foreach (ChunkInfo chunk in loadedChunks.ToArray())
        {
            if (Math.Abs(currentChunkX - chunk.Column) >= unloadDistance || Math.Abs(currentChunkY - chunk.Row) >= unloadDistance)
            {
                chunkLoader.UnloadChunk(chunk);
                loadedChunks.Remove(chunk);
            }
        }
    }

    private void LoadChunksAroundCamera()
    {
        int minX = Math.Max(0, CurrentChunkX - renderDistance);
        int maxX = Math.Min(WorldSettings.ChunksInRow, CurrentChunkX + renderDistance);
        int minY = Math.Max(0, CurrentChunkY - renderDistance);
        int maxY = Math.Min(WorldSettings.ChunksInRow, CurrentChunkY + renderDistance);

        for (int i = minX; i < maxX; i++)
        {
            for (int j = minY; j < maxY; j++)
            {
                ChunkInfo chunk = mapGenerator.Chunks[i, j];
                if (!loadedChunks.Contains(chunk))
                {
                    chunkLoader.LoadChunk(chunk);
                    loadedChunks.Add(chunk);
                }
            }
        }
    }
}
