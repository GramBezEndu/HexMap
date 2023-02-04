using UnityEngine;

[RequireComponent(typeof(Camera))]
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
		if (LeftMouseButtonJustPressed())
		{
			HandleInput();
		}

		bool LeftMouseButtonJustPressed()
		{
			return Input.GetMouseButtonDown(0);
		}
	}

	private void HandleInput()
	{
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(inputRay))
		{
            Vector3 worldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
			TouchCell(worldPosition);
		}
	}

	private void TouchCell(Vector3 worldPosition)
	{
		Vector2Int coordinates = GetHexCoordinates(worldPosition);

		int hexGlobalIndex = 
			coordinates.x + (coordinates.y * WorldSettings.ChunksInRow * WorldSettings.ChunkLength) + (coordinates.y / 2);

		int chunkColumn = hexGlobalIndex % (WorldSettings.ChunksInRow * WorldSettings.ChunkLength);
		int chunkRow = hexGlobalIndex / (WorldSettings.ChunksInRow * WorldSettings.ChunkLength);

		int localColumn = hexGlobalIndex % WorldSettings.ChunkLength;
		int localRow = (hexGlobalIndex / (WorldSettings.ChunksInRow * WorldSettings.ChunkLength)) % WorldSettings.ChunkLength;

		Vector2Int chunk = new Vector2Int(chunkColumn / WorldSettings.ChunkLength, chunkRow / WorldSettings.ChunkLength);

		ChunkInfo chunkInfo = mapGenerator.Chunks[chunk.x, chunk.y];
		CellType hexType = chunkInfo.HexType[localRow * WorldSettings.ChunkLength + localColumn];
		HexDetails hexDetails = new HexDetails()
		{
			GlobalIndex = hexGlobalIndex,
			HexType = hexType,
			WorldPosition = GetWorldPosition(chunkColumn, chunkRow),
			ChunkCell = new Vector2Int(localColumn, localRow),
			Chunk = chunk,
		};

		if (hexType == CellType.Yellow || hexType == CellType.Green)
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
				// Toggle
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

    private Vector2Int GetHexCoordinates(Vector3 worldPosition)
    {
		float x = worldPosition.x / HexSharedInfo.Instance.HexSize.x;
		float y = -x;

		float offset = worldPosition.y / (HexSharedInfo.Instance.HexSize.y / 2f * 3f);
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
			offset = HexSharedInfo.Instance.HexSize.x / 2f;
		}

		return new Vector2(
			offset + (chunkColumn * HexSharedInfo.Instance.HexSize.x),
			chunkRow * HexSharedInfo.Instance.RowHeight);
    }
}
