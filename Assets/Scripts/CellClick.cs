using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellClick : MonoBehaviour
{
	[SerializeField]
	private MapGenerator mapGenerator;

	[SerializeField]
	private GameObject hexDetailsPanel;

	private HexDetails hexDetails;

	private new Camera camera;

    private void Awake()
    {
		camera = GetComponent<Camera>();
    }

    private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			HandleInput();
		}
	}

	private void HandleInput()
	{
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit))
		{
            Vector3 worldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
			GameObject chunk = hit.collider.gameObject;
			TouchCell(worldPosition);
		}
	}

	private void TouchCell(Vector3 position)
	{
		Vector2Int coordinates = FromPosition(position);

		int hexGlobalIndex = 
			coordinates.x + coordinates.y * mapGenerator.ChunksInRow * HexSharedInfo.ChunkLength + coordinates.y / 2;

		int chunkColumn = hexGlobalIndex % (mapGenerator.ChunksInRow * HexSharedInfo.ChunkLength);
		int chunkRow = hexGlobalIndex / (mapGenerator.ChunksInRow * HexSharedInfo.ChunkLength);

		int localColumn = hexGlobalIndex % HexSharedInfo.ChunkLength;
		int localRow = (hexGlobalIndex / (mapGenerator.ChunksInRow * HexSharedInfo.ChunkLength)) % HexSharedInfo.ChunkLength;

		int x = chunkColumn / HexSharedInfo.ChunkLength;
		int y = chunkRow / HexSharedInfo.ChunkLength;

		ChunkInfo chunkInfo = mapGenerator.Chunks[x, y];
		HexType hexType = chunkInfo.HexType[localRow * HexSharedInfo.ChunkLength + localColumn];
		hexDetails = new HexDetails()
		{
			HexType = hexType,
		};

		if (hexType == HexType.Yellow || hexType == HexType.Green)
        {
			Debug.Log(string.Format("Chunk [{0}] [{1}]", x, y));
			Debug.Log("Hex type: " + hexType.ToString());
			hexDetailsPanel.SetActive(!hexDetailsPanel.activeInHierarchy);
		}
	}

    private Vector2Int FromPosition(Vector3 position)
    {
		float x = position.x / ChunkPool.Instance.HexSharedInfo.HexSize.x;
		float y = -x;

		float offset = position.y / (ChunkPool.Instance.HexSharedInfo.HexSize.y / 2f * 3f);
		x -= offset;
		y -= offset;

		int iX = Mathf.RoundToInt(x);
		int iY = Mathf.RoundToInt(y);
		int iZ = Mathf.RoundToInt(-x - y);

		if (iX + iY + iZ != 0)
		{
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x - y - iZ);

            if (dX > dY && dX > dZ)
            {
                iX = -iY - iZ;
            }
            else if (dZ > dY)
            {
                iZ = -iX - iY;
            }
        }

		return new Vector2Int(iX, iZ);
	}
}