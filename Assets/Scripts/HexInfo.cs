using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexInfo
{
    public Mesh Mesh { get; private set; }

    public Vector3 LocalPosition { get; set; }

    public HexType HexType { get; set; } = HexType.Blue;

    public void InitializeMesh()
    {
        Mesh = new Mesh();
        Mesh.vertices = ChunkPool.Instance.HexSharedInfo.SharedMesh.vertices;
        Mesh.uv = ChunkPool.Instance.HexSharedInfo.SharedMesh.uv;
        Mesh.triangles = ChunkPool.Instance.HexSharedInfo.SharedMesh.triangles;
        var colors = new List<Color>();
        foreach (Vector3 v in Mesh.vertices)
        {
            colors.Add(GetColor(HexType));
        }

        Mesh.colors = colors.ToArray();
        Mesh.RecalculateNormals();
    }

    private Color GetColor(HexType type)
    {
        switch (type)
        {
            case HexType.Blue:
                return Color.blue;
            case HexType.Green:
                return Color.green;
            case HexType.Yellow:
                return Color.yellow;
            default:
                return Color.gray;
        }
    }
}
