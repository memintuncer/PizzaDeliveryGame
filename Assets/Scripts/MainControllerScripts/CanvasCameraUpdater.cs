using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Canvas Camera Updater updates the camera component of the Canvas component that is attached to a game object to whatever the main camera is.
/// </summary>
public class CanvasCameraUpdater : MonoBehaviour
{
    Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out canvas);
    }

    // Update is called once per frame
    void Update()
    {
        if (canvas != null) canvas.worldCamera = Camera.main;
    }
}
