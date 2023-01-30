using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    TextMeshProUGUI fps;

    int currentFps;

    private void Awake()
    {
        fps = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    private void Update()
    {
        currentFps = (int)(1f / Time.unscaledDeltaTime);
        fps.text = "FPS: " + currentFps;
    }
}
