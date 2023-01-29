using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexInfo : MonoBehaviour
{
    [SerializeField]
    private Texture texture;

    private Vector3[] vertices;

    private Vector2[] uv;

    private int[] triangles;

    private void Start()
    {
        vertices = new Vector3[]
        {
            new Vector3(-1f , -.5f),
            new Vector3(-1f, .5f),
            new Vector3(0f, 1f),
            new Vector3(1f, .5f),
            new Vector3(1f, -.5f),
            new Vector3(0f, -1f)
        };

        triangles = new int[]
        {
            1, 5, 0,
            1, 4, 5,
            1, 2, 4,
            2, 3, 4
        };

        uv = new Vector2[]
        {
            new Vector2(0, 0.25f),
            new Vector2(0, 0.75f),
            new Vector2(0.5f, 1),
            new Vector2(1, 0.75f),
            new Vector2(1, 0.25f),
            new Vector2(0.5f, 0),
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        gameObject.AddComponent<MeshFilter>();
        GetComponent<MeshFilter>().mesh = mesh;

        MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
        renderer.material.mainTexture = texture;
    }
}
