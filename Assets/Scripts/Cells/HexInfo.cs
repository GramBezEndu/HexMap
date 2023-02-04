using System.Collections.Generic;
using UnityEngine;

public class HexInfo
{
    private CellType hexType = CellType.Blue;

    public Mesh Mesh { get; private set; }

    public Vector3 LocalPosition { get; set; }

    public CellType HexType 
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
        Mesh = new Mesh
        {
            vertices = HexSharedInfo.Instance.SharedMesh.vertices,
            uv = HexSharedInfo.Instance.SharedMesh.uv,
            triangles = HexSharedInfo.Instance.SharedMesh.triangles
        };
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

    private Color GetColor(CellType type)
    {
        switch (type)
        {
            case CellType.Blue:
                return Color.blue;
            case CellType.Green:
                return Color.green;
            case CellType.Yellow:
                return Color.yellow;
            default:
                return Color.gray;
        }
    }
}
