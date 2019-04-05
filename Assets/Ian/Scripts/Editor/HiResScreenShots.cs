using UnityEngine;
using System.Collections;
using UnityEditor;

public class HiResScreenShots : MonoBehaviour
{
    [SerializeField]
    public static int resWidth = 1920;
    [SerializeField]
    public static int resHeight = 1080;

    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    [MenuItem("Window/ScreenShot")]
    static void TakeScreenShot()
    {
        ScreenCapture.CaptureScreenshot(ScreenShotName(resWidth, resHeight));
        Debug.Log(string.Format("Took screenshot to: {0}", ScreenShotName(resWidth, resHeight)));
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            ScreenCapture.CaptureScreenshot(ScreenShotName(resWidth, resHeight));
            Debug.Log(string.Format("Took screenshot to: {0}", ScreenShotName(resWidth, resHeight)));
        }
    }
}