using System;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    private const int renderDistance = 3;

    [SerializeField]
    private new Camera camera;

    [SerializeField]
    private MapGenerator mapGenerator;

    private ChunkLoader chunkLoader = new ChunkLoader();

    private List<ChunkInfo> loadedChunks = new List<ChunkInfo>();

    private int currentX;

    private int currentY;

    public int CurrentX
    {
        get => currentX;
        private set
        {
            if (value != currentX)
            {
                currentX = value;
                LoadChunksAroundCamera();
                UnloadChunks();
            }
        }
    }

    public int CurrentY
    {
        get => currentY;
        private set
        {
            if (value != currentY)
            {
                currentY = value;
                LoadChunksAroundCamera();
                UnloadChunks();
            }
        }
    }

    private void Start()
    {
        CurrentX = (int)(camera.transform.position.x / ChunkPool.Instance.HexSharedInfo.ChunkSize.x);
        CurrentY = (int)(camera.transform.position.y / ChunkPool.Instance.HexSharedInfo.ChunkSize.y);

        LoadChunksAroundCamera();
    }

    private void Update()
    {
        CurrentX = (int)(camera.transform.position.x / ChunkPool.Instance.HexSharedInfo.ChunkSize.x);
        CurrentY = (int)(camera.transform.position.y / ChunkPool.Instance.HexSharedInfo.ChunkSize.y);
    }

    private void UnloadChunks()
    {
        int unloadDistance = renderDistance + 1;
        foreach (ChunkInfo chunk in loadedChunks.ToArray())
        {
            if (Math.Abs(currentX - chunk.Column) >= unloadDistance || Math.Abs(currentY - chunk.Row) >= unloadDistance)
            {
                chunkLoader.UnloadChunk(chunk);
                loadedChunks.Remove(chunk);
            }
        }
    }

    private void LoadChunksAroundCamera()
    {
        int minX = Math.Max(0, CurrentX - renderDistance);
        int maxX = Math.Min(mapGenerator.ChunksInRow, CurrentX + renderDistance);
        int minY = Math.Max(0, CurrentY - renderDistance);
        int maxY = Math.Min(mapGenerator.ChunksInRow, CurrentY + renderDistance);

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
