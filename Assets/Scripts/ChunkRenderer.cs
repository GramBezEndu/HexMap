using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRenderer
{
    private GameObject chunkGO;

    public void LoadChunk(ChunkInfo chunkInfo)
    {
        chunkGO = ChunkPool.Instance.GetChunk();
        chunkGO.SetActive(true);
        chunkGO.name = string.Format("[{0} {1}] Chunk", chunkInfo.Column, chunkInfo.Row);
        var meshFilter = chunkGO.GetComponent<MeshFilter>();
        var meshRenderer = chunkGO.GetComponent<MeshRenderer>();

        var chunkLength = HexSharedInfo.ChunkLength;
        var cellCount = chunkLength * chunkLength;
        var hexes = new HexInfo[cellCount];

        for (int i = 0; i < chunkInfo.HexType.Length; i++)
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
                HexType = chunkInfo.HexType[i],
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
            chunkInfo.Column * HexSharedInfo.Instance.ChunkSize.x,
            chunkInfo.Row * HexSharedInfo.Instance.ChunkSize.y);
    }

    public void UnloadChunk(ChunkInfo chunkInfo)
    {
        var chunk = GameObject.Find(string.Format("[{0} {1}] Chunk", chunkInfo.Column, chunkInfo.Row));
        chunk.SetActive(false);
        chunk.name = "Unused";
    }
}
