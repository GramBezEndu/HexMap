using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRenderer
{
    private GameObject chunkGO;

    public void LoadChunk(ChunkInfo chunkInfo)
    {
        var chunkData = ChunkPool.Instance.GetChunk();
        chunkGO = chunkData.ChunkGO;
        chunkGO.SetActive(true);
        chunkGO.name = string.Format("[{0} {1}] Chunk", chunkInfo.Column, chunkInfo.Row);
        var meshFilter = chunkData.MeshFilter;
        var meshRenderer = chunkData.MeshRenderer;

        var chunkLength = HexSharedInfo.ChunkLength;
        var cellCount = chunkLength * chunkLength;
        var hexes = chunkData.Hexes;

        for (int i = 0; i < chunkInfo.HexType.Length; i++)
        {
            hexes[i].HexType = chunkInfo.HexType[i];
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

        meshRenderer.material = ChunkPool.Instance.SharedMaterial;
        meshRenderer.material.mainTexture = ChunkPool.Instance.Texture;

        chunkGO.transform.position = new Vector2(
            chunkInfo.Column * ChunkPool.Instance.HexSharedInfo.ChunkSize.x,
            chunkInfo.Row * ChunkPool.Instance.HexSharedInfo.ChunkSize.y);
    }

    public void UnloadChunk(ChunkInfo chunkInfo)
    {
        var chunk = GameObject.Find(string.Format("[{0} {1}] Chunk", chunkInfo.Column, chunkInfo.Row));
        chunk.SetActive(false);
        chunk.name = "Unused";
    }
}
