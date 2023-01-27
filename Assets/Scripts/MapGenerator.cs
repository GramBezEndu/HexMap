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
    private readonly int blueCount = 600000;

    private readonly int greyCount = 250000;

    private readonly int greenCount = 100000;

    private readonly int yellowCount = 50000;

    private List<HexType> hexTypes = new List<HexType>();

    public Chunk TestChunk { get; private set; }

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

        for (int i = 0; i < 30; i++)
        {
            //Debug.Log("i: " + i + " " + hexTypes[i]);
        }

        TestChunk = new Chunk(hexTypes.Take(100).ToArray());
    }
}
