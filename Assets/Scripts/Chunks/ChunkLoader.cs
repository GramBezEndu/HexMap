using UnityEngine;

public class ChunkLoader
{
    public void LoadChunk(ChunkInfo chunkInfo)
    {
        Chunk chunkData = ChunkPool.Instance.GetChunk();
        GameObject chunkGO = chunkData.ChunkGO;
        chunkGO.SetActive(true);
        chunkGO.name = GetChunkName(chunkInfo);
        MeshFilter meshFilter = chunkData.MeshFilter;

        int chunkLength = WorldSettings.ChunkLength;
        int cellCount = chunkLength * chunkLength;

        SetHexColors(chunkInfo, chunkData);
        CombineMeshes(chunkData, meshFilter, cellCount);
        SetChunkPosition(chunkInfo, chunkGO);
    }

    public void UnloadChunk(ChunkInfo chunkInfo)
    {
        GameObject chunk = GameObject.Find(GetChunkName(chunkInfo));
        chunk.SetActive(false);
        chunk.name = "Unused";
    }

    private void CombineMeshes(Chunk chunkData, MeshFilter meshFilter, int cellCount)
    {
        CombineInstance[] combineInstances = new CombineInstance[cellCount];
        for (int i = 0; i < cellCount; i++)
        {
            HexInfo hex = chunkData.Hexes[i];
            combineInstances[i].mesh = hex.Mesh;
            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetTRS(hex.LocalPosition, Quaternion.identity, Vector3.one);
            combineInstances[i].transform = matrix;
        }

        meshFilter.mesh.CombineMeshes(combineInstances);
        chunkData.MeshCollider.sharedMesh = meshFilter.mesh;
        meshFilter.mesh.RecalculateNormals();
    }

    private void SetHexColors(ChunkInfo chunkInfo, Chunk chunkData)
    {
        for (int i = 0; i < chunkInfo.HexType.Length; i++)
        {
            chunkData.Hexes[i].HexType = chunkInfo.HexType[i];
        }
    }

    private void SetChunkPosition(ChunkInfo chunkInfo, GameObject chunkGO)
    {
        chunkGO.transform.position = new Vector2(
            chunkInfo.Column * HexSharedInfo.Instance.ChunkSize.x,
            chunkInfo.Row * HexSharedInfo.Instance.ChunkSize.y);
    }

    private string GetChunkName(ChunkInfo chunkInfo)
    {
        return string.Format("[{0} {1}] Chunk", chunkInfo.Column, chunkInfo.Row);
    }
}
