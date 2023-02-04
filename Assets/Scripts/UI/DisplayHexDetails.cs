using TMPro;
using UnityEngine;

public class DisplayHexDetails : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI color;

    [SerializeField]
    private TextMeshProUGUI worldPosition;

    [SerializeField]
    private TextMeshProUGUI chunkCell;

    [SerializeField]
    private TextMeshProUGUI chunk;

    private HexDetails hexDetails;

    public HexDetails HexDetails
    {
        get => hexDetails;
        set
        {
            if (hexDetails != value)
            {
                hexDetails = value;
                UpdateValueDisplay();
            }
        }
    }

    private void UpdateValueDisplay()
    {
        color.text = "Color: " + hexDetails.HexType;
        worldPosition.text = "World position: " + hexDetails.WorldPosition;
        chunkCell.text = "Chunk cell: " + hexDetails.ChunkCell;
        chunk.text = "Chunk: " + hexDetails.Chunk;
    }
}
