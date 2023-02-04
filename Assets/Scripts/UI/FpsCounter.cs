using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FpsCounter : MonoBehaviour
{
    private TextMeshProUGUI fps;

    private int currentFps;

    private void Awake()
    {
        fps = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        currentFps = (int)(1f / Time.unscaledDeltaTime);
        fps.text = "FPS: " + currentFps;
    }
}
