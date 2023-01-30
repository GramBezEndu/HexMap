using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum HexType
{
    Blue,
    Green,
    Yellow,
    Gray,
};

public class MapGenerator : MonoBehaviour
{
    private readonly int blueChance = 60;

    private readonly int greyChance = 25;

    private readonly int greenChance = 10;

    private readonly int yellowChance = 5;

    private List<HexType> hexTypes = new List<HexType>();

    public int ChunksInRow => 50;

    public ChunkInfo[,] Chunks { get; private set; }

    private void Awake()
    {
        int hexCount = ChunksInRow * ChunksInRow * HexSharedInfo.ChunkLength * HexSharedInfo.ChunkLength;
        // Add all hexes
        hexTypes.AddRange(Enumerable.Repeat(HexType.Blue, (int)(blueChance * hexCount / 100f)));
        hexTypes.AddRange(Enumerable.Repeat(HexType.Gray, (int)(greyChance * hexCount / 100f)));
        hexTypes.AddRange(Enumerable.Repeat(HexType.Green, (int)(greenChance * hexCount / 100f)));
        hexTypes.AddRange(Enumerable.Repeat(HexType.Yellow, (int)(yellowChance * hexCount / 100f)));

        // Shuffle
        System.Random rng = new System.Random();
        hexTypes = hexTypes.OrderBy(x => rng.Next()).ToList();

        // Create chunks
        Chunks = new ChunkInfo[ChunksInRow, ChunksInRow];

        List<List<HexType>> chunks = hexTypes.Chunk(HexSharedInfo.ChunkLength * HexSharedInfo.ChunkLength).ToList();
        for (int i = 0; i < chunks.Count(); i++)
        {
            int row = i / ChunksInRow;
            int column = i % ChunksInRow;
            List<HexType> hexTypes = chunks[i];
            ChunkInfo currentChunk = new ChunkInfo(hexTypes.ToArray(), column, row);
            Chunks[column, row] = currentChunk;
        }
    }
}
