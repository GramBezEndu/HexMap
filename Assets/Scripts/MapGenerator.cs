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
    private readonly int chunksInRow = 100;

    private readonly int blueCount = 600000;

    private readonly int greyCount = 250000;

    private readonly int greenCount = 100000;

    private readonly int yellowCount = 50000;

    private List<HexType> hexTypes = new List<HexType>();

    public Chunk[,] Chunks { get; private set; }

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

        Chunks = new Chunk[chunksInRow, chunksInRow];

        var chunks = hexTypes.Chunk(100).ToList();
        for (int i = 0; i < chunks.Count(); i++)
        {
            int row = i / chunksInRow;
            int column = i % chunksInRow;
            List<HexType> hexTypes = chunks[i];
            var currentChunk = new Chunk(hexTypes.ToArray(), column, row);
            Chunks[column, row] = currentChunk;
        }
    }
}
