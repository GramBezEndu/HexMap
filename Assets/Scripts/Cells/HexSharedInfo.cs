using UnityEngine;

public class HexSharedInfo
{
    private static HexSharedInfo instance;

    public static HexSharedInfo Instance => instance;

    public Mesh SharedMesh { get; private set; }

    public Vector2 HexSize { get; private set; }

    public Vector2 ChunkSize { get; private set; }

    public float HeightAdjustment { get; private set; }

    public float RowHeight => HexSize.y - HeightAdjustment;

    public HexSharedInfo()
    {
        instance = this;
        SharedMesh = CreateSharedMesh();
        GameObject tempHex = new GameObject("Temp Hex");
        MeshCollider collider = SetupTemporaryHex(tempHex);
        HexSize = collider.bounds.size;
        HeightAdjustment = (HexSize.y - (HexSize.y / 2f)) / 2f;
        ChunkSize = new Vector2(HexSize.x * WorldSettings.ChunkLength, RowHeight * WorldSettings.ChunkLength);
        Object.Destroy(tempHex);
    }

    private MeshCollider SetupTemporaryHex(GameObject tempHex)
    {
        MeshFilter tempFilter = tempHex.AddComponent<MeshFilter>();
        tempFilter.mesh = SharedMesh;
        tempHex.AddComponent<MeshRenderer>();
        MeshCollider collider = tempHex.AddComponent<MeshCollider>();
        return collider;
    }

    private Mesh CreateSharedMesh()
    {
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-1f, -0.5f),
            new Vector3(-1f, 0.5f),
            new Vector3(0f, 1f),
            new Vector3(1f, 0.5f),
            new Vector3(1f, -0.5f),
            new Vector3(0f, -1f)
        };

        int[] triangles = new int[]
        {
            1, 5, 0,
            1, 4, 5,
            1, 2, 4,
            2, 3, 4
        };

        Vector2[] uv = new Vector2[]
        {
            new Vector2(0, 0.25f),
            new Vector2(0, 0.75f),
            new Vector2(0.5f, 1),
            new Vector2(1, 0.75f),
            new Vector2(1, 0.25f),
            new Vector2(0.5f, 0),
        };

        Mesh mesh = new Mesh()
        {
            vertices = vertices,
            triangles = triangles,
            uv = uv
        };

        return mesh;
    }
}
