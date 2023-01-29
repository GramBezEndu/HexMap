using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexInfo
{
    public HexChunk Chunk { get; set; }

    public Mesh Mesh { get; private set; }

    public Vector3 LocalPosition { get; set; }

    public void InitializeMesh()
    {
        Mesh = new Mesh();
        Mesh.vertices = Chunk.SharedMesh.vertices;
        Mesh.uv = Chunk.SharedMesh.uv;
        Mesh.triangles = Chunk.SharedMesh.triangles;
        Mesh.RecalculateNormals();
    }
}
