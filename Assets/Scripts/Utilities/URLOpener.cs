using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLOpener : MonoBehaviour
{
    public static void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}
