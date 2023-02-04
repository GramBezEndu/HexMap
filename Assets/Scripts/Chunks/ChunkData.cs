using UnityEngine;

public class ChunkData
{
    public GameObject ChunkGO { get; private set; }

    public MeshFilter MeshFilter { get; private set; }

    public MeshRenderer MeshRenderer { get; private set; }

    public MeshCollider MeshCollider { get; private set; }

    public HexInfo[] Hexes { get; private set; }

    public ChunkData()
    {
        ChunkGO = new GameObject("Unused");
        MeshFilter = ChunkGO.AddComponent<MeshFilter>();
        MeshRenderer = ChunkGO.AddComponent<MeshRenderer>();
        MeshCollider = ChunkGO.AddComponent<MeshCollider>();
        MeshRenderer.material = ChunkPool.Instance.SharedMaterial;
        MeshRenderer.material.mainTexture = ChunkPool.Instance.Texture;
        ChunkGO.SetActive(false);

        int chunkLength = WorldSettings.ChunkLength;
        int cellCount = chunkLength * chunkLength;
        Hexes = new HexInfo[cellCount];

        for (int i = 0; i < Hexes.Length; i++)
        {
            Hexes[i] = new HexInfo()
            {
                LocalPosition = GetHexPosition(i),
            };

            Hexes[i].InitializeMesh();
        }
    }

    private Vector3 GetHexPosition(int index)
    {
        int chunkLength = WorldSettings.ChunkLength;
        int row = index / chunkLength;
        int column = index % chunkLength;
        bool isRowEven = (row % 2 == 0) ? true : false;
        float offset = 0f;
        if (!isRowEven)
        {
            offset = HexSharedInfo.Instance.HexSize.x / 2f;
        }
        
        return new Vector3(
                    offset + HexSharedInfo.Instance.HexSize.x * column,
                    HexSharedInfo.Instance.RowHeight * row,
                    0f);
    }
}
