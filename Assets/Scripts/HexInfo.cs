using System.Collections.Generic;
using UnityEngine;

public class HexInfo
{
    private HexType hexType = HexType.Blue;

    public Mesh Mesh { get; private set; }

    public Vector3 LocalPosition { get; set; }

    public HexType HexType 
    {
        get => hexType;
        set
        {
            if (hexType != value)
            {
                hexType = value;
                if (Mesh != null)
                {
                    UpdateMeshColor();
                }
            }
        }
    }

    public void InitializeMesh()
    {
        Mesh = new Mesh();
        Mesh.vertices = ChunkPool.Instance.HexSharedInfo.SharedMesh.vertices;
        Mesh.uv = ChunkPool.Instance.HexSharedInfo.SharedMesh.uv;
        Mesh.triangles = ChunkPool.Instance.HexSharedInfo.SharedMesh.triangles;
        UpdateMeshColor();
        Mesh.RecalculateNormals();
    }

    private void UpdateMeshColor()
    {
        List<Color> colors = new List<Color>();
        for (int i = 0; i < Mesh.vertices.Length; i++)
        {
            colors.Add(GetColor(HexType));
        }

        Mesh.colors = colors.ToArray();
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
