using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRenderer
{
    public ChunkInfo ChunkInfo { get; private set; }

    private GameObject chunkGO;

    public ChunkRenderer(ChunkInfo chunkInfo)
    {
        ChunkInfo = chunkInfo;
    }

    public void InitChunk()
    {
        chunkGO = new GameObject("Chunk");
        var meshFilter = chunkGO.AddComponent<MeshFilter>();
        var meshRenderer = chunkGO.AddComponent<MeshRenderer>();

        var chunkLength = HexSharedInfo.Instance.ChunkLength;
        var cellCount = chunkLength * chunkLength;
        var hexes = new HexInfo[cellCount];

        for (int i = 0; i < ChunkInfo.HexType.Length; i++)
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
                LocalPosition = new Vector3(
                    offset + HexSharedInfo.Instance.HexSize.x * column,
                    (HexSharedInfo.Instance.HexSize.y - HexSharedInfo.Instance.HeightAdjustment) * row,
                    0f),
                HexType = ChunkInfo.HexType[i],
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

        chunkGO.transform.position = new Vector2(
            ChunkInfo.Column * HexSharedInfo.Instance.ChunkSize.x,
            ChunkInfo.Row * HexSharedInfo.Instance.ChunkSize.y);
    }
}
