public class ChunkInfo
{
    public CellType[] HexType { get; private set; }

    public int Column { get; private set; }

    public int Row { get; private set; }

    public ChunkInfo(CellType[] hexType, int column, int row)
    {
        HexType = hexType;
        Column = column;
        Row = row;
    }
}
