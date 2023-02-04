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

		int hexColumn = hexGlobalIndex % (WorldSettings.ChunksInRow * WorldSettings.ChunkLength);
		int hexRow = hexGlobalIndex / (WorldSettings.ChunksInRow * WorldSettings.ChunkLength);

		int localColumn = hexGlobalIndex % WorldSettings.ChunkLength;
		int localRow = (hexGlobalIndex / (WorldSettings.ChunksInRow * WorldSettings.ChunkLength)) % WorldSettings.ChunkLength;

		Vector2Int chunk = new Vector2Int(hexColumn / WorldSettings.ChunkLength, hexRow / WorldSettings.ChunkLength);

		ChunkInfo chunkInfo = mapGenerator.Chunks[chunk.x, chunk.y];
		CellType hexType = chunkInfo.HexType[localRow * WorldSettings.ChunkLength + localColumn];
		HexDetails hexDetails = new HexDetails()
		{
			GlobalIndex = hexGlobalIndex,
			HexType = hexType,
			WorldPosition = Chunk.GetHexPosition(hexColumn, hexRow),
			ChunkCell = new Vector2Int(localColumn, localRow),
			Chunk = chunk,
		};

		if (hexType == CellType.Yellow || hexType == CellType.Green)
        {
            if (NewCellClicked(hexDetails))
            {
                displayHexDetails.HexDetails = hexDetails;
                hexHighlight.transform.position = hexDetails.WorldPosition;
                SetUIActive(true);
            }
            else
            {
                ToggleUI();
            }
        }
        else if (hexDetailsPanel.activeInHierarchy)
        {
			SetUIActive(false);
		}

        bool NewCellClicked(HexDetails hexDetails)
        {
            return displayHexDetails.HexDetails == null || displayHexDetails.HexDetails.GlobalIndex != hexDetails.GlobalIndex;
        }
    }

    private Vector2Int GetHexCoordinates(Vector3 worldPosition)
    {
		// Code based on Hex Map tutorial (MIT-0 license) https://catlikecoding.com/unity/tutorials/hex-map/part-1/
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

	private void ToggleUI()
    {
		SetUIActive(!hexDetailsPanel.activeInHierarchy);
	}

	private void SetUIActive(bool active)
	{
		hexDetailsPanel.SetActive(active);
		hexHighlight.SetActive(active);
	}
}
