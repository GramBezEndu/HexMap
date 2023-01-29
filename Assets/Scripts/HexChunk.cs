using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexChunk : MonoBehaviour
{
    [SerializeField]
    private Texture texture;

    // TODO: Should be one for all chunks;
    public Mesh SharedMesh { get; private set; }

    private MeshFilter meshFilter;

    private MeshRenderer meshRenderer;

    private HexInfo[] hexes;

    private void Awake()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
    }

    private void Start()
    {
        CreateSharedMesh();
        int k = 10;
        hexes = new HexInfo[k];
        for (int i = 0; i < k; i++)
        {
            hexes[i] = new HexInfo()
            {
                Chunk = this,
                LocalPosition = new Vector3(5 * i, 0f, 0f),
            };

            hexes[i].InitializeMesh();
        }

        CombineInstance[] combineInstances = new CombineInstance[k];

        for (int i = 0; i < k; i++)
        {
            combineInstances[i].mesh = hexes[i].Mesh;
            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetTRS(hexes[i].LocalPosition, Quaternion.identity, Vector3.one);
            combineInstances[i].transform = matrix;
        }

        meshFilter.mesh.CombineMeshes(combineInstances);
        meshFilter.mesh.RecalculateNormals();

        meshRenderer.material.mainTexture = texture;
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
