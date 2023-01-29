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
    private readonly int chunksInRow = 2;

    private readonly int blueCount = 400;

    private readonly int greyCount = 400;

    private readonly int greenCount = 400;

    private readonly int yellowCount = 400;

    private List<HexType> hexTypes = new List<HexType>();

    public ChunkInfo[,] Chunks { get; private set; }

    private void Awake()
    {
        // Add all hexes
        hexTypes.AddRange(Enumerable.Repeat(HexType.Blue, blueCount));
        hexTypes.AddRange(Enumerable.Repeat(HexType.Gray, greyCount));
        hexTypes.AddRange(Enumerable.Repeat(HexType.Green, greenCount));
        hexTypes.AddRange(Enumerable.Repeat(HexType.Yellow, yellowCount));

        // Shuffle
        System.Random rng = new System.Random();
        hexTypes = hexTypes.OrderBy(x => rng.Next()).ToList();

        // Create chunks
        Chunks = new ChunkInfo[chunksInRow, chunksInRow];

        var chunks = hexTypes.Chunk(400).ToList();
        for (int i = 0; i < chunks.Count(); i++)
        {
            int row = i / chunksInRow;
            int column = i % chunksInRow;
            List<HexType> hexTypes = chunks[i];
            var currentChunk = new ChunkInfo(hexTypes.ToArray(), column, row);
            Chunks[column, row] = currentChunk;
        }

        // Render test
        // TODO: Move this code to different class
        for (int i = 0; i < Chunks.GetLength(0); i++)
        {
            for (int j = 0; j < Chunks.GetLength(1); j++)
            {
                ChunkRenderer test = new ChunkRenderer(Chunks[i, j]);
                test.InitChunk();
            }
        }
    }
}
