// Designed by Kinemation, 2023

using UnityEngine;

public class ScreenshotMaker : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ScreenCapture.CaptureScreenshot("scr.png", 2);
        }
    }
}
