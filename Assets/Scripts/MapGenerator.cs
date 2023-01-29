using System.Collections;
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
    public int ChunksInRow { get; } = 50;

    private readonly int blueChance = 60;

    private readonly int greyChance = 25;

    private readonly int greenChance = 10;

    private readonly int yellowChance = 5;

    private List<HexType> hexTypes = new List<HexType>();

    public ChunkInfo[,] Chunks { get; private set; }

    private void Awake()
    {
        int hexCount = ChunksInRow * ChunksInRow * HexSharedInfo.Instance.ChunkLength * HexSharedInfo.Instance.ChunkLength;
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

        var chunks = hexTypes.Chunk(400).ToList();
        for (int i = 0; i < chunks.Count(); i++)
        {
            int row = i / ChunksInRow;
            int column = i % ChunksInRow;
            List<HexType> hexTypes = chunks[i];
            var currentChunk = new ChunkInfo(hexTypes.ToArray(), column, row);
            Chunks[column, row] = currentChunk;
        }

        //// Render test
        //// TODO: Move this code to different class
        //ChunkRenderer testRenderer = new ChunkRenderer();
        //for (int i = 0; i < Chunks.GetLength(0); i++)
        //{
        //    for (int j = 0; j < Chunks.GetLength(1); j++)
        //    {
        //        testRenderer.LoadChunk(Chunks[i, j]);
        //    }
        //}
    }
}
