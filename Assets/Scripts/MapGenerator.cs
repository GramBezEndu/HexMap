using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private List<CellType> hexTypes = new List<CellType>();

    public ChunkInfo[,] Chunks { get; private set; }

    private void Awake()
    {
        int chunksInRow = WorldSettings.ChunksInRow;
        AddCells(hexTypes);
        hexTypes = Shuffle(hexTypes);
        CreateChunks(chunksInRow);
    }

    private void AddCells(List<CellType> hexTypes)
    {
        int chunksInRow = WorldSettings.ChunksInRow;
        int chunkLength = WorldSettings.ChunkLength;
        int hexCount = chunksInRow * chunksInRow * chunkLength * chunkLength;
        int combinedChance = WorldSettings.CellChances.Values.Sum();
        foreach (KeyValuePair<CellType, int> cellChance in WorldSettings.CellChances)
        {
            hexTypes.AddRange(Enumerable.Repeat(cellChance.Key, (int)(cellChance.Value * hexCount / combinedChance)));
        }

        if (hexTypes.Count != hexCount)
        {
            int difference = hexCount - hexTypes.Count;
            Debug.Log(string.Format("Adding {0} blue cells to fill up chunks", difference));
            hexTypes.AddRange(Enumerable.Repeat(CellType.Blue, difference));
        }
    }

    private List<CellType> Shuffle(List<CellType> hexTypes)
    {
        System.Random rng = new System.Random();
        return hexTypes.OrderBy(x => rng.Next()).ToList();
    }

    private void CreateChunks(int chunksInRow)
    {
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
