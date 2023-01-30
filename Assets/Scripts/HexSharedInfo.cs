using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexSharedInfo : MonoBehaviour
{
    public static HexSharedInfo Instance => instance;

    [SerializeField]
    private Texture texture;

    [SerializeField]
    private Material sharedMaterial;

    private static HexSharedInfo instance;

    public Mesh SharedMesh { get; private set; }

    public Texture Texture => texture;

    public Material SharedMaterial => sharedMaterial;

    public Vector2 HexSize { get; private set; }

    public const int ChunkLength = 20;

    public Vector2 ChunkSize { get; private set; }

    public float HeightAdjustment { get; private set; }

    private void Awake()
    {
        CreateSharedMesh();
        GameObject hexHelperGO = new GameObject("Temp Hex");
        MeshFilter tempFilter = hexHelperGO.AddComponent<MeshFilter>();
        tempFilter.mesh = SharedMesh;
        hexHelperGO.AddComponent<MeshRenderer>();
        MeshCollider collider = hexHelperGO.AddComponent<MeshCollider>();
        HexSize = collider.bounds.size;
        HeightAdjustment = (HexSize.y - HexSize.y / 2f) / 2f;
        ChunkSize = new Vector2(HexSize.x * 10, (HexSize.y - HeightAdjustment) * 10);
        Destroy(hexHelperGO);

        instance = this;
    }

    private void CreateSharedMesh()
    {
        var vertices = new Vector3[]
        {
            new Vector3(-1f , -.5f),
            new Vector3(-1f, .5f),
            new Vector3(0f, 1f),
            new Vector3(1f, .5f),
            new Vector3(1f, -.5f),
            new Vector3(0f, -1f)
        };

        var triangles = new int[]
        {
            1, 5, 0,
            1, 4, 5,
            1, 2, 4,
            2, 3, 4
        };

        var uv = new Vector2[]
        {
            new Vector2(0, 0.25f),
            new Vector2(0, 0.75f),
            new Vector2(0.5f, 1),
            new Vector2(1, 0.75f),
            new Vector2(1, 0.25f),
            new Vector2(0.5f, 0),
        };

        SharedMesh = new Mesh();
        SharedMesh.vertices = vertices;
        SharedMesh.triangles = triangles;
        SharedMesh.uv = uv;
    }
}