using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshotter : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            ScreenCapture.CaptureScreenshot(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".png", 1);
            Debug.Log("Screenshot Saved!");
        }
    }
}
