/*
 * Author: Ian Hudson
 * Description: Set a replacement shader to render to the screen
 * Created: 08/02/2019
 * Edited By: Ian
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;


[ExecuteInEditMode]
public class ReplcementShader : MonoBehaviour
{
    //Set the shader
    [SerializeField]
    private Shader shader;
    //Set the material
    [SerializeField]
    private Material fow;

    //Set the render texture to render to
    [SerializeField]
    private RenderTexture renderTexture;

    //Set the camera
    Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        //Assign the camera
        camera = GetComponent<Camera>();
        camera.SetReplacementShader(shader, "RenderType");

    }

    // Update is called once per frame
    void Update()
    {
        //Render to the render texture 
        renderTexture = camera.targetTexture;
        //Blit the render texture with fow material
        Graphics.Blit(renderTexture, renderTexture, fow);
    }
}
