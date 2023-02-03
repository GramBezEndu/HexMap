using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private readonly int blueChance = 60;

    private readonly int greyChance = 25;

    private readonly int greenChance = 10;

    private readonly int yellowChance = 5;

    private List<CellType> hexTypes = new List<CellType>();

    public ChunkInfo[,] Chunks { get; private set; }

    private void Awake()
    {
        int chunksInRow = WorldSettings.ChunksInRow;
        int chunkLength = WorldSettings.ChunkLength;
        int hexCount = chunksInRow * chunksInRow * chunkLength * chunkLength;
        // Add all hexes
        hexTypes.AddRange(Enumerable.Repeat(CellType.Blue, (int)(blueChance * hexCount / 100f)));
        hexTypes.AddRange(Enumerable.Repeat(CellType.Gray, (int)(greyChance * hexCount / 100f)));
        hexTypes.AddRange(Enumerable.Repeat(CellType.Green, (int)(greenChance * hexCount / 100f)));
        hexTypes.AddRange(Enumerable.Repeat(CellType.Yellow, (int)(yellowChance * hexCount / 100f)));

        // Shuffle
        System.Random rng = new System.Random();
        hexTypes = hexTypes.OrderBy(x => rng.Next()).ToList();

        // Create chunks
        Chunks = new ChunkInfo[chunksInRow, chunksInRow];

        List<List<CellType>> chunks = hexTypes.Chunk(WorldSettings.ChunkLength * WorldSettings.ChunkLength).ToList();
        for (int i = 0; i < chunks.Count(); i++)
        {
            int row = i / chunksInRow;
            int column = i % chunksInRow;
            List<CellType> hexTypes = chunks[i];
            ChunkInfo currentChunk = new ChunkInfo(hexTypes.ToArray(), column, row);
            Chunks[column, row] = currentChunk;
        }
    }
}
