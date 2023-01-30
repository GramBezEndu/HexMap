using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkPool : MonoBehaviour
{
    private static ChunkPool instance;

    public static ChunkPool Instance => instance;

    private GameObject[] chunks;

    private readonly int chunkCount = 64;

    private void Awake()
    {
        instance = this;
        chunks = new GameObject[chunkCount];
        for (int i = 0; i < chunkCount; i++)
        {
            chunks[i] = new GameObject("Unused");
            chunks[i].transform.parent = gameObject.transform;
            chunks[i].AddComponent<MeshFilter>();
            chunks[i].AddComponent<MeshRenderer>();
            chunks[i].SetActive(false);
        }
    }

    public GameObject GetChunk()
    {
        for (int i = 0; i < chunkCount; i++)
        {
            if (!chunks[i].activeInHierarchy)
            {
                return chunks[i];
            }
        }

        throw new InvalidOperationException("Chunk pool is empty");
    }
}
