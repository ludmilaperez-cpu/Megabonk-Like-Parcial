using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class CameraScreenshot_4 : MonoBehaviour
{
    public Camera screenshotCamera;

    public int width = 1920;
    public int height = 1080;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame)
        {
            TakeScreenshot();
        }
    }

    void TakeScreenshot()
    {
        if (screenshotCamera == null)
        {
            Debug.LogError("Camera Selected");
            return;
        }

        RenderTexture renderTexture = new RenderTexture(width, height, 24);

        screenshotCamera.targetTexture = renderTexture;

        Texture2D screenshot = new Texture2D(
            width,
            height,
            TextureFormat.RGB24,
            false
        );

        screenshotCamera.Render();

        RenderTexture.active = renderTexture;

        screenshot.ReadPixels(
            new Rect(0, 0, width, height),
            0,
            0
        );

        screenshot.Apply();

        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;

        byte[] bytes = screenshot.EncodeToPNG();

        string path = Path.Combine(
            Application.dataPath,
            "Screenshot_" +
            System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") +
            ".png"
        );

        File.WriteAllBytes(path, bytes);

        Destroy(renderTexture);
        Destroy(screenshot);

        Debug.Log("Screenshot Saved: " + path);
    }
}