using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;


[ExecuteInEditMode]
public class ReplcementShader : MonoBehaviour
{
    [SerializeField]
    private Shader shader;
    [SerializeField]
    private Material fow;

    [SerializeField]
    private RenderTexture renderTexture;

    Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        camera.SetReplacementShader(shader, "RenderType");

    }

    // Update is called once per frame
    void Update()
    {
        renderTexture = camera.targetTexture;
        Graphics.Blit(renderTexture, renderTexture, fow);
    }
}
