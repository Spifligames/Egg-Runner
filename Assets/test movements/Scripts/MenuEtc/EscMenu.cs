using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabMenu : MonoBehaviour
{
    public Canvas Canvas;
    bool CanvasActive;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CanvasActive = !CanvasActive;
        }

        if (CanvasActive)
            Canvas.enabled = true;

        if (!CanvasActive)
            Canvas.enabled = false;
    }
}
