public class ChunkInfo
{
    public HexType[] HexType { get; private set; }

    public int Column { get; private set; }

    public int Row { get; private set; }

    public ChunkInfo(HexType[] hexType, int column, int row)
    {
        HexType = hexType;
        Column = column;
        Row = row;
    }
}
