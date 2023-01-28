using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunksRenderer : MonoBehaviour
{
    [SerializeField]
    private MapGenerator mapGenerator;

    [SerializeField]
    private GameObject hex;

    [SerializeField]
    private new Camera camera;

    private readonly int padding = 10;

    private readonly int renderDistance = 6;

    private GameObject allChunks;

    private List<Chunk> loadedChunks = new List<Chunk>();

    private Vector2 hexSize;

    Vector2 chunkSize;

    float heightAdjustment;

    private int currentX;
    
    private int currentY;

    public int CurrentX
    {
        get => currentX;
        private set
        {
            if (value != currentX)
            {
                currentX = value;
                LoadChunksAroundCamera();
                UnloadChunks();
            }
        }
    }

    public int CurrentY
    {
        get => currentY;
        private set
        {
            if (value != currentY)
            {
                currentY = value;
                LoadChunksAroundCamera();
                UnloadChunks();
            }
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        SpriteRenderer hexRenderer = hex.GetComponent<SpriteRenderer>();
        hexSize = hexRenderer.size;
        heightAdjustment = (float)(Math.Sqrt(Math.Pow(hexSize.y / 2f, 2f) - Math.Pow(hexSize.x / 2f, 2f)));
        chunkSize = new Vector2(hexSize.x * 10, (hexSize.y - heightAdjustment) * 10);
        Debug.Log("hexSize: " + hexSize);
        Debug.Log("chunkSize: " + chunkSize);

        CurrentX = (int)(camera.transform.position.x / chunkSize.x);
        CurrentY = (int)(camera.transform.position.y / chunkSize.y);

        allChunks = new GameObject("Loaded chunks");

        // TODO: Load chunks around camera
        LoadChunksAroundCamera();
        UnloadChunks();
    }

    private void LoadChunksAroundCamera()
    {
        int minX = Math.Max(0, CurrentX - renderDistance);
        // TODO: Remove harcoded values
        int maxX = Math.Min(100, CurrentX + renderDistance);
        int minY = Math.Max(0, CurrentY - renderDistance);
        int maxY = Math.Min(100, CurrentY + renderDistance);

        for (int i = minX; i < maxX; i++)
        {
            for (int j = minY; j < maxY; j++)
            {
                Chunk chunk = mapGenerator.Chunks[i, j];
                if (!chunk.IsLoaded)
                {
                    LoadChunk(chunk);
                    chunk.IsLoaded = true;
                    loadedChunks.Add(chunk);
                }
            }
        }

        Debug.Log("LOADED CHUNKS: " + loadedChunks.Count);
    }

    private void UnloadChunks()
    {
        // TODO: Unload chunks
        int unloadDistance = renderDistance + 2;
        foreach (Chunk chunk in loadedChunks.ToArray())
        {
            if (Math.Abs(currentX - chunk.PositionX) > unloadDistance || Math.Abs(currentY - chunk.PositionY) > unloadDistance)
            {
                Destroy(GameObject.Find(string.Format("[{0} {1}] Chunk", chunk.PositionX, chunk.PositionY)));
                chunk.IsLoaded = false;
                loadedChunks.Remove(chunk);
            }
        }
    }

    private void Update()
    {
        CurrentX = (int)(camera.transform.position.x / chunkSize.x);
        CurrentY = (int)(camera.transform.position.y / chunkSize.y);
    }

    private void LoadChunk(Chunk chunk)
    {
        GameObject chunkParent = new GameObject(string.Format("[{0} {1}] Chunk", chunk.PositionX, chunk.PositionY));
        chunkParent.transform.parent = allChunks.transform;
        chunkParent.transform.position = new Vector3(chunkSize.x * chunk.PositionX, chunkSize.y * chunk.PositionY, 0f);
        for (int i = 0; i < chunk.HexTypes.Length; i++)
        {
            int row = i / 10;
            int column = i % 10;
            bool isRowEven = (row % 2 == 0) ? true : false;
            float offset = 0f;
            if (isRowEven)
            {
                offset = hexSize.x / 2f;
            }

            GameObject hexCell = Instantiate(hex);
            hexCell.name = i + " Hex";
            hexCell.transform.parent = chunkParent.transform;
            hexCell.transform.localPosition = new Vector3(offset + (column * hexSize.x), row * (hexSize.y - heightAdjustment), 0);
            hexCell.GetComponent<SpriteRenderer>().color = GetColor(chunk.HexTypes[i]);
        }
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
