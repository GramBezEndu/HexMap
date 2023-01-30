using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkData
{
    public GameObject ChunkGO { get; private set; }

    public MeshFilter MeshFilter { get; private set; }

    public MeshRenderer MeshRenderer { get; private set; }

    public ChunkData()
    {
        ChunkGO = new GameObject("Unused");
        //chunks[i].transform.parent = gameObject.transform;
        MeshFilter = ChunkGO.AddComponent<MeshFilter>();
        MeshRenderer = ChunkGO.AddComponent<MeshRenderer>();
        ChunkGO.SetActive(false);
    }
}
