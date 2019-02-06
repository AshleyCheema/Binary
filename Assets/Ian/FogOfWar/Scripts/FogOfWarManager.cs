using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarManager : MonoBehaviour
{


    #region Old
    /*
    [SerializeField]
    private int textureSize = 256;

    [SerializeField]
    private Color fogOfWarColour;

    [SerializeField]
    private LayerMask fogOfWarLayer;

    private Texture2D texture;
    private Color[] pixels;
    private List<FogOfWarRevealer> revealers;
    private int pixelsPerUnit;
    private Vector2 centerPixel;

    private static FogOfWarManager instance;
    public static FogOfWarManager Instance
    { get { return instance; } }

    private void Awake()
    {
        instance = this;

        Renderer renderer = GetComponent<Renderer>();
        Material fogOfWarMat = null;
        if(renderer != null)
        {
            fogOfWarMat = renderer.material;
        }

        if(fogOfWarMat == null)
        {
            Debug.LogError("Material for Fog Of War not found!");
            return;
        }

        texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        texture.wrapMode = TextureWrapMode.Clamp;

        pixels = texture.GetPixels();
        ClearPixels();

        fogOfWarMat.mainTexture = texture;

        revealers = new List<FogOfWarRevealer>();

        pixelsPerUnit = Mathf.RoundToInt(textureSize / transform.lossyScale.x);

        centerPixel = new Vector2(textureSize * 0.5f, textureSize * 0.5f);
    }

    public void RegisterRevealer(FogOfWarRevealer a_revealer)
    {
        revealers.Add(a_revealer);
    }

    private void ClearPixels()
    {
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = fogOfWarColour;
        }
    }

    /// <summary>
    /// Set the pixels in pixels to clear a circle
    /// </summary>
    /// <param name="a_originX"></param>
    /// <param name="a_originY"></param>
    /// <param name="a_radius"></param>
    private void CreateCircle(int a_originX, int a_originY, int a_radius)
    {
        for (var y = -a_radius * pixelsPerUnit; y <= a_radius * pixelsPerUnit; ++y)
        {
            for (var x = -a_radius * pixelsPerUnit; x <= a_radius * pixelsPerUnit; ++x)
            {
                if (x * x + y * y <= (a_radius * pixelsPerUnit) * (a_radius * pixelsPerUnit))
                {
                    pixels[(a_originY + y) * textureSize + a_originX + x] = new Color(0, 0, 0, 0);
                }
            }
        }
    }

    private void Update()
    {
        ClearPixels();

        foreach (FogOfWarRevealer revealer in revealers)
        {
            Camera mainCamera = Camera.main;
            //should do raycast from the revealer to the camera
            Vector3 screenPoint = mainCamera.WorldToScreenPoint(revealer.transform.position);
            Ray ray = mainCamera.ScreenPointToRay(screenPoint);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 1000, fogOfWarLayer.value))
            {
                //Translate the revealer to the center of the fog of war.
                //This way the position lines up with the center pixel and can be converted easier
                Vector3 translatedPos = hit.point - transform.position;

                int pixelPosX = Mathf.RoundToInt(translatedPos.x * pixelsPerUnit + centerPixel.x);
                int pixelPosY = Mathf.RoundToInt(translatedPos.z * pixelsPerUnit + centerPixel.y);

                Debug.Log(translatedPos);

                CreateCircle(pixelPosX, pixelPosY, revealer.radius);
            }
        }

        texture.SetPixels(pixels);
        texture.Apply(false);
    }
    */
    #endregion
}