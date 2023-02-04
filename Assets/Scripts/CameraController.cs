using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float speed = 100f;

    private Vector3 dragOrigin;

    private Rect mapBounds;

    private Vector2 cameraSize;

    private void Awake()
    {
        Camera camera = GetComponent<Camera>();
        float cameraHeight = camera.orthographicSize * 2f;
        cameraSize = new Vector2(
            cameraHeight * ((float)Screen.width / Screen.height),
            cameraHeight);
    }

    private void Start()
    {
        float offset = 5f;
        float mapWidth = WorldSettings.ChunkLength * HexSharedInfo.Instance.HexSize.x * WorldSettings.ChunksInRow;
        float mapHeight = 
            WorldSettings.ChunkLength *
            HexSharedInfo.Instance.RowHeight *
            WorldSettings.ChunksInRow;
        
        mapBounds = new Rect(
            -offset,
            -offset,
            mapWidth + 2 * offset,
            mapHeight + 2 * offset);
    }

    private void LateUpdate()
    {
        CameraMovement();
    }

    private void CameraMovement()
    {
        if (RightMouseButtonJustPressed())
        {
            dragOrigin = Input.mousePosition;
        }
        else if (RightMouseButtonHeldDown())
        {
            Vector3 position = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 movement = new Vector3(position.x, position.y, 0) * speed * Time.deltaTime;
            Vector3 newPosition = transform.position + movement;
            newPosition = ClampToMapBounds(newPosition);
            transform.position = newPosition;
        }

        bool RightMouseButtonHeldDown()
        {
            return Input.GetMouseButton(1);
        }

        bool RightMouseButtonJustPressed()
        {
            return Input.GetMouseButtonDown(1);
        }
    }

    private Vector3 ClampToMapBounds(Vector3 position)
    {
        float clampedX = Mathf.Clamp(position.x, mapBounds.min.x + cameraSize.x / 2, mapBounds.max.x - cameraSize.x / 2);
        float clampedY = Mathf.Clamp(position.y, mapBounds.min.y + cameraSize.y / 2, mapBounds.max.y - cameraSize.y / 2);
        return new Vector3(clampedX, clampedY, position.z);
    }
}
