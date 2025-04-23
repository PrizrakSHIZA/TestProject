using System.IO;
using UnityEngine;

public class CreateScreenshot : MonoBehaviour
{
    [SerializeField] RenderTexture rt;

    Texture2D texture;

    private void TakeScreenshot()
    {
        texture = new Texture2D(rt.width, rt.height, TextureFormat.RGBA64, false);
        RenderTexture.active = rt;
        texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        texture.Apply();
        var path = "Assets/Images/temp.png";
        File.WriteAllBytes(path, texture.EncodeToPNG());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            TakeScreenshot();
    }
}