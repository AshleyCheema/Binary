using UnityEngine;
using System.Collections;
using UnityEditor;

public class HiResScreenShots : MonoBehaviour
{
    [SerializeField]
    public static int resWidth = 1920;
    [SerializeField]
    public static int resHeight = 1080;

    private bool takeHiResShot = false;
    private Camera camera;

    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void TakeHiResShot()
    {
        takeHiResShot = true;
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
        return;
        if(camera == null)
        {
            camera = GetComponent<Camera>();
        }

        takeHiResShot |= Input.GetKeyDown("k");
        if (takeHiResShot)
        {
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            camera.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            camera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            camera.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(resWidth, resHeight);
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            takeHiResShot = false;
        }
    }
}