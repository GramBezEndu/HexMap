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

	[SerializeField]
	private GameObject hexHighlight;

	private DisplayHexDetails displayHexDetails;

	private new Camera camera;

    private void Awake()
    {
		camera = GetComponent<Camera>();
		displayHexDetails = hexDetailsPanel.GetComponent<DisplayHexDetails>();
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

		// Global hex cell X, Y
		int x = chunkColumn / HexSharedInfo.ChunkLength;
		int y = chunkRow / HexSharedInfo.ChunkLength;

		ChunkInfo chunkInfo = mapGenerator.Chunks[x, y];
		HexType hexType = chunkInfo.HexType[localRow * HexSharedInfo.ChunkLength + localColumn];
		HexDetails hexDetails = new HexDetails()
		{
			GlobalIndex = hexGlobalIndex,
			HexType = hexType,
			WorldPosition = GetWorldPosition(chunkColumn, chunkRow),
			ChunkCell = new Vector2Int(localColumn, localRow),
			Chunk = new Vector2Int(x, y),
		};

		if (hexType == HexType.Yellow || hexType == HexType.Green)
        {
			// New hex cell clicked
			if (displayHexDetails.HexDetails == null || displayHexDetails.HexDetails.GlobalIndex != hexDetails.GlobalIndex)
            {
				// Hex details
				displayHexDetails.HexDetails = hexDetails;
				hexDetailsPanel.SetActive(true);
				// Highlight
				hexHighlight.SetActive(true);
				hexHighlight.transform.position = hexDetails.WorldPosition;
			}
			else
            {
				hexDetailsPanel.SetActive(!hexDetailsPanel.activeInHierarchy);
				hexHighlight.SetActive(!hexHighlight.activeInHierarchy);
			}
		}
		else if (hexDetailsPanel.activeInHierarchy)
        {
			hexDetailsPanel.SetActive(false);
			hexHighlight.SetActive(false);
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

	private Vector2 GetWorldPosition(int chunkColumn, int chunkRow)
    {
		// TODO: Remove duplicate code
		bool isRowEven = (chunkRow % 2 == 0) ? true : false;
		float offset = 0f;
		if (!isRowEven)
		{
			offset = ChunkPool.Instance.HexSharedInfo.HexSize.x / 2f;
		}

		return new Vector2(
			offset + (chunkColumn * ChunkPool.Instance.HexSharedInfo.HexSize.x),
			chunkRow * (ChunkPool.Instance.HexSharedInfo.HexSize.y - ChunkPool.Instance.HexSharedInfo.HeightAdjustment));
    }
}
