using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public int PositionX { get; set; }

    public int PositionY { get; set; }

    private MeshFilter meshFilter;

    private MeshRenderer meshRenderer;

    private HexInfo[] hexes;

    private readonly int chunkLength = 20;

    private int cellCount;

    private void Awake()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
    }

    private void Start()
    {
        cellCount = chunkLength * chunkLength;
        hexes = new HexInfo[cellCount];
        for (int i = 0; i < cellCount; i++)
        {
            int row = i / chunkLength;
            int column = i % chunkLength;
            bool isRowEven = (row % 2 == 0) ? true : false;
            float offset = 0f;
            if (isRowEven)
            {
                offset = HexSharedInfo.Instance.HexSize.x / 2f;
            }

            hexes[i] = new HexInfo()
            {
                Chunk = this,
                LocalPosition = new Vector3(
                    offset + HexSharedInfo.Instance.HexSize.x * column,
                    (HexSharedInfo.Instance.HexSize.y - HexSharedInfo.Instance.HeightAdjustment) * row,
                    0f),
            };

            hexes[i].InitializeMesh();
        }

        CombineInstance[] combineInstances = new CombineInstance[cellCount];

        for (int i = 0; i < cellCount; i++)
        {
            HexInfo hex = hexes[i];
            combineInstances[i].mesh = hex.Mesh;
            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetTRS(hex.LocalPosition, Quaternion.identity, Vector3.one);
            combineInstances[i].transform = matrix;
        }

        meshFilter.mesh.CombineMeshes(combineInstances);
        meshFilter.mesh.RecalculateNormals();

        meshRenderer.material = HexSharedInfo.Instance.SharedMaterial;
        meshRenderer.material.mainTexture = HexSharedInfo.Instance.Texture;

        gameObject.transform.position = new Vector2(PositionX * 100f, PositionY * 100f);
    }
}
