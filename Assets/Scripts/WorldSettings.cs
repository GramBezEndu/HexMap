using System.Collections.Generic;

public static class WorldSettings
{
    public const int ChunkLength = 20;

    public const int ChunksInRow = 50;

    public static Dictionary<CellType, int> CellChances { get; } = new Dictionary<CellType, int>()
    {
        [CellType.Blue] = 60,
        [CellType.Gray] = 25,
        [CellType.Green] = 10,
        [CellType.Yellow] = 5,
    };
}
